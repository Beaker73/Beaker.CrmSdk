using System;

namespace Beaker.CrmSdk.Composition.Attributes
{
	/// <summary>
	///     Applies the executing stage to the step. If this attribute is ommited the step defaults to PostOperation (40)
	/// </summary>
	[AttributeUsage(AttributeTargets.Class)]
	public sealed class StageAttribute
		: Attribute
	{
		/// <summary>
		///     Initializes a new plugin step stage attribute
		/// </summary>
		public StageAttribute(PluginStage stage)
		{
			Stage = stage;
		}

		/// <summary>
		///     The stage the plugin step should execute during
		/// </summary>
		public PluginStage Stage { get; }

		/// <summary>
		///     For PostOperation it is possible to execute async by settings this option to true
		/// </summary>
		public bool Async { get; set; }
	}
}