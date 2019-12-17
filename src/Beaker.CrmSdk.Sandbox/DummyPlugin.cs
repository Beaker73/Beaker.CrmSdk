using Microsoft.Xrm.Sdk;
using System;

namespace Beaker.CrmSdk.CodeFirst.SmokeTest.Tests.Builders
{
	/// <summary>
	/// Dummy plugin in case user did not configure an entry point
	/// </summary>
	public sealed class DummyPlugin
		: IPlugin
	{
		/// <summary>
		/// Dummy that does nothing when called
		/// </summary>
		/// <param name="serviceProvider"></param>
		public void Execute(IServiceProvider serviceProvider)
		{
		}
	}
}
