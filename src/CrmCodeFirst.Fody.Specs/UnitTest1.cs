using FluentAssertions;

using Fody;

using Mono.Cecil;

using System.Linq;

using Xunit;

namespace CrmCodeFirst.Fody.Specs
{
	public class UnitTest1
	{
		[Fact]
		public void Test1()
		{
			var subject = new ModuleWeaver();
			subject.ExecuteTestRun("CrmCodeFirst.Source.dll", runPeVerify: true, afterExecuteCallback: CompareModules);
		}

		private static void CompareModules(ModuleDefinition result)
		{
			ModuleDefinition target = ModuleDefinition.ReadModule("CrmCodeFirst.Target.dll");
			result.Assembly.Write("CrmCodeFirst.Result.dll");

			foreach (TypeDefinition targetType in target.Types)
			{
				TypeDefinition resultType = result.Types.Single(rt => rt.FullName == targetType.FullName);
				CompareTypes(resultType, targetType);
			}

		}

		private static void CompareTypes(TypeDefinition resultType, TypeDefinition targetType)
		{
			foreach (MethodDefinition targetMethod in targetType.Methods)
			{
				MethodDefinition resultMethod = resultType.Methods.Single(rm => rm.FullName == targetMethod.FullName);
				CompareMethods(resultMethod, targetMethod);
			}
		}

		private static void CompareMethods(MethodDefinition resultMethod, MethodDefinition targetMethod)
		{
			resultMethod.Body.Instructions.Should().BeEquivalentTo(targetMethod.Body.Instructions,
				because: $"method body of {targetMethod.FullName} should match the expected target");
		}

	}
}
