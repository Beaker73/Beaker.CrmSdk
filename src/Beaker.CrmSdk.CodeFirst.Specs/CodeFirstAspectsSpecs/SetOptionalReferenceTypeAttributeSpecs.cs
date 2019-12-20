using Beaker.CrmSdk.CodeFirst.Specs.CodeFirstAspecsSpecs;

using FluentAssertions;

using Xunit;

namespace Beaker.CrmSdk.CodeFirst.Specs.CodeFirstAspectsSpecs
{
	/// <summary>
	/// Specs to validate the set aspect for reference type attributes
	/// </summary>
	public sealed class SetOptionalReferenceTypeAttributeSpecs
	{
		/// <summary>
		/// Validate that setting a value will be persisted
		/// </summary>
		[Fact]
		public void SetOptionalReferenceTypeIsPersistedInAttributeCollection()
		{
			TestEntity entity = new TestEntity();

			CodeFirstAspect<TestEntity>.SetReferenceTypeAttribute<string>(entity, typeof(TestEntity).GetProperty("OptionalString"), "optionalstring", "Banana");

			entity.Attributes.ToDictionary()
				.Should().ContainKey("optionalstring")
				.WhichValue.Should().Be("Banana",
					because: "the aspect should store the value in the attribute collection");
		}

		/// <summary>
		/// Validate that setting an existing value will overwritten
		/// </summary>
		[Fact]
		public void SetOptionalReferenceTypeIsOverwrittenInAttributeCollection()
		{
			TestEntity entity = new TestEntity();
			entity["optionalstring"] = "original";

			CodeFirstAspect<TestEntity>.SetReferenceTypeAttribute<string>(entity, typeof(TestEntity).GetProperty("OptionalString"), "optionalstring", "new");

			entity.Attributes.ToDictionary()
				.Should().ContainKey("optionalstring")
				.WhichValue.Should().Be("new",
					because: "the aspect should overwrite the value in the attribute collection");
		}

		/// <summary>
		/// Validate that setting a null value will be persisted
		/// </summary>
		[Fact]
		public void SetOptionalReferenceTypeWithNullValueShouldBePersistedInAttributeCollection()
		{
			TestEntity entity = new TestEntity();

			CodeFirstAspect<TestEntity>.SetReferenceTypeAttribute<string>(entity, typeof(TestEntity).GetProperty("OptionalString"), "optionalstring", null);

			entity.Attributes.ToDictionary()
				.Should().ContainKey("optionalstring")
				.WhichValue.Should().BeNull(
					because: "the aspect should store the value in the attribute collection");
		}
	}
}
