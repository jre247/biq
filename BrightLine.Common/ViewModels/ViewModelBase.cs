using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrightLine.Common.ViewModels
{
	public class ViewModelBase
	{
		public int Id { get; set; }

		public bool IsNewModel { get { return Id <= 0; } }
	}
}
