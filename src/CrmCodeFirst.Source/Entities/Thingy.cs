using System.ComponentModel.DataAnnotations;

namespace CrmCodeFirst.Source.Entities
{
	/// <summary>
	/// A Thingy
	/// </summary>
	public sealed class Thingy
	{
		/// <summary>
		/// Name of the thingy
		/// </summary>
		[StringLength(100)]
		public string OptionalWithStringMaxLength { get; set; }
	}
}
