using Beaker.CrmSdk.CodeFirst.Attributes;
using System.ComponentModel;

namespace Beaker.CrmSdk.CodeFirst.SmokeTest.Entities
{
	/// <summary>
	/// Message arrived at the bus stop for processing
	/// </summary>
	[Entity]
	[Description("Message arrived at the bus stop for processing")]
	public sealed class BusMessage
		: CrmEntity
	{
	}
}
