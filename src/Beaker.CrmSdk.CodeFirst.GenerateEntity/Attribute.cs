using System.Xml.Linq;

namespace Beaker.CrmSdk.CodeFirst.GenerateEntity
{
	public sealed class Attribute
		: IGenerator
	{
		public Attribute(string schemaName)
		{
			SchemaName = schemaName;
		}

		public string SchemaName { get; }
		public string LogicalName => SchemaName.ToLowerInvariant();
		public string DisplayName { get; set; }
		public string Description { get; set; }


		public XNode ToXml()
		{
			return
				new XElement("attribute",
					new XAttribute("PhysicalName", SchemaName)
				);
		}
	}
}
