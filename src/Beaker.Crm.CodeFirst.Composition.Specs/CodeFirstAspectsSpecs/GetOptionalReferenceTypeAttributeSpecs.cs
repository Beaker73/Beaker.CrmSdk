using FluentAssertions;

using System;

using Xunit;

namespace Beaker.Crm.CodeFirst.Composition.Specs.CodeFirstAspecsSpecs
{
	/// <summary>
	/// Specs to validate the get aspect for reference type attributes
	/// </summary>
	public class GetReferenceTypeAttributeSpecs
	{
		/// <summary>
		/// Validate that getting a existing value returns that value
		/// </summary>
		[Fact]
		public void GetReferenceWithExistingValueReturnsThatValue()
		{
			TestEntity entity = new TestEntity();
			entity["optionalstring"] = "test";

			string result = CodeFirstAspect<TestEntity>.GetReferenceTypeAttribute<string>(entity, typeof(TestEntity).GetProperty("OptionalString"), "optionalstring");

			result.Should().Be("test", because: "the aspect should return the value from the attribute collection");
		}

		/// <summary>
		/// Validate that getting a non existing value returns default for non required reference types
		/// </summary>
		[Fact]
		public void GetGeferenceWithNonExistingValueReturnsNull()
		{
			TestEntity entity = new TestEntity();

			string result = CodeFirstAspect<TestEntity>.GetReferenceTypeAttribute<string>(entity, typeof(TestEntity).GetProperty("OptionalString"), "optionalstring");

			result.Should().BeNull(because: "the aspect should return null when there is not value in the attribute collection");
		}

		/// <summary>
		/// Validate that getting wrong typed value returns default for non required reference types
		/// </summary>
		[Fact]
		public void GetGeferenceWithWrongTypeThrows()
		{
			TestEntity entity = new TestEntity();
			entity["optionalstring"] = 45;

			Action act = () => CodeFirstAspect<TestEntity>.GetReferenceTypeAttribute<string>(entity, typeof(TestEntity).GetProperty("OptionalString"), "optionalstring");

			act.Should().Throw<InvalidCastException>(
				because: "the aspect should throw when there is a wrongly typed value in the attribute collection")
				.WithMessage("Found wrongly typed value in attribute 'optionalstring'. Found 'System.Int32' while expecting 'System.String'.");
		}

		/// <summary>
		/// Validate that getting wrong typed value returns default for non required reference types
		/// </summary>
		[Fact]
		public void GetGeferenceWithNullValueReturnsNull()
		{
			TestEntity entity = new TestEntity();
			entity["optionalstring"] = null;

			string result = CodeFirstAspect<TestEntity>.GetReferenceTypeAttribute<string>(entity, typeof(TestEntity).GetProperty("OptionalString"), "optionalstring");

			result.Should().BeNull(because: "the aspect should return null when there is a null in the attribute collection");
		}

	}
}
