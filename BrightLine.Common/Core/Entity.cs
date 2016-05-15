using BrightLine.Core.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;

namespace BrightLine.Core
{
	[DataContract(IsReference = true)]
	public class EntityBase : IEntity
	{
		[DataMember]
		[EntityEditor(AllowEdit = false, IsHidden = true)]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public virtual int Id { get; set; }

		[DataMember]
		[EntityEditor(AllowEdit = false, IsHidden = true)]
		public virtual bool IsDeleted { get; set; }

		[DataMember]
		[EntityEditor(AllowEdit = false, IsHidden = true, ShowInListing = true, Display = "Date created", DataFormatString = "MM/dd/yyyy")]
		public virtual DateTime DateCreated { get; set; }

		[DataMember]
		[EntityEditor(AllowEdit = false, IsHidden = true, ShowInListing = true, Display = "Date updated", DataFormatString = "MM/dd/yyyy")]
		public virtual DateTime? DateUpdated { get; set; }

		[DataMember]
		[EntityEditor(AllowEdit = false, IsHidden = true)]
		public virtual DateTime? DateDeleted { get; set; }

		public bool IsNewEntity { get { return Id <= 0; } }

		[DataMember]
		[EntityEditor(IsHidden = true)]
		public virtual string Display { get; set; }

		[DataMember]
		[EntityEditor(IsHidden = true)]
		public virtual string ShortDisplay { get; set; }

		public override string ToString()
		{
			return Display;
		}

		public override bool Equals(object other)
		{
			var o = other as IEntity;
			return (o != null && this.GetHashCode() == o.GetHashCode());
		}

		public override int GetHashCode()
		{
			var type = base.GetType();
			if (type.FullName.Contains("System.Data.Entity.DynamicProxies"))
				type = type.BaseType;

			return (this.IsNewEntity) ? base.GetHashCode() : this.Id.GetHashCode() + type.GetHashCode();
		}

		public Type GetBaseType()
		{
			var type = base.GetType();
			if (type.FullName.Contains("System.Data.Entity.DynamicProxies"))
				type = type.BaseType;

			return type;
		}

        public virtual bool IsValid()
        {
            return true;
        }
	}
}
