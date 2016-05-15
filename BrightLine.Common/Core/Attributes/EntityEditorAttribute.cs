using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrightLine.Core.Attributes
{
	/// <summary>
	/// Describes the manner in which a property should be edited.
	/// </summary>
	[AttributeUsageAttribute(AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
	public class EntityEditorAttribute : Attribute
	{
		public EntityEditorAttribute()
		{
			AllowEdit = true;
			Order = int.MaxValue; // would use a nullable int, but an attribute cannot be initialized with a nullable value.  See: http://stackoverflow.com/questions/3765339/nullable-int-in-attribute-constructor-or-property
		}

		/// <summary>
		/// Gets or sets whether this property can be changed once it is set. Default is true.
		/// </summary>
		public bool AllowEdit { get; set; }

		/// <summary>
		/// Gets or sets whether this property should only be added as a hidden field.
		/// </summary>
		public bool IsHidden { get; set; }

		/// <summary>
		/// Gets or sets whether this property was selected from a collection of items.
		/// </summary>
		public bool IsFromCollection { get; set; }

		/// <summary>
		/// Gets or sets the editor data type.
		/// </summary>
		public DataType EditType { get; set; }

		/// <summary>
		/// Gets or sets the data format string to be used when displaying the initial value in the input field.
		/// </summary>
		public string DataFormatString { get; set; }

		/// <summary>
		/// Gets or sets the display label text.
		/// </summary>
		public string Display { get; set; }

		/// <summary>
		/// Gets or sets the whether the value can be set at create time.
		/// </summary>
		public bool DisplayOnly { get; set; }

		/// <summary>
		/// Gets or sets the order the property should be displayed in the editor and details view.
		/// </summary>
		public int Order { get; set; }

		/// <summary>
		/// Gets or sets whether the column should be shown in the models listing. Default is false.
		/// </summary>
		public bool ShowInListing { get; set; }

		/// <summary>
		/// Gets or sets whether the property should be copied when Entity has AllowCopy == true. Default is false.
		/// </summary>
		public bool CopyProperty { get; set; }

		/// <summary>
		/// Gets or sets whether the property should be copied when Entity has AllowCopy == true. Default is false.
		/// </summary>
		public bool IsUserId { get; set; }
	}
}