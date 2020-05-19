using Beaker.CrmSdk.CodeFirst.Attributes;

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

[assembly: Publisher("bkr")]

namespace Beaker.CrmSdk.CodeFirst.SmokeTest.Entities
{
	/// <summary>
	/// Bus stop where messages from the bus can be delivered.
	/// </summary>
	[Entity]
	[Description("Bus stop where messages from the bus can be delivered. There can be multiple bus stops for different systems.")]
	public sealed class BusStop
		: CrmEntity
	{
		/// <summary>
		/// The name of the bus stop
		/// </summary>
		[StringLength(100)]
		[Description("The name of the bus stop")]
		public string Name { get; set; }
	}
}
