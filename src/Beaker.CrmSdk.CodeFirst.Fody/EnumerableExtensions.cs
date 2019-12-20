using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Beaker.CrmSdk.CodeFirst.Fody
{
	/// <summary>
	/// Extensionsion on enumerable
	/// </summary>
	public static class EnumerableExtensions
	{
		/// <summary>
		/// Takes the single expected item. Throws when none or multiple found giving the <paramref name="error"/> as feedback.
		/// </summary>
		/// <typeparam name="T">The type of the items in the sequence.</typeparam>
		/// <param name="sequence">The 'sequence' to get the item from.</param>
		/// <param name="error">The error message to throw if not exactly one item</param>
		/// <returns>The single item in the 'sequence'.</returns>
		public static T SingleOrException<T>(this IEnumerable<T> sequence, string error)
		{
			T[] items = sequence.Take(2).ToArray();
			if (items.Length == 0)
				throw new InvalidOperationException($"Found no items: " + error);
			if (items.Length > 1)
				throw new InvalidOperationException($"Found multiple items: " + error);

			return items[0];
		}

		/// <summary>
		/// Takes the single expected item. Throws when none or multiple found giving the <paramref name="error"/> as feedback.
		/// </summary>
		/// <typeparam name="T">The type of the items in the sequence.</typeparam>
		/// <param name="sequence">The 'sequence' to get the item from.</param>
		/// <param name="predicate">The predicate to filter the sequence with before taking 1 item.</param>
		/// <param name="error">The error message to throw if not exactly one item</param>
		/// <returns>The single item in the 'sequence'.</returns>
		public static T SingleOrException<T>(this IEnumerable<T> sequence, Func<T, Boolean> predicate, string error)
		{
			T[] items = sequence.Where(predicate).Take(2).ToArray();
			if (items.Length == 0)
				throw new InvalidOperationException($"Found no items: " + error);
			if (items.Length > 1)
				throw new InvalidOperationException($"Found multiple items: " + error);

			return items[0];
		}
	}
}
