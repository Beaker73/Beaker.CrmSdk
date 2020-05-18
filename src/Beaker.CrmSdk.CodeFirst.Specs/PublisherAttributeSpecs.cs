using Beaker.CrmSdk.CodeFirst.Attributes;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Beaker.CrmSdk.CodeFirst.Specs
{
	/// <summary>
	/// Specifications for the <see cref="PublisherAttribute"/>
	/// </summary>
	public sealed class PublisherAttributeSpecs
	{
		/// <summary>
		/// Validates that the generated option value prefix matches the one in CRM
		/// </summary>
		[Fact]
		public void CustomizationOptionValuePrefixShouldMatchCrmDefault()
		{
			new PublisherAttribute("wur").CustomizationOptionValuePrefix.Should().Be(97_568);
			new PublisherAttribute("banaan").CustomizationOptionValuePrefix.Should().Be(70_593);
			new PublisherAttribute("aaaaaaaa").CustomizationOptionValuePrefix.Should().Be(12_338);
			new PublisherAttribute("z9z9z9z9").CustomizationOptionValuePrefix.Should().Be(99_230);
		}
	}
}
