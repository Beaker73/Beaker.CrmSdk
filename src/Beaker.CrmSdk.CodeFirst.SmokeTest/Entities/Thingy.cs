using Beaker.CrmSdk.Composition;
using Beaker.CrmSdk.Composition.Attributes;

using Microsoft.Xrm.Sdk;

using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Beaker.CrmSdk.CodeFirst.SmokeTest.Entities
{
	/// <summary>
	/// The thingy entity
	/// </summary>
	[Entity]
	public sealed class Thingy
		: CrmEntity
	{
		/// <summary>
		/// Name of the thingy
		/// </summary>
		[Required]
		[StringLength(50)]
		[Description("Name of the thingy")]
		public string Name { get; set; }

		/// <summary>
		/// Number of characters in the name of the thingy
		/// </summary>
		[Range(0, Int32.MaxValue)]
		[Description("Number of characters in the name of the thingy")]
		public int CharactersInName { get; set; }

		/// <summary>
		/// Difference between number of characters in the name between current and previous name
		/// </summary>
		[Range(0, Int32.MaxValue)]
		[Description("Difference between number of characters in the name between current and previous name")]
		public int? CharacterDifWithPreviousName { get; set; }
	}
}
