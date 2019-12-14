using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace Beaker.Crm.CodeFirst.Composition
{
	/// <summary>
	/// Contains the CodeFirst 'aspects' that will be injected into the getter/setters of the code first classes
	/// </summary>
	/// <typeparam name="TEntity">The type of the entity to get the attribute from</typeparam>
	public static class CodeFirstAspect<TEntity>
		where TEntity : Entity
	{
		/// <summary>
		/// Gets a reference value named <paramref name="logicalAttributeName"/> from the provided <paramref name="entity"/>.
		/// </summary>
		/// <typeparam name="TProperty">The type of the result</typeparam>
		/// <param name="entity">The entity to get the value from</param>
		/// <param name="property">The mapped property with the validation attributes</param>
		/// <param name="logicalAttributeName">The logical attribute name</param>
		/// <returns>The value of the attribute</returns>
		public static TProperty GetReferenceTypeAttribute<TProperty>(TEntity entity, PropertyInfo property, string logicalAttributeName)
			where TProperty : class
		{
			bool isRequired = !(property.GetCustomAttribute<RequiredAttribute>() is null);

			// try to get the value
			if (entity.Attributes.TryGetValue(logicalAttributeName, out object untypedValue))
			{
				// ensure correct type
				TProperty type = untypedValue as TProperty;

				// if we have a value, return it
				if (!(type is null))
					return type;
			}

			// no value, but required, throw
			if (isRequired)
				throw new KeyNotFoundException($"No attribute named ${logicalAttributeName} found");

			// no value, return default
			return default;
		}

		/// <summary>
		/// Set a reference value named <paramref name="logicalAttributeName"/> on the provided <paramref name="entity"/>.
		/// </summary>
		/// <typeparam name="TProperty">The type of the property</typeparam>
		/// <param name="entity">The entity to get the value from</param>
		/// <param name="property">The mapped property with the validation attributes</param>
		/// <param name="logicalAttributeName">The logical attribute name</param>
		/// <param name="value">The value to set the attribute to</param>
		public static void SetReferenceTypeAttribute<TProperty>(TEntity entity, PropertyInfo property, string logicalAttributeName, TProperty value)
			where TProperty : class
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Gets a reference value named <paramref name="logicalAttributeName"/> from the provided <paramref name="entity"/>.
		/// </summary>
		/// <typeparam name="TProperty">The type of the result</typeparam>
		/// <param name="entity">The entity to get the value from</param>
		/// <param name="property">The mapped property with the validation attributes</param>
		/// <param name="logicalAttributeName">The logical attribute name</param>
		/// <returns>The value of the attribute</returns>
		public static TProperty GetValueTypeAttribute<TProperty>(TEntity entity, PropertyInfo property, string logicalAttributeName)
			where TProperty : struct
		{
			// try to get the value
			if (entity.Attributes.TryGetValue(logicalAttributeName, out object untypedValue))
			{
				// ensure correct type
				if (untypedValue is TProperty)
					return (TProperty)untypedValue;
			}

			// no value, throw
			throw new KeyNotFoundException($"No attribute named ${logicalAttributeName} found");
		}

		/// <summary>
		/// Set a reference value named <paramref name="logicalAttributeName"/> on the provided <paramref name="entity"/>.
		/// </summary>
		/// <typeparam name="TProperty">The type of the property</typeparam>
		/// <param name="entity">The entity to get the value from</param>
		/// <param name="property">The mapped property with the validation attributes</param>
		/// <param name="logicalAttributeName">The logical attribute name</param>
		/// <param name="value">The value to set the attribute to</param>
		public static void SetValueTypeAttribute<TProperty>(TEntity entity, PropertyInfo property, string logicalAttributeName, TProperty value)
			where TProperty : struct
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Gets a reference value named <paramref name="logicalAttributeName"/> from the provided <paramref name="entity"/>.
		/// </summary>
		/// <typeparam name="TProperty">The type of the result</typeparam>
		/// <param name="entity">The entity to get the value from</param>
		/// <param name="property">The mapped property with the validation attributes</param>
		/// <param name="logicalAttributeName">The logical attribute name</param>
		/// <returns>The value of the attribute</returns>
		public static TProperty? GetNullableValueTypeAttribute<TProperty>(TEntity entity, PropertyInfo property, string logicalAttributeName)
			where TProperty : struct
		{
			// try to get the value
			if (entity.Attributes.TryGetValue(logicalAttributeName, out object untypedValue))
			{
				// ensure correct type
				if (untypedValue is TProperty)
					return (TProperty)untypedValue;
			}

			return default;
		}

		/// <summary>
		/// Set a nullable value type named <paramref name="logicalAttributeName"/> on the provided <paramref name="entity"/>.
		/// </summary>
		/// <typeparam name="TProperty">The type of the property</typeparam>
		/// <param name="entity">The entity to get the value from</param>
		/// <param name="property">The mapped property with the validation attributes</param>
		/// <param name="logicalAttributeName">The logical attribute name</param>
		/// <param name="value">The value to set the attribute to</param>
		public static void SetNullableValueTypeAttribute<TProperty>(TEntity entity, PropertyInfo property, string logicalAttributeName, TProperty? value)
			where TProperty : struct
		{
			throw new NotImplementedException();
		}
	}
}
