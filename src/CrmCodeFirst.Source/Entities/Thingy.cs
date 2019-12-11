using Beaker.Crm.CodeFirst.Composition.Attributes;
using Microsoft.Xrm.Sdk;
using System.ComponentModel.DataAnnotations;

namespace CrmCodeFirst.Source.Entities
{
	/// <summary>
	/// A Thingy
	/// </summary>
	[Entity]
	public sealed class Thingy
		: Entity
	{
		/// <summary>
		/// optional string value with a max length
		/// </summary>
		[StringLength(100)]
		public string OptionalWithStringMaxLength { get; set; }

		/// <summary>
		/// required string value with a max length
		/// </summary>
		[StringLength(50)]
		[Required]
		public string RequiredWithStringMaxLength { get; set; }
	}
}
