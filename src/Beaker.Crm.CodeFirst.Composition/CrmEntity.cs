using Microsoft.Xrm.Sdk;
using System;

namespace Beaker.Crm.CodeFirst.Composition
{
	/// <summary>
	/// Based class for code first entities that also need to access the default CRM attributes
	/// </summary>
	public abstract class CrmEntity
		: Entity
	{
		/// <summary>
		/// The UTC Date and Time this record was created on
		/// </summary>
		public DateTime CreatedOn { get; set; }
		/// <summary>
		/// The user who created the record
		/// </summary>
		public EntityReference CreatedBy { get; set; }

		/// <summary>
		/// The UTC Date and Time this record was modified on
		/// </summary>
		public DateTime ModifiedOn {get; set; }
		/// <summary>
		/// The user who modified the record
		/// </summary>
		public EntityReference ModifiedBy { get; set; }
	}
}
