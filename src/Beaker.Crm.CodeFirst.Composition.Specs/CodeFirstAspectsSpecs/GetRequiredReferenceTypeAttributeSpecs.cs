using FluentAssertions;
using System;
using System.Collections.Generic;
using Xunit;

namespace Beaker.Crm.CodeFirst.Composition.Specs.CodeFirstAspecsSpecs
{
	/// <summary>
	/// Specs to validate the get aspect for reference type attributes
	/// </summary>
	public class GetRequiredReferenceTypeAttributeSpecs
	{
		/// <summary>
		/// Validate that getting a existing value returns that value
		/// </summary>
		[Fact]
		public void GetReferenceWithExistingValueReturnsThatValue()
		{
			TestEntity entity = new TestEntity();
			entity["requiredstring"] = "test";

			string result = CodeFirstAspect<TestEntity>.GetReferenceTypeAttribute<string>(entity, typeof(TestEntity).GetProperty("RequiredString"), "requiredstring");

			result.Should().Be("test",
				because: "the aspect should return the value from the attribute collection");
		}

		/// <summary>
		/// Validate that getting a non existing value throw an exception
		/// </summary>
		[Fact]
		public void GetGeferenceWithNonExistingThrowsException()
		{
			TestEntity entity = new TestEntity();

			Action act = () => CodeFirstAspect<TestEntity>.GetReferenceTypeAttribute<string>(entity, typeof(TestEntity).GetProperty("RequiredString"), "requiredstring");

			act.Should().Throw<KeyNotFoundException>(
				because: "the aspect should throw if the attribute collection does not contain a value")
				.WithMessage("No attribute named 'requiredstring' found for attribute marked as required.");
		}

		/// <summary>
		/// Validate that getting wrong typed value throws an exception
		/// </summary>
		[Fact]
		public void GetGeferenceWithWrongTypeThrowsException()
		{
			TestEntity entity = new TestEntity();
			entity["requiredstring"] = 45;

			Action act = () => CodeFirstAspect<TestEntity>.GetReferenceTypeAttribute<string>(entity, typeof(TestEntity).GetProperty("RequiredString"), "requiredstring");

			act.Should().Throw<InvalidCastException>(
				because: "the aspect should throw when there is a wrongly typed value in the attribute collection")
				.WithMessage("Found wrongly typed value in attribute 'requiredstring'. Found 'System.Int32' while expecting 'System.String'.");
		}

		/// <summary>
		/// Validate that a null value for a required throws null reference
		/// </summary>
		[Fact]
		public void GetGeferenceWithNullValueThrowsException()
		{
			TestEntity entity = new TestEntity();
			entity["requiredstring"] = null;

			Action act = () => CodeFirstAspect<TestEntity>.GetReferenceTypeAttribute<string>(entity, typeof(TestEntity).GetProperty("RequiredString"), "requiredstring");

			act.Should().Throw<NullReferenceException>(
				because: "the aspect should throw when there is a null in the attribute collection")
				.WithMessage("Null value found for attribute 'requiredstring' which is marked as required.");
		}

	}
}
