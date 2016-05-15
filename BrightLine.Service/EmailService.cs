using System.Net;
using System.Net.Mail;
using BrightLine.Common.Services;
using BrightLine.Common.Framework;

namespace BrightLine.Service
{
	public class EmailService : IEmailService	
	{
		/// <summary>
		/// Sends a MailMessage from a specific SmtpClient (or the default SmtpClient if not provided)
		/// </summary>
		/// <param name="message">The MailMessage to send.</param>
		/// <param name="client">The client to send the MailMessage.</param>
		public void SendEmail(MailMessage message, SmtpClient client = null)
		{
			if (client == null)
			{
				var svc = IoC.Resolve<ISettingsService>();

				client = new SmtpClient
					{
						Host = svc.SmtpHost,
						Port = svc.SmtpPort,
						Credentials = new NetworkCredential(svc.SmtpUserName, svc.SmtpPassword),
						EnableSsl = true
					};
			}

			client.Send(message);
		}
	}
}
