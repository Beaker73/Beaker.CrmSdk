using Beaker.Crm.CodeFirst.Composition.Specs.CodeFirstAspecsSpecs;

using FluentAssertions;
using System;
using Xunit;

namespace Beaker.Crm.CodeFirst.Composition.Specs.CodeFirstAspectsSpecs
{
	/// <summary>
	/// Specs to validate the set aspect for nullable value type attributes
	/// </summary>
	public sealed class SetRequiredNullableValueTypeAttributeSpecs
	{
		/// <summary>
		/// Validate that setting a value will be persisted
		/// </summary>
		[Fact]
		public void SetNullableValueTypeIsPersistedInAttributeCollection()
		{
			TestEntity entity = new TestEntity();

			CodeFirstAspect<TestEntity>.SetNullableValueTypeAttribute<int>(entity, typeof(TestEntity).GetProperty("RequiredNullableInteger"), "requirednullableinteger", 45);

			entity.Attributes.ToDictionary()
				.Should().ContainKey("requirednullableinteger")
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
			entity["requirednullableinteger"] = 90;

			CodeFirstAspect<TestEntity>.SetNullableValueTypeAttribute<int>(entity, typeof(TestEntity).GetProperty("RequiredNullableInteger"), "requirednullableinteger", 45);

			entity.Attributes.ToDictionary()
				.Should().ContainKey("requirednullableinteger")
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

			Action act  = () => CodeFirstAspect<TestEntity>.SetNullableValueTypeAttribute<int>(entity, typeof(TestEntity).GetProperty("RequiredNullableInteger"), "requirednullableinteger", null);

			act.Should().Throw<ArgumentNullException>(
					because: "the aspect should store the value in the attribute collection")
				.WithMessage("The attribute 'requirednullableinteger' is required\nParameter name: value");
		}
	}
}
