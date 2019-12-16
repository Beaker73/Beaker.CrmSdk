using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Workflow;

using System;
using System.Activities;
using System.Activities.DurableInstancing;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Security;
using System.Security.Permissions;
using System.Security.Policy;
using System.ServiceModel.Activities;
using System.Text.RegularExpressions;

using static System.FormattableString;

namespace Beaker.Crm.CodeFirst.SmokeTest.Tests.Builders
{
	/// <summary>
	/// Builds a sandbox so plugin steps can be tested using the same restrictions as real sandboxes
	/// </summary>
	public sealed class SandboxBuilder
	{
		/// <summary>
		/// Builds the sandbox as configured
		/// </summary>
		/// <returns></returns>
		public ISandbox Build()
		{
			Assembly[] trusted = new[] {
				typeof(Entity).Assembly
			};

			Assembly[] visibleTo = new[] {
				typeof(CodeActivity).Assembly,
				typeof(WorkflowService).Assembly,
				typeof(InstanceEncodingOption).Assembly,
				typeof(InputAttribute).Assembly,
			};

			return new Sandbox(trusted, visibleTo);
		}
	}

	/// <summary>
	/// Creates a sandbox to run code in
	/// </summary>
	public sealed class Sandbox
		: ISandbox
	{
		private readonly ImmutableList<Assembly> _trustedAssemblies;
		private readonly ImmutableList<Assembly> _visibleToAssemblies;
		private readonly AppDomain _appDomain;

		/// <summary>
		/// Initializes a new sandbox.
		/// </summary>
		/// <param name="trustedAssemblies">The list of trusted assemblies the code can use.</param>
		/// <param name="visibleTo">The list of assemblies the code is visible to.</param>
		public Sandbox(IEnumerable<Assembly> trustedAssemblies, IEnumerable<Assembly> visibleTo)
		{
			_trustedAssemblies = trustedAssemblies?.ToImmutableList() ?? ImmutableList<Assembly>.Empty;
			_visibleToAssemblies = visibleTo?.ToImmutableList() ?? ImmutableList<Assembly>.Empty;
			_appDomain = CreateAppDomain();
		}

		private AppDomain CreateAppDomain()
		{
			AppDomainSetup setup = CreateAppDomainSetup();
			PermissionSet permissions = CreatePermissionSet();
			StrongName[] trustedAssemblyNames = _trustedAssemblies.Select(CreateStrongName).ToArray();
			return AppDomain.CreateDomain("UnitTest-Domain", null, setup, permissions, trustedAssemblyNames);
		}

		// the regex for uri matching as used by CRM
		private static readonly Regex _allowedUris =
			new Regex(@"^http[s]?://(?!((localhost[:/])|(\\[.*\\])|([0-9]+[:/])|(0x[0-9a-f]+[:/])|(((([0-9]+)|(0x[0-9A-F]+))\\.){3}(([0-9]+)|(0x[0-9A-F]+))[:/]))).+", RegexOptions.IgnoreCase | RegexOptions.Compiled | RegexOptions.Singleline | RegexOptions.CultureInvariant);

		private AppDomainSetup CreateAppDomainSetup()
		{
			AppDomainSetup setup = new AppDomainSetup();
			setup.ApplicationBase = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("n"));
			setup.PartialTrustVisibleAssemblies = _visibleToAssemblies.Select(CreateAssemblyName).ToArray();
			return setup;
		}

		/// <summary>
		/// Create a strong name from the provided <paramref name="assembly"/>.
		/// </summary>
		/// <param name="assembly">The assembly to create a strong name for.</param>
		/// <returns>The strong name for the requested assembly.</returns>
		private StrongName CreateStrongName(Assembly assembly)
		{
			AssemblyName assemblyName = assembly.GetName();
			StrongNamePublicKeyBlob blob = new StrongNamePublicKeyBlob(assemblyName.GetPublicKey());
			return new StrongName(blob, assemblyName.Name, assemblyName.Version);
		}

		private string CreateAssemblyName(Assembly assembly)
		{
			StrongName strongName = CreateStrongName(assembly);
			AssemblyName assemblyName = assembly.GetName();
			return Invariant($"{assemblyName.Name}, PublicKey={strongName.PublicKey}");
		}

		/// <summary>
		/// Creates a permission set that should match what Dynamics CRM Plugins receive (basic execute rights and web access except localhost)
		/// </summary>
		/// <returns>The created permission set</returns>
		private PermissionSet CreatePermissionSet()
		{
			PermissionSet permissions = new PermissionSet(PermissionState.None);
			permissions.AddPermission(new SecurityPermission(SecurityPermissionFlag.Execution));
			permissions.AddPermission(new WebPermission(NetworkAccess.Connect, _allowedUris));
			return permissions;
		}

		/// <summary>
		/// Execute the code in the sandbox
		/// </summary>
		public void Execute()
		{
			throw new NotImplementedException();
		}
	}

	/// <summary>
	/// Interface to run the code in the sandbox
	/// </summary>
	public interface ISandbox
	{
		/// <summary>
		/// Execute the code in the sandbox
		/// </summary>
		void Execute();
	}
}
