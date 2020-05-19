using Beaker.CrmSdk.CodeFirst.Attributes;

using CommandLine;

using Microsoft.Xrm.Sdk;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
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
			if (!reflectionAssembly.TryGetAttributeData<PublisherAttribute>(out CustomAttributeData data)
				|| !data.TryGetParameterValue("customizationPrefix", out string customizationPrefix))
				customizationPrefix = "new";

			IEnumerable<Type> entityTypes =
				from type in reflectionAssembly.GetTypes()
				where type.IsClass && !type.IsAbstract
				let attr = type.GetCustomAttributesData().FirstOrDefault(cad => cad.AttributeType.FullName == typeof(EntityAttribute).FullName)
				where !(attr is null)
				select type;

			foreach (Type type in entityTypes)
				generateEntity(type);

			void generateEntity(Type type)
			{
				Entity entity = new Entity(customizationPrefix + "_" + type.Name);

				if (type.TryGetDescription(out string entityDescription))
					entity.Description = entityDescription;

				// loop over all the properties to generate fields
				IEnumerable<PropertyInfo> properties =
					from property in type.GetProperties()
					where property.CanRead && property.TryGetAttributeData<AttributeLogicalNameAttribute>(out CustomAttributeData _)
					select property;

				foreach (PropertyInfo property in properties)
					generateField(property);

				// create sub folder, if not there yet
				string folderPath = Path.Combine(args.DestinationPath, type.Name);
				if (!Directory.Exists(folderPath))
					Directory.CreateDirectory(folderPath);

				// generate the final entity xml
				XDocument xml = new XDocument(entity.ToXml());
				xml.Save(Path.Combine(args.DestinationPath, type.Name, "Entity.xml"));

				void generateField(PropertyInfo property)
				{
					Attribute attribute = new Attribute(customizationPrefix + "_" + property.Name);

					if (property.TryGetDescription(out string attributeDescription))
						attribute.Description = attributeDescription;

					entity.Attributes.Add(attribute);
				}
			}
		}
	}
}