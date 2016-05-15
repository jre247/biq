using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BrightLine.Common.Data;

namespace BrightLine.OLAP
{
	public class AggregationRepository<T> : IAggregationRepository<T> where T : class, IAggregation, new()
	{
		
		protected DbContext dbContext;
		public AggregationRepository()
		{
            dbContext = OLAPContextSingleton.Instance;
		}

		public virtual IQueryable<T> GetAll()
		{
			return dbContext.Set<T>().Where(o => o.IsDeleted == false);
		}
	}
}
