using Microsoft.Xrm.Sdk;

using System;

namespace Beaker.Crm.CodeFirst.Composition
{
	/// <summary>
	/// Base class for plugins that will use Composition and CodeFirst
	/// </summary>
	public abstract class CodeFirstPlugin
		: IPlugin
	{
		void IPlugin.Execute(IServiceProvider serviceProvider)
		{
		}
	}
}
