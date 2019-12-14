using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
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
		[SuppressMessage("Design", "CA1000:Do not declare static members on generic types", Justification = "Only called via code weaving by generated code, so nothing unclear for users")]
		public static TProperty GetReferenceTypeAttribute<TProperty>(TEntity entity, PropertyInfo property, string logicalAttributeName)
			where TProperty : class
		{
			if (entity is null)
				throw new ArgumentNullException(nameof(entity));
			if (property is null)
				throw new ArgumentNullException(nameof(property));
			if (string.IsNullOrEmpty(logicalAttributeName))
				throw new ArgumentNullException(nameof(logicalAttributeName));

			bool isRequired = !(property.GetCustomAttribute<RequiredAttribute>() is null);

			// try to get the value
			if (entity.Attributes.TryGetValue(logicalAttributeName, out object untypedValue))
			{
				// null handling
				if (untypedValue is null)
				{
					if (isRequired)
						throw new NullReferenceException($"Null value found for attribute '{logicalAttributeName}' which is marked as required.");
					else
						return null;
				}
				// wrong type
				if (!(untypedValue is TProperty typedValue))
					throw new InvalidCastException($"Found wrongly typed value in attribute '{logicalAttributeName}'. Found '{untypedValue.GetType()}' while expecting '{typeof(TProperty)}'.");

				return typedValue;
			}

			// no value, but required, throw
			if (isRequired)
				throw new KeyNotFoundException($"No attribute named '{logicalAttributeName}' found for attribute marked as required.");

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
		[SuppressMessage("Design", "CA1000:Do not declare static members on generic types", Justification = "Only called via code weaving by generated code, so nothing unclear for users")]
		public static void SetReferenceTypeAttribute<TProperty>(TEntity entity, PropertyInfo property, string logicalAttributeName, TProperty value)
			where TProperty : class
		{
			if (entity is null)
				throw new ArgumentNullException(nameof(entity));
			if (property is null)
				throw new ArgumentNullException(nameof(property));
			if (string.IsNullOrEmpty(logicalAttributeName))
				throw new ArgumentNullException(nameof(logicalAttributeName));

			if (!ValidateAttributes(property, value, out Exception exception))
				throw exception;

			bool isRequired = !(property.GetCustomAttribute<RequiredAttribute>() is null);

			// if marked as required and value is null, throw
			if (isRequired && value is null)
				throw new ArgumentNullException(nameof(value), $"The attribute ${logicalAttributeName} is required");

			entity.Attributes[logicalAttributeName] = value;
		}

		/// <summary>
		/// Gets a reference value named <paramref name="logicalAttributeName"/> from the provided <paramref name="entity"/>.
		/// </summary>
		/// <typeparam name="TProperty">The type of the result</typeparam>
		/// <param name="entity">The entity to get the value from</param>
		/// <param name="property">The mapped property with the validation attributes</param>
		/// <param name="logicalAttributeName">The logical attribute name</param>
		/// <returns>The value of the attribute</returns>
		[SuppressMessage("Design", "CA1000:Do not declare static members on generic types", Justification = "Only called via code weaving by generated code, so nothing unclear for users")]
		public static TProperty GetValueTypeAttribute<TProperty>(TEntity entity, PropertyInfo property, string logicalAttributeName)
			where TProperty : struct
		{
			if (entity is null)
				throw new ArgumentNullException(nameof(entity));
			if (property is null)
				throw new ArgumentNullException(nameof(property));
			if (string.IsNullOrEmpty(logicalAttributeName))
				throw new ArgumentNullException(nameof(logicalAttributeName));

			// try to get the value
			if (entity.Attributes.TryGetValue(logicalAttributeName, out object untypedValue))
			{
				// null handling
				if (untypedValue is null)
					throw new NullReferenceException($"Null value found for attribute '{logicalAttributeName}' which is marked as required.");
				// wrong type
				if (!(untypedValue is TProperty typedValue))
					throw new InvalidCastException($"Found wrongly typed value in attribute '{logicalAttributeName}'. Found '{untypedValue.GetType()}' while expecting '{typeof(TProperty)}'.");

				return typedValue;
			}

			// no value, but required, throw
			throw new KeyNotFoundException($"No attribute named '{logicalAttributeName}' found for attribute marked as required.");
		}

		/// <summary>
		/// Set a reference value named <paramref name="logicalAttributeName"/> on the provided <paramref name="entity"/>.
		/// </summary>
		/// <typeparam name="TProperty">The type of the property</typeparam>
		/// <param name="entity">The entity to get the value from</param>
		/// <param name="property">The mapped property with the validation attributes</param>
		/// <param name="logicalAttributeName">The logical attribute name</param>
		/// <param name="value">The value to set the attribute to</param>
		[SuppressMessage("Design", "CA1000:Do not declare static members on generic types", Justification = "Only called via code weaving by generated code, so nothing unclear for users")]
		public static void SetValueTypeAttribute<TProperty>(TEntity entity, PropertyInfo property, string logicalAttributeName, TProperty value)
			where TProperty : struct
		{
			if (entity is null)
				throw new ArgumentNullException(nameof(entity));
			if (property is null)
				throw new ArgumentNullException(nameof(property));
			if (string.IsNullOrEmpty(logicalAttributeName))
				throw new ArgumentNullException(nameof(logicalAttributeName));

			if (!ValidateAttributes(property, value, out Exception exception))
				throw exception;

			entity[logicalAttributeName] = value;
		}

		/// <summary>
		/// Gets a reference value named <paramref name="logicalAttributeName"/> from the provided <paramref name="entity"/>.
		/// </summary>
		/// <typeparam name="TProperty">The type of the result</typeparam>
		/// <param name="entity">The entity to get the value from</param>
		/// <param name="property">The mapped property with the validation attributes</param>
		/// <param name="logicalAttributeName">The logical attribute name</param>
		/// <returns>The value of the attribute</returns>
		[SuppressMessage("Design", "CA1000:Do not declare static members on generic types", Justification = "Only called via code weaving by generated code, so nothing unclear for users")]
		public static TProperty? GetNullableValueTypeAttribute<TProperty>(TEntity entity, PropertyInfo property, string logicalAttributeName)
			where TProperty : struct
		{
			if (entity is null)
				throw new ArgumentNullException(nameof(entity));
			if (property is null)
				throw new ArgumentNullException(nameof(property));
			if (string.IsNullOrEmpty(logicalAttributeName))
				throw new ArgumentNullException(nameof(logicalAttributeName));

			bool isRequired = !(property.GetCustomAttribute<RequiredAttribute>() is null);

			// try to get the value
			if (entity.Attributes.TryGetValue(logicalAttributeName, out object untypedValue))
			{
				// null handling
				if (untypedValue is null)
				{
					if (isRequired)
						throw new NullReferenceException($"Null value found for attribute '{logicalAttributeName}' which is marked as required.");
					else
						return null;
				}
				// wrong type
				if (!(untypedValue is TProperty typedValue))
					throw new InvalidCastException($"Found wrongly typed value in attribute '{logicalAttributeName}'. Found '{untypedValue.GetType()}' while expecting '{typeof(TProperty)}'.");

				return typedValue;
			}

			// no value, but required, throw
			if (isRequired)
				throw new KeyNotFoundException($"No attribute named '{logicalAttributeName}' found for attribute marked as required.");

			// no value, return default
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
		[SuppressMessage("Design", "CA1000:Do not declare static members on generic types", Justification = "Only called via code weaving by generated code, so nothing unclear for users")]
		public static void SetNullableValueTypeAttribute<TProperty>(TEntity entity, PropertyInfo property, string logicalAttributeName, TProperty? value)
			where TProperty : struct
		{
			if (entity is null)
				throw new ArgumentNullException(nameof(entity));
			if (property is null)
				throw new ArgumentNullException(nameof(property));
			if (string.IsNullOrEmpty(logicalAttributeName))
				throw new ArgumentNullException(nameof(logicalAttributeName));

			if (!ValidateAttributes(property, value, out Exception exception))
				throw exception;

			bool isRequired = !(property.GetCustomAttribute<RequiredAttribute>() is null);

			// if marked as required and value is null, throw
			if (isRequired && !value.HasValue)
				throw new ArgumentNullException(nameof(value), $"The attribute '{logicalAttributeName}' is required");

			entity.Attributes[logicalAttributeName] = value;
		}

		/// <summary>
		/// Validates the attributes on a property during set
		/// </summary>
		/// <param name="property">The property to validate</param>
		/// <param name="value">The value being set on the property</param>
		/// <param name="exception">If failed, the exception that should be thrown</param>
		/// <returns>True when validation was successfull</returns>
		[SuppressMessage("Design", "CA1000:Do not declare static members on generic types", Justification = "Only called via code weaving by generated code, so nothing unclear for users")]
		public static bool ValidateAttributes(PropertyInfo property, object value, out Exception exception)
		{
			IEnumerable<ValidationAttribute> attributes = property.GetCustomAttributes().OfType<ValidationAttribute>();
			foreach (ValidationAttribute attribute in attributes)
			{
				if (!attribute.IsValid(value))
				{
					if (attribute is RequiredAttribute)
					{
						exception = new ArgumentNullException(nameof(value), attribute.ErrorMessage);
						return false;
					}

					if (attribute is StringLengthAttribute || attribute is RangeAttribute)
					{
						exception = new ArgumentOutOfRangeException(nameof(value), attribute.ErrorMessage);
						return false;
					}

					exception = new ArgumentException(attribute.ErrorMessage, nameof(value));
					return false;
				}
			}

			exception = null;
			return true;
		}

	}
}
