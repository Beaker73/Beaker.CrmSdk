using System.Xml.Linq;

namespace Beaker.CrmSdk.CodeFirst.GenerateEntity
{
	public interface IGenerator
	{
		/// <summary>
		/// Generates XML
		/// </summary>
		/// <returns></returns>
		XNode ToXml();
	}
}