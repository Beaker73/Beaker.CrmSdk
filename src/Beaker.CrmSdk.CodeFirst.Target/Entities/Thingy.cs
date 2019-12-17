using Beaker.Crm.CodeFirst.Composition;
using Beaker.Crm.CodeFirst.Composition.Attributes;
using Beaker.CrmSdk.CodeFirst;
using Microsoft.Xrm.Sdk;

using System.ComponentModel.DataAnnotations;

namespace CrmCodeFirst.Source.Entities
{
	/// <summary>
	/// A Thingy
	/// </summary>
	[Entity]
	public sealed class Thingy
		: Entity
	{
		/// <summary>
		/// optional string value with a max length
		/// </summary>
		[StringLength(100)]
		[AttributeLogicalName("my_optionalstringwithmaxlength")]
		public string OptionalStringWithMaxLength
		{
			get => CodeFirstAspect<Thingy>.GetReferenceTypeAttribute<string>(this, typeof(Thingy).GetProperty("OptionalStringWithMaxLength"), "my_optionalstringwithmaxlength");
			set => CodeFirstAspect<Thingy>.SetReferenceTypeAttribute<string>(this, typeof(Thingy).GetProperty("OptionalStringWithMaxLength"), "my_optionalstringwithmaxlength", value);
		}

		/// <summary>
		/// required string value with a max length
		/// </summary>
		[StringLength(50)]
		[Required]
		[AttributeLogicalName("my_requiredstringwithmaxlength")]
		public string RequiredStringWithMaxLength
		{
			get => CodeFirstAspect<Thingy>.GetReferenceTypeAttribute<string>(this, typeof(Thingy).GetProperty("RequiredStringWithMaxLength"), "my_optionalstringwithmaxlength");
			set => CodeFirstAspect<Thingy>.SetReferenceTypeAttribute<string>(this, typeof(Thingy).GetProperty("RequiredStringWithMaxLength"), "my_optionalstringwithmaxlength", value);
		}

		/// <summary>
		/// optional integer with a range
		/// </summary>
		[Range(50, 100)]
		[AttributeLogicalName("my_OptionalWithIntRange")]
		public int? OptionalWithIntRange
		{
			get => CodeFirstAspect<Thingy>.GetNullableValueTypeAttribute<int>(this, typeof(Thingy).GetProperty("OptionalWithIntRange"), "my_optionalwithintrange");
			set => CodeFirstAspect<Thingy>.SetNullableValueTypeAttribute<int>(this, typeof(Thingy).GetProperty("OptionalWithIntRange"), "my_optionalwithintrange", value);
		}

		/// <summary>
		/// required integer with a range
		/// </summary>
		[Range(50, 100)]
		public int RequiredWithIntRange
		{
			get => CodeFirstAspect<Thingy>.GetValueTypeAttribute<int>(this, typeof(Thingy).GetProperty("OptionalWithIntRange"), "my_optionalwithintrange");
			set => CodeFirstAspect<Thingy>.SetValueTypeAttribute<int>(this, typeof(Thingy).GetProperty("OptionalWithIntRange"), "my_optionalwithintrange", value);
		}
	}
}
