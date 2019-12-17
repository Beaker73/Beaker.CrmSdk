using System;

namespace Beaker.CrmSdk.Composition.Attributes
{
	/// <summary>
	/// Mark the entity as code first and to be exported to CRM
	/// </summary>
	[AttributeUsage(AttributeTargets.Class)]
	public sealed class EntityAttribute
		: Attribute
	{
	}
}
