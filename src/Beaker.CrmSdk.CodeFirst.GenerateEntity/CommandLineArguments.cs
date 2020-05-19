using CommandLine;

using System.Collections.Generic;
using System.Collections.Immutable;

namespace Beaker.CrmSdk.CodeFirst.GenerateEntity
{
	/// <summary>
	/// The command line arguments passed to the application
	/// </summary>
	public sealed class CommandLineArguments
	{
		/// <summary>
		/// Initializes a new command line arguments instance
		/// </summary>
		/// <param name="assemblyPaths">The provided paths to assemblies</param>
		/// <param name="destinationPath">The destination path to generate to</param>
		public CommandLineArguments(IEnumerable<string> assemblyPaths, string destinationPath)
		{
			AssemblyPaths = assemblyPaths?.ToImmutableList() ?? ImmutableList<string>.Empty;
			DestinationPath = destinationPath;
		}

		/// <summary>
		/// Paths to assemblies to scan for entities
		/// </summary>
		[Value(0)]
		public IImmutableList<string> AssemblyPaths { get; }

		/// <summary>
		/// The destination path to generate to
		/// </summary>
		[Option('d', "destination", Required = true, HelpText = "The destination folder to generate the entities to")]
		public string DestinationPath { get; }
	}
}
