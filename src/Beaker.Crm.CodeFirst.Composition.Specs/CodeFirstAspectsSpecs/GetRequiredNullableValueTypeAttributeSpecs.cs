using FluentAssertions;

using System;
using System.Collections.Generic;

using Xunit;

namespace Beaker.Crm.CodeFirst.Composition.Specs.CodeFirstAspecsSpecs
{
	/// <summary>
	/// Specs to validate the get aspect for reference type attributes
	/// </summary>
	public class GetRequiredNullableValueTypeAttributeSpecs
	{
		/// <summary>
		/// Validate that getting a existing value returns that value
		/// </summary>
		[Fact]
		public void GetValueTypeWithExistingValueReturnsThatValue()
		{
			TestEntity entity = new TestEntity();
			entity["requirednullableinteger"] = 45;

			int? result = CodeFirstAspect<TestEntity>.GetNullableValueTypeAttribute<int>(entity, typeof(TestEntity).GetProperty("RequiredNullableInteger"), "requirednullableinteger");

			result.Should().Be(45,
				because: "the aspect should return the value from the attribute collection");
		}

		/// <summary>
		/// Validate that getting a non existing value returns throws for nullable value types marked as required
		/// </summary>
		[Fact]
		public void GetValueTypeWithNonExistingValueThrowException()
		{
			TestEntity entity = new TestEntity();

			Action act = () => CodeFirstAspect<TestEntity>.GetNullableValueTypeAttribute<int>(entity, typeof(TestEntity).GetProperty("RequiredNullableInteger"), "requirednullableinteger");

			act.Should().Throw<KeyNotFoundException>(
				because: "the aspect should throw for value types when there is not value in the attribute collection")
				.WithMessage("No attribute named 'requirednullableinteger' found for attribute marked as required.");
		}

		/// <summary>
		/// Validate that getting wrong typed value throws an invalid cast
		/// </summary>
		[Fact]
		public void GetValueTypeWithWrongTypeThrows()
		{
			TestEntity entity = new TestEntity();
			entity["requirednullableinteger"] = "wrong";

			Action act = () => CodeFirstAspect<TestEntity>.GetNullableValueTypeAttribute<int>(entity, typeof(TestEntity).GetProperty("RequiredNullableInteger"), "requirednullableinteger");

			act.Should().Throw<InvalidCastException>(
				because: "the aspect should throw when there is a wrongly typed value in the attribute collection")
				.WithMessage("Found wrongly typed value in attribute 'requirednullableinteger'. Found 'System.String' while expecting 'System.Int32'.");
		}

		/// <summary>
		/// Validate that getting null value throws for nullable value types marked as required
		/// </summary>
		[Fact]
		public void GetValueTypeWithNullValueThrows()
		{
			TestEntity entity = new TestEntity();
			entity["requirednullableinteger"] = null;

			Action act = () => CodeFirstAspect<TestEntity>.GetNullableValueTypeAttribute<int>(entity, typeof(TestEntity).GetProperty("RequiredNullableInteger"), "requirednullableinteger");

			act.Should().Throw<NullReferenceException>(
				because: "the aspect should throw for value types when there is not value in the attribute collection")
				.WithMessage("Null value found for attribute 'requirednullableinteger' which is marked as required.");
		}

	}
}
