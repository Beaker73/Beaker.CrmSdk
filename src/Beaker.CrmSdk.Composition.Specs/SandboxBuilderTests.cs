using Beaker.CrmSdk.CodeFirst.SmokeTest.Tests.Builders;

using Xunit;

namespace Beaker.CrmSdk.CodeFirst.SmokeTest.Tests
{
	/// <summary>
	/// Tests the sandbox builder
	/// </summary>
	public sealed class SandboxBuilderTests
	{
		/// <summary>
		/// Test sandbox
		/// </summary>
		[Fact]
		public void SandboxTest()
		{
			ISandbox sandbox = new SandboxBuilder()
				.ForPlugin<TestPlugin>()
				.Build();
			sandbox.Execute();
		}
	}
}
