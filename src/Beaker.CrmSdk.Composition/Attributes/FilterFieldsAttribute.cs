using System;
using System.Collections.Immutable;

namespace Beaker.Crm.CodeFirst.Composition.Attributes
{
	/// <summary>
	/// Marks the fields the plugin step shoud filter on
	/// </summary>
	[AttributeUsage(AttributeTargets.Class)]
	public sealed class FilterFieldsAttribute
		: Attribute
	{
		/// <summary>
		/// Initializes a new filter field attribute
		/// </summary>
		/// <param name="codeFirstPropertyNames">The code first property names that should update to trigger the step</param>
		public FilterFieldsAttribute(params string[] codeFirstPropertyNames)
		{
			CodeFirstPropertyNames = codeFirstPropertyNames?.ToImmutableList() ?? ImmutableList<string>.Empty;
		}

		/// <summary>
		/// The list property names of the code first entity
		/// </summary>
		public IImmutableList<string> CodeFirstPropertyNames { get; }
	}
}
