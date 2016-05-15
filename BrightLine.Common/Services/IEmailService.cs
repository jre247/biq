using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace BrightLine.Common.Services
{
	public interface IEmailService
	{
		void SendEmail(MailMessage message, SmtpClient client = null);
	}
}
