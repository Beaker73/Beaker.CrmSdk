using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Beaker.CrmSdk.CodeFirst.GenerateEntity
{
	public sealed class Entity
		: IGenerator
	{
		public Entity(string schemaName)
		{
			SchemaName = schemaName;
		}

		public string SchemaName { get; }
		public string LogicalName => SchemaName.ToLowerInvariant();
		public string DisplayName { get; set; }
		public string Description { get; set; }

		public List<Attribute> Attributes { get; } = new List<Attribute>();

		/// <summary>
		/// Creates XML from the Entity Information
		/// </summary>
		/// <returns>The Root Node for the Entity</returns>
		public XNode ToXml()
		{
			return
				new XElement("Entity",
					new XElement("Name",
						new XText(SchemaName)
					),
					new XElement("EntityInfo",
						new XElement("entity",
							new XAttribute("Name", SchemaName),
							new XElement("attributes", Attributes.Select(a => a.ToXml()))
						)
					)
				);
		}
	}
}
