using System;
using System.ComponentModel.DataAnnotations.Schema;
using BrightLine.Core;
using System.ComponentModel.DataAnnotations;

namespace BrightLine.Common.Models
{
	public class FileItem : EntityBase, IEntity
	{
		/// <summary>
		/// Name of the resource document
		/// </summary>
		[StringLength(255)]
		public string Name { get; set; }


		/// <summary>
		/// File extension
		/// </summary>
		public string Extension { get; set; }


		/// <summary>
		/// Contents as bytes.
		/// </summary>
		[NotMapped]
		public byte[] Contents { get; set; }


		/// <summary>
		/// The time of the file
		/// </summary>
		public DateTime LastWriteTime { get; set; }


		/// <summary>
		/// The original full name
		/// </summary>
		[StringLength(255)]
		public string FullNameRaw { get; set; }


		/// <summary>
		/// Length of the file in bytes.
		/// </summary>
		public int Length { get; set; }


		/// <summary>
		/// Name of the file that is actualy saved.
		/// </summary>
		/// <returns></returns>
		public string FullName()
		{
			return this.Id + "_" + Name + "." + Extension;
		}
	}
}