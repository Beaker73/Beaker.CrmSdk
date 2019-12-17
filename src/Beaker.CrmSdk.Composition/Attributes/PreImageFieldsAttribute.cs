using System;
using System.Collections.Immutable;

namespace Beaker.CrmSdk.Composition.Attributes
{
	/// <summary>
	/// Marks the fields the registered pre-image should contain
	/// </summary>
	[AttributeUsage(AttributeTargets.Class)]
	public sealed class PreImageFieldsAttribute
		: Attribute
	{
		/// <summary>
		/// Initializes a new pre image field attribute
		/// </summary>
		/// <param name="codeFirstPropertyNames">The code first property names that should be part of the PreImage</param>
		public PreImageFieldsAttribute(params string[] codeFirstPropertyNames)
		{
			CodeFirstPropertyNames = codeFirstPropertyNames?.ToImmutableList() ?? ImmutableList<string>.Empty;
		}

		/// <summary>
		/// The list of property names of the code first entity
		/// </summary>
		public IImmutableList<string> CodeFirstPropertyNames { get; }
	}
}
