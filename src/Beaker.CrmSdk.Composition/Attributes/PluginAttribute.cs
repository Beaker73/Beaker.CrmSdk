using System.Composition;

namespace Beaker.CrmSdk.Composition.Attributes
{
	/// <summary>
	/// Attribute to export the Plugin
	/// </summary>
	public sealed class PluginAttribute
		: ExportAttribute
	{
		/// <summary>
		/// Initializes a new plugin
		/// </summary>
		public PluginAttribute()
			: base(typeof(IPluginStep))
		{
		}
	}
}
