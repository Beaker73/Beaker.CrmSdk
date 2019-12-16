using Beaker.Crm.CodeFirst.SmokeTest.Entities;
using FluentAssertions;
using System;
using System.Reflection;
using Xunit;

namespace Beaker.Crm.CodeFirst.SmokeTest.Tests
{
	/// <summary>
	/// Test that thingy has been weaved correctly
	/// </summary>
	public class ThingyWeavingTests
	{
		/// <summary>
		/// Validate that a property can be set and read
		/// </summary>
		[Fact]
		public void SimplePropertyTest()
		{
			Thingy subject = new Thingy();
			subject.Name = "Banana";
			subject.Name.Should().Be("Banana");
		}

		/// <summary>
		/// Validate that settings the backing will result in a value from the property getter
		/// </summary>
		[Fact]
		public void BackingPropertyTest()
		{
			Thingy subject = new Thingy();
			subject.Attributes["my_name"] = "Apple";
			subject.Name.Should().Be("Apple");
		}

		/// <summary>
		/// Validate that the attribute are applied and validated during setting.
		/// </summary>
		[Fact]
		public void SetValidationsShouldBeApplied()
		{
			Thingy subject = new Thingy();
			Action act = () => subject.Name = "This is a text that should be to long to fit an thus should throw an exception";

			act.Should().Throw<ArgumentOutOfRangeException>(
				because: "a validation for data annotation attributes should have been weaved into the setter of the property");
		}

		/// <summary>
		/// Validate that the required attribute is validated during setting.
		/// </summary>
		[Fact]
		public void RequiredValidationShouldBeApplied()
		{
			Thingy subject = new Thingy();
			Action act = () => subject.Name = null;

			act.Should().Throw<ArgumentNullException>(
				because: "a validation for null should have been weaved into the property");
		}

		/// <summary>
		/// Validate that the SDK attributes are correctly applied
		/// </summary>
		[Fact]
		public void AttributesAppliedForCompatibilityWithSdk()
		{
			typeof(Thingy).GetProperty("Name")
				.GetCustomAttribute<Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute>()
				.LogicalName.Should().Be("my_name",
					because: "a logical name should be set on the propperty");
		}
	}
}
