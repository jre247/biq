using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrightLine.CMS
{
	public class CmsValidationException : Exception
	{
	    public CmsValidationException()
	    {
	    }


        public CmsValidationException(string message)
        {
            Errors = new List<string>();
            Errors.Add(message);
        }


	    public List<string> Errors;
	}
}
