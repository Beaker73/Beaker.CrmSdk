using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beaker.Crm.CodeFirst.Composition.Attributes
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
