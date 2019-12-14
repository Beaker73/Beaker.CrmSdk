using Fody;

using Mono.Cecil;
using Mono.Cecil.Cil;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;

/// <summary>
/// Weaver entry point
/// </summary>
public sealed partial class ModuleWeaver
	: BaseModuleWeaver
{
	private MethodReference _typeGetProperty;
	private MethodReference _typeGetTypeFromHandle;

	/// <summary>
	/// Execute the weaver
	/// </summary>
	public override void Execute()
	{
		// ensure there is a reference to the CrmSdk assembly
		AssemblyNameReference sdkRef = EnsureAssemblyReference("Microsoft.Xrm.Sdk", new Version(9, 0, 0, 0), new byte[] { 0x31, 0xbf, 0x38, 0x56, 0xad, 0x36, 0x4e, 0x35 });

		_typeGetProperty = ModuleDefinition.ImportReference(FindType("System.Type")
			.Methods
			.Where(m => m.Name == "GetProperty")
			.Where(m => m.Parameters.Count == 1)
			.Where(m => m.Parameters[0].ParameterType.Resolve() == ImportType<string>().Resolve())
			.Single());
		_typeGetTypeFromHandle = ModuleDefinition.ImportReference(FindType("System.Type")
			.Methods
			.Where(m => m.Name == "GetTypeFromHandle")
			.Where(m => m.Parameters.Count == 1)
			.Where(m => m.Parameters[0].ParameterType.Resolve() == ImportType<RuntimeTypeHandle>().Resolve())
			.Single());

		//var sdk = ResolveAssembly(sdkRef.Name);
		//if (sdk is null)
		//	throw new WeavingException("Failed to find Microsoft.Xrm.Sdk");
		//var elna = FindType("Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute");

		foreach (TypeDefinition type in ModuleDefinition.Types)
		{
			bool isCodeFirstEntity = type.CustomAttributes.Any(a => a.AttributeType.FullName == "Beaker.Crm.CodeFirst.Composition.Attributes.EntityAttribute");

			if (isCodeFirstEntity)
			{
				foreach (PropertyDefinition property in type.Properties)
				{
					string schemaName = "my_" + property.Name;
					string logicalName = schemaName.ToLowerInvariant();

					// add attribute for logical name
					AddAttribute(property, "Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute", logicalName);

					TypeDefinition baseType = EnsureBaseType(property.DeclaringType, "Microsoft.Xrm.Sdk.Entity");
					if (!(baseType is null) && property.GetMethod.IsPublic && property.SetMethod.IsPublic)
					{
						// must be auto getter setter, so lets find the matching backing field
						// and remove it, since Attributes collection will be the backing
						FieldDefinition backing = type.Fields.SingleOrDefault(fd => fd.Name == $"<{property.Name}>k__BackingField");
						if (!(backing is null))
						{
							type.Fields.Remove(backing);

							RebuildGetter(type, property, logicalName);
							RebuildSetter(type, property, logicalName);
						}
					}
				}
			}
		}
	}

	private void RebuildGetter(TypeDefinition entityType, PropertyDefinition property, string logicalName)
	{
		property.GetMethod.Body.Instructions.Clear();
		ILProcessor processor = property.GetMethod.Body.GetILProcessor();

		AddAttribute(property.GetMethod, "System.Runtime.CompilerServices.CompilerGeneratedAttribute");

		GenericInstanceType aspectType = MakeGenericType("Beaker.Crm.CodeFirst.Composition.CodeFirstAspect", entityType);
		bool isValueType = property.PropertyType.IsValueType;
		bool isNullable = IsNullableType(property.PropertyType, out TypeReference nonNullableValueType);

		// this
		processor.Emit(OpCodes.Ldarg_0);
		// property info
		processor.Emit(OpCodes.Ldtoken, entityType);
		processor.Emit(OpCodes.Call, _typeGetTypeFromHandle);
		processor.Emit(OpCodes.Ldstr, property.Name);
		processor.Emit(OpCodes.Call, _typeGetProperty);
		// attribute name
		processor.Emit(OpCodes.Ldstr, logicalName);
		// call 
		MethodReference getValueMethod = GetMethod(aspectType, $"Get{AspectTypeName(isValueType, isNullable)}Attribute");
		GenericInstanceMethod typedGetValueMethod = new GenericInstanceMethod(getValueMethod);
		typedGetValueMethod.GenericArguments.Add(isNullable ? nonNullableValueType : property.PropertyType);
		processor.Emit(OpCodes.Call, typedGetValueMethod);
		// return the result
		processor.Emit(OpCodes.Ret);
	}

	private void RebuildSetter(TypeDefinition entityType, PropertyDefinition property, string logicalName)
	{
		property.SetMethod.Body.Instructions.Clear();
		ILProcessor processor = property.SetMethod.Body.GetILProcessor();

		AddAttribute(property.SetMethod, "System.Runtime.CompilerServices.CompilerGeneratedAttribute");

		GenericInstanceType aspectType = MakeGenericType("Beaker.Crm.CodeFirst.Composition.CodeFirstAspect", entityType);
		bool isValueType = property.PropertyType.IsValueType;
		bool isNullable = IsNullableType(property.PropertyType, out TypeReference nonNullableValueType);

		// this
		processor.Emit(OpCodes.Ldarg_0);
		// property info
		processor.Emit(OpCodes.Ldtoken, entityType);
		processor.Emit(OpCodes.Call, _typeGetTypeFromHandle);
		processor.Emit(OpCodes.Ldstr, property.Name);
		processor.Emit(OpCodes.Call, _typeGetProperty);
		// attribute name
		processor.Emit(OpCodes.Ldstr, logicalName);
		// value
		processor.Emit(OpCodes.Ldarg_1);
		// call 
		MethodReference setValueMethod = GetMethod(aspectType, $"Set{AspectTypeName(isValueType, isNullable)}Attribute");
		GenericInstanceMethod typedSetValueMethod = new GenericInstanceMethod(setValueMethod);
		typedSetValueMethod.GenericArguments.Add(isNullable ? nonNullableValueType : property.PropertyType);
		processor.Emit(OpCodes.Call, typedSetValueMethod);
		// return
		processor.Emit(OpCodes.Ret);
	}

	private string AspectTypeName(bool isValueType, bool isNullable)
		=> isValueType ? (isNullable ? "NullableValueType" : "ValueType") : "ReferenceType";

	private TypeReference ImportType<T>()
	{
		return ModuleDefinition.ImportReference(typeof(T));
	}

	private TypeReference ImportType(TypeReference typeRef)
	{
		return ModuleDefinition.ImportReference(typeRef);
	}


	private TypeDefinition EnsureBaseType(TypeDefinition type, string typeName)
	{
		if (type.FullName == typeName)
			return type;
		if (type.BaseType is null)
			return null;

		TypeDefinition baseType = type.BaseType.Resolve();
		return EnsureBaseType(baseType, typeName);
	}

	private AssemblyNameReference EnsureAssemblyReference(string name, Version version, byte[] token = null)
	{
		// try to find all existing reference to the assembly
		List<AssemblyNameReference> references = ModuleDefinition.AssemblyReferences
			.Where(ar => ar.Name == name)
			.Where(ar => (token == null && ar.PublicKey == null)
				|| (token != null && token.Equals(ar.PublicKeyToken)))
			.ToList();

		// none found, add it
		if (references.Count == 0)
		{
			AssemblyNameReference sdkRef = new AssemblyNameReference(name, version) { PublicKeyToken = token };
			ModuleDefinition.AssemblyReferences.Add(sdkRef);
			references.Add(sdkRef);
		}
		// multiple found, fail
		else if (references.Count > 1)
		{
			throw new WeavingException($"Multipe references to the {name} assembly found");
		}

		// return the one single reference there should be now
		return references.Single();
	}

	private void ApplyAttribute<TAttr>(PropertyDefinition property, params object[] args)
		where TAttr : Attribute
	{
		Type[] argTypes = args.Select(a => a.GetType()).ToArray();
		MethodReference methodReference = ModuleDefinition.ImportReference(typeof(TAttr).GetConstructor(argTypes));
		property.CustomAttributes.Add(new CustomAttribute(methodReference));
	}

	/// <summary>
	/// 
	/// </summary>
	/// <returns></returns>
	public override IEnumerable<string> GetAssembliesForScanning()
	{
		yield return "Microsoft.Xrm.Sdk";
		yield return "Beaker.Crm.CodeFirst.Composition";
		//yield return "netstandard";
		//yield return "mscorlib";
		//yield return "System";
		//yield return "System.Runtime";
		//yield return "System.Core";
	}
}
