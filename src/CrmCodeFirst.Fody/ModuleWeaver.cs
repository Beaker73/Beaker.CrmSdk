using Fody;

using Mono.Cecil;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

/// <summary>
/// Weaver entry point
/// </summary>
public sealed class ModuleWeaver
	: BaseModuleWeaver
{
	/// <summary>
	/// Execute the weaver
	/// </summary>
	public override void Execute()
	{
		// ensure there is a reference to the CrmSdk assembly
		var sdkRef = EnsureAssemblyReference("Microsoft.Xrm.Sdk", new Version(9, 0, 0, 0), new byte[] { 0x31, 0xbf, 0x38, 0x56, 0xad, 0x36, 0x4e, 0x35 });

		var sdk = ResolveAssembly(sdkRef.Name);
		if (sdk is null)
			throw new WeavingException("Failed to find Microsoft.Xrm.Sdk");
		var elna = FindType("Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute");

		foreach (TypeDefinition type in ModuleDefinition.Types)
		{
			bool isCodeFirstEntity = type.CustomAttributes.Any(a => a.AttributeType.FullName == "Beaker.Crm.CodeFirst.Composition.Attributes.EntityAttribute");

			foreach (PropertyDefinition property in type.Properties)
			{
				//ApplyAttribute(property, attributeLogicalName, "bcf_", property.Name.ToLowerInvariant());
			}
		}
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
