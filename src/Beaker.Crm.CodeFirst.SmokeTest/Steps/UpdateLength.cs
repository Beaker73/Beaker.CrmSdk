using Beaker.Crm.CodeFirst.Composition;
using Beaker.Crm.CodeFirst.Composition.Attributes;
using Beaker.Crm.CodeFirst.SmokeTest.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beaker.Crm.CodeFirst.SmokeTest.Steps
{
	/// <summary>
	/// Step that sets the <see cref="Thingy.CharactersInName"/> based on the <see cref="Thingy.Name"/> field.
	/// </summary>
	[Message("Create", "Update")]
	[Target(typeof(Thingy))]
	[FilterFields(nameof(Thingy.Name))]
	public sealed class UpdateLength
		: PluginStep<Thingy>
	{
		/// <summary>
		/// Called when the name property has changed
		/// </summary>
		/// <param name="target">The target thingy that has been created/updated</param>
		protected override void Execute(Thingy target)
		{
			target.CharactersInName = target.Name?.Length ?? 0;
		}
	}
}
