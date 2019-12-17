using Beaker.Crm.CodeFirst.Composition;
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
		: CrmEntity
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

		/// <summary>
		/// optional integer with a range
		/// </summary>
		[Range(50, 100)]
		public int? OptionalIntWithRange { get; set; }

		/// <summary>
		/// required integer with a range
		/// </summary>
		[Range(50, 100)]
		public int RequiredIntWithRange { get; set; }
	}
}
