using BrightLine.Common.Framework;
using BrightLine.Common.Services;
using BrightLine.Service.RabbitMQ.Interfaces;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrightLine.Service.RabbitMQ.Services
{
	public class RabbitMQService : IRabbitMQService
	{
		public void Send(string exchange, string queue, string message, string routingKey)
		{
			var settings = IoC.Resolve<ISettingsService>();

			var port = settings.RabbitMQDefaultPort;
			var host = settings.RabbitMQHost;
			var username = settings.RabbitMQUsername;
			var password = settings.RabbitMQPassword;

			try
			{
				var factory = new ConnectionFactory()
				{
					HostName = host,
					Port = port,
					UserName = username,
					Password = password
				};

				using (var connection = factory.CreateConnection())
				{
					using (var channel = connection.CreateModel())
					{

						var body = Encoding.UTF8.GetBytes(message);

						channel.BasicPublish(exchange: exchange,
											 routingKey: routingKey,
											 basicProperties: null,
											 body: body);
					}
				}
			}
			catch (Exception ex)
			{
				IoC.Log.Error("An unexpected error occurred in Rabbit MQ Service.", ex);

			}
		}
		
	}
}
