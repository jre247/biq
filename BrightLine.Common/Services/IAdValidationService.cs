using BrightLine.Common.Framework.Exceptions;
using BrightLine.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrightLine.Common.Services
{
	public interface IAdValidationService
	{
		void ValidateAd(ValidationException vex, Ad ad);
	}
}
