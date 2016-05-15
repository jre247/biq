using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BrightLine.Core;
using BrightLine.Common.Models;
using BrightLine.Core.Attributes;


namespace BrightLine.Common.Services
{
	public interface IAuditEventService : ICrudService<AuditEvent>
	{
		/// <summary>
		/// Executes an audit action
		/// </summary>
		/// <param name="operation"></param>
		/// <param name="group"></param>
		/// <param name="source"></param>
		void Audit(string operation, string group, string source);
	}
}
