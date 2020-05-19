using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace Beaker.CrmSdk.CodeFirst.GenerateEntity
{
	/// <summary>
	/// Extensions for reflection
	/// </summary>
	public static class ReflectionExtensions
	{
		public static bool TryGetAttributeData<TAttribute>(this Assembly assembly, out CustomAttributeData data)
			where TAttribute : System.Attribute
		{
			data = assembly
				.GetCustomAttributesData()
				.FirstOrDefault(cad => cad.AttributeType.FullName == typeof(TAttribute).FullName);

			return !(data is null);
		}

		/// <summary>
		/// Try to get the <see cref="CustomAttributeData"/> for attribute of the <typeparamref name="TAttribute"/> on type <paramref name="type"/>.
		/// </summary>
		/// <typeparam name="TAttribute">The type of the attribute to get the <see cref="CustomAttributeData"/> for.</typeparam>
		/// <param name="type">The type to scan for the attribute.</param>
		/// <returns>If found, the requested attribute data; otherwise null</returns>
		public static bool TryGetAttributeData<TAttribute>(this Type type, out CustomAttributeData data)
			where TAttribute : System.Attribute
		{
			data = type
				.GetCustomAttributesData()
				.FirstOrDefault(cad => cad.AttributeType.FullName == typeof(TAttribute).FullName);

			return !(data is null);
		}

		public static bool TryGetAttributeData<TAttribute>(this PropertyInfo property, out CustomAttributeData data)
			where TAttribute : System.Attribute
		{
			data = property
				.GetCustomAttributesData()
				.FirstOrDefault(cad => cad.AttributeType.FullName == typeof(TAttribute).FullName);

			return !(data is null);
		}

		public static bool TryGetParameterValue<TValue>(this CustomAttributeData attributeData, string parameterName, out TValue value)
		{
			value = default;

			if (attributeData is null)
				return false;
			if (attributeData.Constructor is null)
				return false;

			ParameterInfo[] parameters = attributeData.Constructor.GetParameters();
			for (int ix = 0; ix < parameters.Length; ix++)
				if (parameters[ix].Name == parameterName)
				{
					value = (TValue)attributeData.ConstructorArguments[ix].Value;
					return true;
				}

			return false;
		}

		public static bool TryGetPropertyValue<TValue>(this CustomAttributeData attributeData, string propertyName, out TValue value)
		{
			value = default;

			if (attributeData is null)
				return false;

			foreach (CustomAttributeNamedArgument namedArgument in attributeData.NamedArguments)
			{
				if (namedArgument.MemberName == propertyName)
				{
					value = (TValue)namedArgument.TypedValue.Value;
					return true;
				}
			}

			return false;
		}

		public static bool TryGetDescription(this PropertyInfo property, out string description)
		{
			return property.TryGetAttributeData<DescriptionAttribute>(out CustomAttributeData attribute)
				? attribute.TryGetParameterValue("description", out description)
				: attribute.TryGetPropertyValue("Description", out description);
		}

		public static bool TryGetDescription(this Type type, out string description)
		{
			return type.TryGetAttributeData<DescriptionAttribute>(out CustomAttributeData attribute)
				? attribute.TryGetParameterValue("description", out description)
				: attribute.TryGetPropertyValue("Description", out description);
		}
	}
}
