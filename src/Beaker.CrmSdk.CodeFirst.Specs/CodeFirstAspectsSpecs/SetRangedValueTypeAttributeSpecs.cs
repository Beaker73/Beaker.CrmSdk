using Beaker.CrmSdk.CodeFirst.Specs.CodeFirstAspecsSpecs;
using Beaker.CrmSdk.CodeFirst;
using FluentAssertions;
using System;
using Xunit;

namespace Beaker.CrmSdk.CodeFirst.Specs.CodeFirstAspectsSpecs
{
	/// <summary>
	/// Specs to validate the set aspect for nullable value type attributes
	/// </summary>
	public sealed class SetRangedValueTypeAttributeSpecs
	{
		/// <summary>
		/// Validate that setting a to low value will throw
		/// </summary>
		[Fact]
		public void SetValueToLowWillThrow()
		{
			TestEntity entity = new TestEntity();

			int belowRange = 49;
			Action act = () => CodeFirstAspect<TestEntity>.SetValueTypeAttribute<int>(entity, typeof(TestEntity).GetProperty("RangedInteger"), "rangedinteger", belowRange);

			act.Should().Throw<ArgumentOutOfRangeException>(
				because: "the value is outside the lower range");
		}

		/// <summary>
		/// Validate that setting a low value in range will not throw
		/// </summary>
		[Fact]
		public void SetValueInLowerRangeWillNotThrow()
		{
			TestEntity entity = new TestEntity();

			int lowerValue = 50;
			Action act = () => CodeFirstAspect<TestEntity>.SetValueTypeAttribute<int>(entity, typeof(TestEntity).GetProperty("RangedInteger"), "rangedinteger", lowerValue);

			act.Should().NotThrow<ArgumentOutOfRangeException>(
				because: "the value is exactly inside the lower range");
		}

		/// <summary>
		/// Validate that setting a upper value in range will not throw
		/// </summary>
		[Fact]
		public void SetValueInUpperRangeWillNotThrow()
		{
			TestEntity entity = new TestEntity();

			int highValue = 100;
			Action act = () => CodeFirstAspect<TestEntity>.SetValueTypeAttribute<int>(entity, typeof(TestEntity).GetProperty("RangedInteger"), "rangedinteger", highValue);

			act.Should().NotThrow<ArgumentOutOfRangeException>(
				because: "the value is exactly inside the upper range");
		}

		/// <summary>
		/// Validate that setting a to high value will throw
		/// </summary>
		[Fact]
		public void SetValueToHighWillThrow()
		{
			TestEntity entity = new TestEntity();

			int aboveRange = 101;
			Action act = () => CodeFirstAspect<TestEntity>.SetValueTypeAttribute<int>(entity, typeof(TestEntity).GetProperty("RangedInteger"), "rangedinteger", aboveRange);

			act.Should().Throw<ArgumentOutOfRangeException>(
				because: "the value is outside the upper range");
		}
	}


}
