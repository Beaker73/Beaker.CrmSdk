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
	/// <summary>
	/// Execute the weaver
	/// </summary>
	public override void Execute()
	{
		// ensure there is a reference to the CrmSdk assembly
		var sdkRef = EnsureAssemblyReference("Microsoft.Xrm.Sdk", new Version(9, 0, 0, 0), new byte[] { 0x31, 0xbf, 0x38, 0x56, 0xad, 0x36, 0x4e, 0x35 });

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
					bool isRequired = property.CustomAttributes.Any(a => a.AttributeType.FullName == "System.ComponentModel.DataAnnotations.RequiredAttribute") || property.PropertyType.IsValueType;
					string schemaName = "my_" + property.Name;
					string logicalName = schemaName.ToLowerInvariant();

					// add attribute for logical name
					AddAttribute(property, "Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute", logicalName);

					var baseType = EnsureBaseType(property.DeclaringType, "Microsoft.Xrm.Sdk.Entity");
					if (!(baseType is null) && property.GetMethod.IsPublic && property.SetMethod.IsPublic)
					{
						// must be auto getter setter, so lets find the matching backing field
						// and remove it, since Attributes collection will be the backing
						var backing = type.Fields.SingleOrDefault(fd => fd.Name == $"<{property.Name}>k__BackingField");
						if (!(backing is null))
						{
							type.Fields.Remove(backing);

							property.GetMethod.Body.Instructions.Clear();
							//property.GetMethod.Body.Variables.Add(new VariableDefinition(Import<bool>()));
							//property.GetMethod.Body.Variables.Add(new VariableDefinition(Import<string>()));
							AddAttribute(property.GetMethod, "System.Runtime.CompilerServices.CompilerGeneratedAttribute");
							var processor = property.GetMethod.Body.GetILProcessor();

							// get Attributes collection via Attributes property
							var attrProp = baseType.Properties.Single(p => p.Name == "Attributes");
							var getAttrProp = ModuleDefinition.ImportReference(attrProp.GetMethod);
							processor.Emit(OpCodes.Ldarg_0); // param0: this
							processor.Emit(OpCodes.Call, getAttrProp); // call get_Attributes

							// test if logical name exists in the attributes collection
							var dataCollectionType = MakeGenericType<string, object>("Microsoft.Xrm.Sdk.DataCollection");
							var containsKey = GetMethod<bool, string>(dataCollectionType, "ContainsKey");
							processor.Emit(OpCodes.Ldstr, logicalName); // param0: logicalName
							processor.Emit(OpCodes.Callvirt, containsKey); // call ContainsKey
							//processor.Emit(OpCodes.Stloc_0); // var0 = bool result

							// test if result is false, i.e. no key available
							// jump to null return if so.
							//processor.Emit(OpCodes.Ldloc_0);
							var noValueLabel = processor.Create(OpCodes.Nop);
							processor.Emit(OpCodes.Brfalse_S, noValueLabel);

							// return value via indexer on collection
							var getItem = GetMethod<object, string>(dataCollectionType, "get_Item");
							processor.Emit(OpCodes.Ldarg_0);
							processor.Emit(OpCodes.Call, getAttrProp); // call get_Attributes
							processor.Emit(OpCodes.Ldstr, logicalName); // param0: logicalName
							processor.Emit(OpCodes.Callvirt, getItem); // call ContainsKey
							processor.Emit(OpCodes.Castclass, property.PropertyType);
							//processor.Emit(OpCodes.Stloc_1); // var1 = return value

							// return the return value
							//processor.Emit(OpCodes.Ldloc_1); // get return value
							processor.Emit(OpCodes.Ret); ; // return it

							// not found, throw or return null
							processor.Append(noValueLabel);
							if (isRequired)
							{
								var knfe = FindType("System.Collections.Generic.KeyNotFoundException");
								var ctor = GetConstructor<string>(knfe);
								processor.Emit(OpCodes.Ldstr, $"The {property.Name} attribute is missing");
								processor.Emit(OpCodes.Newobj, ctor);
								processor.Emit(OpCodes.Throw);
							}
							else
							{
								processor.Emit(OpCodes.Ldnull);
								processor.Emit(OpCodes.Ret);
							}

							property.SetMethod.Body.Instructions.Clear();
							processor = property.SetMethod.Body.GetILProcessor();
							processor.Emit(OpCodes.Ret);
						}
					}
					//ApplyAttribute(property, attributeLogicalName, "bcf_", property.Name.ToLowerInvariant());
				}
			}
		}
	}

	private TypeReference ImportType<T>()
	{
		return ModuleDefinition.ImportReference(typeof(T));
	}

	private TypeDefinition EnsureBaseType(TypeDefinition type, string typeName)
	{
		if (type.FullName == typeName)
			return type;
		if (type.BaseType is null)
			return null;

		var baseType = type.BaseType.Resolve();
		return EnsureBaseType(baseType, typeName);
	}

	private AssemblyNameReference EnsureAssemblyReference(string name, Version version, byte[] token = null)
	{
		// try to find all existing reference to the assembly
		var references = ModuleDefinition.AssemblyReferences
			.Where(ar => ar.Name == name)
			.Where(ar => (token == null && ar.PublicKey == null)
				|| (token != null && token.Equals(ar.PublicKeyToken)))
			.ToList();

		// none found, add it
		if (references.Count == 0)
		{
			var sdkRef = new AssemblyNameReference(name, version) { PublicKeyToken = token };
			ModuleDefinition.AssemblyReferences.Add(sdkRef);
			references.Add(sdkRef);
		}
		// multiple found, fail
		else if (references.Count > 1)
			throw new WeavingException($"Multipe references to the {name} assembly found");

		// return the one single reference there should be now
		return references.Single();
	}

	private void ApplyAttribute<TAttr>(PropertyDefinition property, params object[] args)
		where TAttr : Attribute
	{
		var argTypes = args.Select(a => a.GetType()).ToArray();
		var methodReference = ModuleDefinition.ImportReference(typeof(TAttr).GetConstructor(argTypes));
		property.CustomAttributes.Add(new CustomAttribute(methodReference));
	}

	/// <summary>
	/// 
	/// </summary>
	/// <returns></returns>
	public override IEnumerable<string> GetAssembliesForScanning()
	{
		yield return "Microsoft.Xrm.Sdk";
		//yield return "netstandard";
		//yield return "mscorlib";
		//yield return "System";
		//yield return "System.Runtime";
		//yield return "System.Core";
	}
}
