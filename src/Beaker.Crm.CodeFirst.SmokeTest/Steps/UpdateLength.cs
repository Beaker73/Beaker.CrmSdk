using Beaker.Crm.CodeFirst.Composition;
using Beaker.Crm.CodeFirst.Composition.Attributes;
using Beaker.Crm.CodeFirst.SmokeTest.Entities;
using System;

namespace Beaker.Crm.CodeFirst.SmokeTest.Steps
{
	/// <summary>
	/// Step that sets the <see cref="Thingy.CharactersInName"/> based on the <see cref="Thingy.Name"/> field.
	/// </summary>
	[Message("Create", "Update")]
	[Target(typeof(Thingy))] // TODO: Auto inject/weave using fody, or hints via custom analyzer? (based on type of PluginStep<T> used)
	[FilterFields(nameof(Thingy.Name))] // TODO: Auto inject/weave using fody, or hints via custom analyzer? (based on accessed fields on target)
	[PreImageFields(nameof(Thingy.Name))] // TODO: Auto inject/weave using fody, or hints via custom analyzer? (based on accessed fields on PreImage property)
	[Stage(PluginStage.PreOperation)]
	public sealed class UpdateLength
		: PluginStep<Thingy>
	{
		/// <summary>
		/// 	Called when the name property has changed
		/// </summary>
		/// <param name="target">The target thingy that has been created/updated</param>
		protected override void Execute(Thingy target)
		{
			int oldLength = PreImage.Name?.Length ?? 0;
			int newLength = target.Name?.Length ?? 0;
			target.CharactersInName = newLength;
			target.CharacterDifWithPreviousName = Math.Abs(newLength - oldLength);
		}
	}
}
