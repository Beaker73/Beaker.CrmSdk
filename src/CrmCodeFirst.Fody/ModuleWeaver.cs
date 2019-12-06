using Fody;

using Mono.Cecil;

using System;
using System.Collections.Generic;

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
		Console.WriteLine("Called");

		foreach (TypeDefinition type in ModuleDefinition.Types)
		{
			foreach (MethodDefinition method in type.Methods)
			{
				Console.WriteLine(method.FullName);
			}
		}
	}

	/// <summary>
	/// 
	/// </summary>
	/// <returns></returns>
	public override IEnumerable<string> GetAssembliesForScanning()
	{
		yield break;
		//yield return "netstandard";
		//yield return "mscorlib";
		//yield return "System";
		//yield return "System.Runtime";
		//yield return "System.Core";
	}
}
