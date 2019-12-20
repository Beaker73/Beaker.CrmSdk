using Beaker.CrmSdk.CodeFirst.Specs.CodeFirstAspecsSpecs;

using FluentAssertions;

using System;

using Xunit;

namespace Beaker.CrmSdk.CodeFirst.Specs.CodeFirstAspectsSpecs
{
	/// <summary>
	/// Validates string length attributes adherence
	/// </summary>
	public sealed class SetStringWithLengthAttributeSpecs
	{
		/// <summary>
		/// Validates that a string that is not to long is set without an error
		/// </summary>
		[Fact]
		public void SetStringInRange()
		{
			TestEntity entity = new TestEntity();

			string veryShort = "short";
			Action act = () => CodeFirstAspect<TestEntity>.SetReferenceTypeAttribute<string>(entity, typeof(TestEntity).GetProperty("StringWithLength"), "stringwithlength", veryShort);
			act.Should().NotThrow(
				because: "the string is not to long");
		}

		/// <summary>
		/// Validate that a string that is cleary out of range, will throw
		/// </summary>
		[Fact]
		public void SetStringOutOfRange()
		{
			TestEntity entity = new TestEntity();

			string toLong = "This string is over forty characters longs, so it should fail when setting";
			Action act = () => CodeFirstAspect<TestEntity>.SetReferenceTypeAttribute<string>(entity, typeof(TestEntity).GetProperty("StringWithLength"), "stringwithlength", toLong);

			act.Should().Throw<ArgumentOutOfRangeException>(
				because: "the string is cleary to long");
		}

		/// <summary>
		/// Validate that a string that is stil xactly in range will not fail
		/// </summary>
		[Fact]
		public void SetStringExeactlyInRange()
		{
			TestEntity entity = new TestEntity();

			string exactly = "This string is exactly forty characters.";
			Action act = () => CodeFirstAspect<TestEntity>.SetReferenceTypeAttribute<string>(entity, typeof(TestEntity).GetProperty("StringWithLength"), "stringwithlength", exactly);

			act.Should().NotThrow(
				because: "the string exactly matches the max length");
		}

		/// <summary>
		/// Validate that a string that is stil xactly in range will not fail
		/// </summary>
		[Fact]
		public void SetStringSlightyOutOfRange()
		{
			TestEntity entity = new TestEntity();

			string slightlyToLong = "This string is just over forty characters";
			Action act = () => CodeFirstAspect<TestEntity>.SetReferenceTypeAttribute<string>(entity, typeof(TestEntity).GetProperty("StringWithLength"), "stringwithlength", slightlyToLong);

			act.Should().Throw<ArgumentOutOfRangeException>(
				because: "the string 1 character to long");
		}
	}
}
