using Microsoft.Xrm.Sdk;
using System;

namespace Beaker.CrmSdk.Composition.Attributes
{
	/// <summary>
	/// Attribute to mark the type of target entity to handle
	/// </summary>
	[AttributeUsage(AttributeTargets.Class)]
	public sealed class TargetAttribute
		: Attribute
	{
		/// <summary>
		/// Initializes a new target attribute
		/// </summary>
		/// <param name="targetEntity">The type of target entity this plugin step will handle</param>
		public TargetAttribute(Type targetEntity)
		{
			if (!typeof(Entity).IsAssignableFrom(targetEntity))
				throw new ArgumentException("Type should derive from Entity", nameof(targetEntity));
			TargetEntity = targetEntity;
		}

		/// <summary>
		/// Gets the type of the target entity
		/// </summary>
		public Type TargetEntity { get; }
	}
}
