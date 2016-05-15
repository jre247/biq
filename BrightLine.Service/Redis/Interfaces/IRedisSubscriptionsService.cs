using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrightLine.Service.Redis.Interfaces
{
	public interface IRedisSubscriptionsService
	{
		void Setup();
		void RefreshBrsFiles();
	}
}
