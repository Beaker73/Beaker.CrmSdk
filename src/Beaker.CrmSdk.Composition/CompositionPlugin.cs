using Microsoft.Xrm.Sdk;

using System;

namespace Beaker.CrmSdk.Composition
{
	/// <summary>
	/// Base class for plugins that will use Composition via MEF
	/// </summary>
	public abstract class CompositionPlugin
		: IPlugin
	{
		void IPlugin.Execute(IServiceProvider serviceProvider)
		{
		}
	}
}
