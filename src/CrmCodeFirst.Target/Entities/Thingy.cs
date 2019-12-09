using Microsoft.Xrm.Sdk;

using System;
using System.ComponentModel.DataAnnotations;

namespace CrmCodeFirst.Source.Entities
{
	/// <summary>
	/// A Thingy
	/// </summary>
	public sealed class Thingy
		: Entity
	{
		/// <summary>
		/// Name of the thingy
		/// </summary>
		[StringLength(100)]
		[AttributeLogicalName("my_OptionalWithStringMaxLength")]
		public string OptionalWithStringMaxLength
		{
			get
			{
				// first validate if key exists, before returning it
				if (Attributes.ContainsKey("my_OptionalWithStringMaxLength"))
					return (string)Attributes["my_OptionalWithStringMaxLength"];

				// when key not found, return the default of null
				return null;
			}
			set
			{
				// A string length validation should be added
				if ((value?.Length ?? 0) > 100)
					throw new ArgumentOutOfRangeException(nameof(value), "Maximum string length is 100");

				// set value
				Attributes["my_OptionalWithStringMaxLength"] = value;
			}
		}

	}
}
