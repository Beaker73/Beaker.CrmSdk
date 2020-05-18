using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Beaker.CrmSdk.CodeFirst.Attributes
{
	/// <summary>
	/// Prefix used for the entities and resources
	/// </summary>
	[AttributeUsage(AttributeTargets.Assembly, AllowMultiple = false)]
	public sealed class PublisherAttribute
		: Attribute
	{
		private static readonly Regex _customizationPrefixRegex =
			new Regex("^[a-z][a-z0-9]{1,7}$", RegexOptions.Compiled | RegexOptions.Singleline | RegexOptions.CultureInvariant);

		private int _customizationOptionValuePrefix = 0;

		/// <summary>
		/// Initializes a new publisher for the assembly
		/// </summary>
		/// <param name="customizationPrefix">The customization prefix</param>
		public PublisherAttribute(string customizationPrefix)
		{
			if (string.IsNullOrEmpty(customizationPrefix))
				throw new ArgumentNullException(nameof(customizationPrefix));
			if (customizationPrefix.Length < 2 || customizationPrefix.Length > 8)
				throw new ArgumentOutOfRangeException(nameof(customizationPrefix), "Customization Prefix should be between 2 and 8 characters");

			customizationPrefix = customizationPrefix.ToLowerInvariant();
			if (customizationPrefix == "mscrm" || !_customizationPrefixRegex.IsMatch(customizationPrefix))
				throw new FormatException($"Customization Prefix should start with a letter, then between 1 and 7 letters and/or digits and should not be mscrm.");

			CustomizationPrefix = customizationPrefix;
		}

		/// <summary>
		/// The customization prefix
		/// </summary>
		public string CustomizationPrefix { get; }

		/// <summary>
		/// The 5-digit prefix for option sets
		/// </summary>
		/// <remarks>
		/// If not set, generated based on the <see cref="CustomizationPrefix"/>
		/// </remarks>
		public int CustomizationOptionValuePrefix
		{
			get
			{
				if (_customizationOptionValuePrefix == 0)
					return (Math.Abs(CustomizationPrefix.GetHashCode()) % 90000) + 10000;

				return _customizationOptionValuePrefix;
			}
			set
			{
				if (value < 10000 || value > 99999)
					throw new ArgumentOutOfRangeException(nameof(value));

				_customizationOptionValuePrefix = value;
			}
		}
	}
}
