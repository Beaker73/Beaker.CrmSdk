using Beaker.CrmSdk.CodeFirst;

using FluentAssertions;

using System;
using System.Collections.Generic;

using Xunit;

namespace Beaker.CrmSdk.CodeFirst.Specs.CodeFirstAspecsSpecs
{
	/// <summary>
	/// Specs to validate the get aspect for reference type attributes
	/// </summary>
	public class GetValueTypeAttributeSpecs
	{
		/// <summary>
		/// Validate that getting a existing value returns that value
		/// </summary>
		[Fact]
		public void GetValueTypeWithExistingValueReturnsThatValue()
		{
			TestEntity entity = new TestEntity();
			entity["integer"] = 45;

			int result = CodeFirstAspect<TestEntity>.GetValueTypeAttribute<int>(entity, typeof(TestEntity).GetProperty("Integer"), "integer");

			result.Should().Be(45,
				because: "the aspect should return the value from the attribute collection");
		}

		/// <summary>
		/// Validate that getting a non existing value returns throws for value types
		/// </summary>
		[Fact]
		public void GetValueTypeWithNonExistingValueThrowException()
		{
			TestEntity entity = new TestEntity();

			Action act = () => CodeFirstAspect<TestEntity>.GetValueTypeAttribute<int>(entity, typeof(TestEntity).GetProperty("Integer"), "integer");

			act.Should().Throw<KeyNotFoundException>(
				because: "the aspect should throw for value types when there is not value in the attribute collection")
				.WithMessage("No attribute named 'integer' found for attribute marked as required.");
		}

		/// <summary>
		/// Validate that getting wrong typed value throws an invalid cast
		/// </summary>
		[Fact]
		public void GetValueTypeWithWrongTypeThrows()
		{
			TestEntity entity = new TestEntity();
			entity["integer"] = "wrong";

			Action act = () => CodeFirstAspect<TestEntity>.GetValueTypeAttribute<int>(entity, typeof(TestEntity).GetProperty("Integer"), "integer");

			act.Should().Throw<InvalidCastException>(
				because: "the aspect should throw when there is a wrongly typed value in the attribute collection")
				.WithMessage("Found wrongly typed value in attribute 'integer'. Found 'System.String' while expecting 'System.Int32'.");
		}

		/// <summary>
		/// Validate that getting null value throws for value types
		/// </summary>
		[Fact]
		public void GetValueTypeWithNullValueThrows()
		{
			TestEntity entity = new TestEntity();
			entity["integer"] = null;

			Action act = () => CodeFirstAspect<TestEntity>.GetValueTypeAttribute<int>(entity, typeof(TestEntity).GetProperty("Integer"), "integer");

			act.Should().Throw<NullReferenceException>(
				because: "the aspect should throw for value types when there is not value in the attribute collection")
				.WithMessage("Null value found for attribute 'integer' which is marked as required.");
		}

	}
}
