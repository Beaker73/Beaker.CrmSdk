using Microsoft.Xrm.Sdk;

using System;

namespace Beaker.Crm.Composition
{
	/// <summary>
	/// Typed reference to an entity
	/// </summary>
	/// <typeparam name="TEntity">The type of the entity being referenced</typeparam>
	public sealed class EntityReference<TEntity>
	{
		/// <summary>
		/// Initializes a new entity reference
		/// </summary>
		/// <param name="id">The if of the entity being referenced</param>
		/// <param name="name">The name of the entity being referenced</param>
		/// <param name="rowVersion">The row versiono of the entity being referenced</param>
		public EntityReference(Guid id, string name = null, string rowVersion = null )
		{
			Id = id;
			Name = name;
			RowVersion = rowVersion;
		}

		/// <summary>
		/// Gets the Id of the referenced entity
		/// </summary>
		public Guid Id { get; }

		/// <summary>
		/// Gets the name of the reference
		/// </summary>
		public string Name { get; }

		/// <summary>
		/// Gets the row version of the reference
		/// </summary>
		public string RowVersion { get; }

		/// <summary>
		/// Converts the type reference to an untyped reference
		/// </summary>
		/// <param name="typedReference">The typed reference to convert</param>
		public static implicit operator EntityReference(EntityReference<TEntity> typedReference)
		{
			var reference = new EntityReference
			{
				LogicalName = CrmNameHelper<TEntity>.EntityLogicalName,
				Id = typedReference.Id
			};

			if (!String.IsNullOrEmpty(typedReference.Name))
				reference.Name = typedReference.Name;
			if (!String.IsNullOrEmpty(typedReference.RowVersion))
				reference.RowVersion = typedReference.RowVersion;

			return reference;
		}
	}
}
