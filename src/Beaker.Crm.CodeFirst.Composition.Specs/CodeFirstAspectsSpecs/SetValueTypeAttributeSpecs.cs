using Beaker.Crm.CodeFirst.Composition.Specs.CodeFirstAspecsSpecs;

using FluentAssertions;

using Xunit;

namespace Beaker.Crm.CodeFirst.Composition.Specs.CodeFirstAspectsSpecs
{
	/// <summary>
	/// Specs to validate the set aspect for nullable value type attributes
	/// </summary>
	public sealed class SetValueTypeAttributeSpecs
	{
		/// <summary>
		/// Validate that setting a value will be persisted
		/// </summary>
		[Fact]
		public void SetValueTypeIsPersistedInAttributeCollection()
		{
			TestEntity entity = new TestEntity();

			CodeFirstAspect<TestEntity>.SetValueTypeAttribute<int>(entity, typeof(TestEntity).GetProperty("Integer"), "integer", 45);

			entity.Attributes.ToDictionary()
				.Should().ContainKey("integer")
				.WhichValue.Should().Be(45,
					because: "the aspect should store the value in the attribute collection");
		}

		/// <summary>
		/// Validate that setting an existing value will overwritten
		/// </summary>
		[Fact]
		public void SetValueTypeIsOverwrittenInAttributeCollection()
		{
			TestEntity entity = new TestEntity();
			entity["integer"] = 90;

			CodeFirstAspect<TestEntity>.SetValueTypeAttribute<int>(entity, typeof(TestEntity).GetProperty("Integer"), "integer", 45);

			entity.Attributes.ToDictionary()
				.Should().ContainKey("integer")
				.WhichValue.Should().Be(45,
					because: "the aspect should overwrite the value in the attribute collection");
		}
	}
}
