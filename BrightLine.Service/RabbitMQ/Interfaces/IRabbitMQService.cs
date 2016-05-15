using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrightLine.Service.RabbitMQ.Interfaces
{
	public interface IRabbitMQService
	{
		void Send(string exchange, string queue, string message, string routingKey);
	}
}
