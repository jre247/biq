using BrightLine.Common.Models;
using BrightLine.Publishing.Areas.AdResponses.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrightLine.Publishing.Areas.AdResponses.Interfaces
{
	public interface IAdTypeAdResponses
	{
		IEnumerable<AdResponseViewModel> GetAdResponses();
	}
}
