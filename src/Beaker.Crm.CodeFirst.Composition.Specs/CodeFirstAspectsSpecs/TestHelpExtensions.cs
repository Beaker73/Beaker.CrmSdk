using Microsoft.Xrm.Sdk;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace Beaker.Crm.CodeFirst.Composition.Specs.CodeFirstAspectsSpecs
{
	/// <summary>
	/// Extensions to help with testing
	/// </summary>
	public static class TestHelpExtensions
	{
		/// <summary>
		/// Convert DataCollection to Dictionary for easy testing, (why does it not implement IDictionary?)
		/// </summary>
		/// <param name="collection">The collection to convert</param>
		/// <returns>The collection as a dictionary</returns>
		public static IDictionary<TKey, TValue> ToDictionary<TKey, TValue>(this DataCollection<TKey, TValue> collection)
		{
			var builder = ImmutableDictionary.CreateBuilder<TKey, TValue>();
			foreach (var kv in collection)
				builder.Add(kv.Key, kv.Value);
			return builder.ToImmutable();
		}
	}
}
