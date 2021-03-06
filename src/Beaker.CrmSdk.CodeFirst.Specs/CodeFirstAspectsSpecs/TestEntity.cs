﻿using Microsoft.Xrm.Sdk;

using System.ComponentModel.DataAnnotations;

namespace Beaker.CrmSdk.CodeFirst.Specs.CodeFirstAspecsSpecs
{
	/// <summary>
	/// Test entity withy properties to get property info from
	/// </summary>
	internal sealed class TestEntity
		: Entity
	{
		/// <summary>
		/// Property info for an optional string test
		/// </summary>
		public string OptionalString { get; set; }

		/// <summary>
		/// Property info for a required string test
		/// </summary>
		[Required]
		public string RequiredString { get; set; }

		/// <summary>
		/// Property info for a string with a maximum string length applied
		/// </summary>
		[StringLength(40)]
		public string StringWithLength { get; set; }

		/// <summary>
		/// Property info for a nullable integer test
		/// </summary>
		public int? NullableInteger { get; set; }

		/// <summary>
		/// Property info for an integer test
		/// </summary>
		public int Integer { get; set; }

		/// <summary>
		/// Property info for a nullable integer that is marked as required test (why?)
		/// </summary>
		[Required]
		public int RequiredNullableInteger { get; set; }

		/// <summary>
		/// Property info for an integer with a range applied
		/// </summary>
		[Range(50, 100)]
		public int RangedInteger { get; set; }

	}
}
