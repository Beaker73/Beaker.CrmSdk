using Beaker.CrmSdk.CodeFirst;

using FluentAssertions;

using System;

using Xunit;

namespace Beaker.CrmSdk.CodeFirst.Specs.CodeFirstAspecsSpecs
{
	/// <summary>
	/// Specs to validate the get aspect for reference type attributes
	/// </summary>
	public class GetNullableValueTypeAttributeSpecs
	{
		/// <summary>
		/// Validate that getting a existing value returns that value
		/// </summary>
		[Fact]
		public void GetReferenceWithExistingValueReturnsThatValue()
		{
			TestEntity entity = new TestEntity();
			entity["nullableinteger"] = 45;

			int? result = CodeFirstAspect<TestEntity>.GetNullableValueTypeAttribute<int>(entity, typeof(TestEntity).GetProperty("NullableInteger"), "nullableinteger");

			result.Should().Be(45,
				because: "the aspect should return the value from the attribute collection");
		}

		/// <summary>
		/// Validate that getting a non existing value returns default for nullable value types
		/// </summary>
		[Fact]
		public void GetGeferenceWithNonExistingValueReturnsNull()
		{
			TestEntity entity = new TestEntity();

			int? result = CodeFirstAspect<TestEntity>.GetNullableValueTypeAttribute<int>(entity, typeof(TestEntity).GetProperty("NullableInteger"), "nullableinteger");

			result.Should().BeNull(
				because: "the aspect should return null when there is not value in the attribute collection");
		}

		/// <summary>
		/// Validate that getting wrong typed value returns default for nullable value types
		/// </summary>
		[Fact]
		public void GetGeferenceWithWrongTypeThrows()
		{
			TestEntity entity = new TestEntity();
			entity["nullableinteger"] = "wrong";

			Action act = () => CodeFirstAspect<TestEntity>.GetNullableValueTypeAttribute<int>(entity, typeof(TestEntity).GetProperty("NullableInteger"), "nullableinteger");

			act.Should().Throw<InvalidCastException>(
				because: "the aspect should throw when there is a wrongly typed value in the attribute collection")
				.WithMessage("Found wrongly typed value in attribute 'nullableinteger'. Found 'System.String' while expecting 'System.Int32'.");
		}

		/// <summary>
		/// Validate that getting wrong typed value returns default for nullable value types
		/// </summary>
		[Fact]
		public void GetGeferenceWithNullValueReturnsNull()
		{
			TestEntity entity = new TestEntity();
			entity["nullableinteger"] = null;

			int? result = CodeFirstAspect<TestEntity>.GetNullableValueTypeAttribute<int>(entity, typeof(TestEntity).GetProperty("NullableInteger"), "nullableinteger");

			result.Should().BeNull(
				because: "the aspect should return null when there is a null in the attribute collection");
		}

	}
}
