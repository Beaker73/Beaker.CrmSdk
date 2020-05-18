using Beaker.CrmSdk.CodeFirst.Attributes;

using CommandLine;
using Microsoft.Xrm.Sdk;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web.UI.WebControls;
using System.Xml;
using System.Xml.Linq;

namespace Beaker.CrmSdk.CodeFirst.GenerateEntity
{
	public sealed class Program
	{
		static void Main(string[] args)
		{
			Parser.Default.ParseArguments<CommandLineArguments>(args)
				.WithParsed(a => Run(a));
		}

		private static void Run(CommandLineArguments args)
		{
			AppDomain.CurrentDomain.ReflectionOnlyAssemblyResolve += ResolveAssembly;
			try
			{
				foreach (string assemblyPath in args.AssemblyPaths)
				{
					Assembly reflectionAssembly = Assembly.ReflectionOnlyLoadFrom(assemblyPath);
					Reflect(args, reflectionAssembly);
				}
			}
			finally
			{
				AppDomain.CurrentDomain.ReflectionOnlyAssemblyResolve -= ResolveAssembly;
			}
		}

		private static Assembly ResolveAssembly(object sender, ResolveEventArgs args)
		{
			return Assembly.ReflectionOnlyLoad(args.Name);
		}

		private static void Reflect(CommandLineArguments args, Assembly reflectionAssembly)
		{
			var entities =
				from type in reflectionAssembly.GetTypes()
				where type.IsClass && !type.IsAbstract
				let attr = type.GetCustomAttributesData().FirstOrDefault(cad => cad.AttributeType.FullName == typeof(EntityAttribute).FullName)
				where !(attr is null)
				select new { type, attr };

			foreach (var entity in entities)
				GenerateEntity(args, entity.type, entity.attr);
		}

		private static void GenerateEntity(CommandLineArguments args, Type type, CustomAttributeData entityAttribute)
		{
			string schemaName = type.Name;
			string displayName = entityAttribute.NamedArguments
				.Select(na => (CustomAttributeNamedArgument?)na)
				.FirstOrDefault(na => na.Value.MemberName == "DisplayName")?.TypedValue.Value as string 
				?? schemaName;

			XDocument xml = new XDocument(
				new XElement("Entity",
					new XElement("Name",
						new XAttribute("LocalizedName", displayName),
						new XAttribute("OriginalName", displayName),
						new XText(schemaName)
					),
					new XElement("entity",
						new XAttribute("Name", schemaName)
					)
				));

			// loop over all the properties to generate fields
			var properties =
				from property in type.GetProperties()
				where property.CanRead
				let attr = type.GetCustomAttributesData().FirstOrDefault(cad => cad.AttributeType.FullName == typeof(AttributeLogicalNameAttribute).FullName)
				where !(attr is null)
				select new { property, attr };

			foreach (var property in properties)
				GeneratedField(args, type, property.property, property.attr);

			// create sub folder, if not there yet
			string folderPath = Path.Combine(args.DestinationPath, type.Name);
			if (!Directory.Exists(folderPath))
				Directory.CreateDirectory(folderPath);

			// generate the final entity xml
			xml.Save(Path.Combine(args.DestinationPath, type.Name, "Entity.xml"));
		}

		private static void GeneratedField(CommandLineArguments args, Type type, PropertyInfo property, CustomAttributeData propertyAttribute)
		{

		}
	}
}
