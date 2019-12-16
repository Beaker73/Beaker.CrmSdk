using Microsoft.Xrm.Sdk;

namespace Beaker.Crm.CodeFirst.Composition
{
	/// <summary>
	///		Step to be executed as a plugin inside CRM
	/// </summary>
	public interface IPluginStep
	{
		/// <summary>
		///		Called to execute the step
		/// </summary>
		/// <param name="context">The plugin execution context</param>
		void Execute(IPluginExecutionContext context);
	}
}
