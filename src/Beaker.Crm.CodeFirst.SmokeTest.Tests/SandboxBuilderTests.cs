using Beaker.Crm.CodeFirst.SmokeTest.Tests.Builders;

using Xunit;

namespace Beaker.Crm.CodeFirst.SmokeTest.Tests
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
			ISandbox sandbox = new SandboxBuilder().Build();
			sandbox.Execute();
		}
	}
}
