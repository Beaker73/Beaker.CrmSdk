using Microsoft.Xrm.Sdk;

using System;
using System.Reflection;

namespace Beaker.Crm.CodeFirst.SmokeTest.Tests.Builders
{
	/// <summary>
	/// Builds a sandbox so plugin steps can be tested using the same restrictions as real sandboxes
	/// </summary>
	public sealed class SandboxBuilder
	{
		private Type _pluginType;

		/// <summary>
		/// Set up the builder to create a sandbox for the provided <typeparamref name="TPlugin"/> type.
		/// </summary>
		/// <typeparam name="TPlugin">The type of the plugin to create a sandbox for.</typeparam>
		public SandboxBuilder ForPlugin<TPlugin>()
			where TPlugin : IPlugin
		{
			_pluginType = typeof(TPlugin);
			return this;
		}

		/// <summary>
		/// Builds the sandbox as configured
		/// </summary>
		/// <returns></returns>
		public ISandbox Build()
		{
			Assembly[] trusted = new[] {
				typeof(Entity).Assembly
			};

			Assembly[] visibleTo = new Assembly[0];

			return new Sandbox(_pluginType ?? typeof(DummyPlugin), trusted, visibleTo);
		}
	}
}
