using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BrightLine.Core;
using BrightLine.Common.Models.Lookups;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.ObjectModel;

namespace BrightLine.Common.Models
{
	public class CmsModelInstanceField : EntityBase, IEntity
	{
		[Required]
		[StringLength(255)]
		public string Name { get; set; }

		public bool IsRequired { get; set; }

		[StringLength(255)]
		public string Metatype { get; set; }

		public virtual CmsModelInstance ModelInstance { get; set; }

		public int? FieldType_Id { get; set; }

		[ForeignKey("FieldType_Id")]
		public virtual FieldType FieldType { get; set; }

		private ICollection<CmsModelInstanceFieldValue> _Values { get; set; }
		public virtual ICollection<CmsModelInstanceFieldValue> Values
		{
			get { return _Values ?? (_Values = new Collection<CmsModelInstanceFieldValue>()); }
			protected set { _Values = value; }
		}
	}
}
