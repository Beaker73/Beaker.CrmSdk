using Beaker.CrmSdk.CodeFirst.Specs.CodeFirstAspecsSpecs;

using FluentAssertions;

using Xunit;

namespace Beaker.CrmSdk.CodeFirst.Specs.CodeFirstAspectsSpecs
{
	/// <summary>
	/// Specs to validate the set aspect for nullable value type attributes
	/// </summary>
	public sealed class SetNullableValueTypeAttributeSpecs
	{
		/// <summary>
		/// Validate that setting a value will be persisted
		/// </summary>
		[Fact]
		public void SetNullableValueTypeIsPersistedInAttributeCollection()
		{
			TestEntity entity = new TestEntity();

			CodeFirstAspect<TestEntity>.SetNullableValueTypeAttribute<int>(entity, typeof(TestEntity).GetProperty("NullableInteger"), "nullableinteger", 45);

			entity.Attributes.ToDictionary()
				.Should().ContainKey("nullableinteger")
				.WhichValue.Should().Be(45,
					because: "the aspect should store the value in the attribute collection");
		}

		/// <summary>
		/// Validate that setting an existing value will overwritten
		/// </summary>
		[Fact]
		public void SetNullableValueTypeIsOverwrittenInAttributeCollection()
		{
			TestEntity entity = new TestEntity();
			entity["nullableinteger"] = 90;

			CodeFirstAspect<TestEntity>.SetNullableValueTypeAttribute<int>(entity, typeof(TestEntity).GetProperty("NullableInteger"), "nullableinteger", 45);

			entity.Attributes.ToDictionary()
				.Should().ContainKey("nullableinteger")
				.WhichValue.Should().Be(45,
					because: "the aspect should overwrite the value in the attribute collection");
		}

		/// <summary>
		/// Validate that setting a null value will be persisted
		/// </summary>
		[Fact]
		public void SetNullableValueTypeWithNullValueShouldBePersistedInAttributeCollection()
		{
			TestEntity entity = new TestEntity();

			CodeFirstAspect<TestEntity>.SetNullableValueTypeAttribute<int>(entity, typeof(TestEntity).GetProperty("NullableInteger"), "nullableinteger", null);

			entity.Attributes.ToDictionary()
				.Should().ContainKey("nullableinteger")
				.WhichValue.Should().BeNull(
					because: "the aspect should store the value in the attribute collection");
		}
	}
}
