using Beaker.Crm.CodeFirst.Composition;
using Beaker.Crm.CodeFirst.Composition.Attributes;
using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beaker.Crm.CodeFirst.SmokeTest.Entities
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
		public int? CharactersInName { get; set; }
	}
}
