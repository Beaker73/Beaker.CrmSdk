using System;
using System.Collections.Immutable;

namespace Beaker.Crm.CodeFirst.Composition.Attributes
{
	/// <summary>
	/// Marks the fields the registered pre-image should contain
	/// </summary>
	[AttributeUsage(AttributeTargets.Class)]
	public sealed class PostImageFieldsAttribute
		: Attribute
	{
		/// <summary>
		/// Initializes a new post image field attribute
		/// </summary>
		/// <param name="codeFirstPropertyNames">The code first property names that should be part of the PostImage</param>
		public PostImageFieldsAttribute(params string[] codeFirstPropertyNames)
		{
			CodeFirstPropertyNames = codeFirstPropertyNames?.ToImmutableList() ?? ImmutableList<string>.Empty;
		}

		/// <summary>
		/// The list of property names of the code first entity
		/// </summary>
		public IImmutableList<string> CodeFirstPropertyNames { get; }
	}
}
