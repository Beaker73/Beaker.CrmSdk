using Beaker.Crm.CodeFirst.Composition.Attributes;

using Microsoft.Xrm.Sdk;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CrmCodeFirst.Source.Entities
{
	/// <summary>
	/// A Thingy
	/// </summary>
	[Entity]
	public sealed class Thingy
		: Entity
	{
		/// <summary>
		/// optional string value with a max length
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

		/// <summary>
		/// required string value with a max length
		/// </summary>
		[StringLength(50)]
		[Required]
		public string RequiredWithStringMaxLength
		{
			get
			{
				// first validate if key exists, before returning it
				if (Attributes.ContainsKey("my_RequiredWithStringMaxLength"))
					return (string)Attributes["my_RequiredWithStringMaxLength"];

				// when key not found, throw
				throw new KeyNotFoundException("The RequiredWithStringMaxLength attribute is missing");
			}
			set
			{
				// required, so validate input value against null
				if (value is null)
					throw new ArgumentNullException(nameof(value), "The RequiredWithStringMaxStringLength attribute is required");

				// A string length validation should be added
				if ((value?.Length ?? 0) > 50)
					throw new ArgumentOutOfRangeException(nameof(value), "Maximum string length is 50");

				// set value
				Attributes["my_RequiredWithStringMaxLength"] = value;
			}
		}
	}
}
