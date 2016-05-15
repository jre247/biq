using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using BrightLine.Common.Core.Attributes;

namespace BrightLine.Core
{
	public interface IRepository<T> where T : class, IEntity, new()
	{
		Dictionary<IEntity, IEnumerable<string>> Changes { get; }

		void Insert(T o);

		T Get(int id, params string[] inclusions);
		IQueryable<T> GetAll(bool includeDeleted = false, params string[] inclusions);
		IQueryable<T> Where(Expression<Func<T, bool>> predicate, bool includeDeleted = false, params string[] inclusions);

		void Update(T o);

		void Delete(T o, DeleteTypes deleteType = DeleteTypes.Hard);
		void Delete(int[] id, DeleteTypes deleteType = DeleteTypes.Hard);
		void Delete(Expression<Func<T, object>> predicate, params int[] ids);
		void Delete(Expression<Func<T, object>> predicate, DeleteTypes deleteType = DeleteTypes.Hard, params int[] ids);

		void Save();
		void Restore(T o);
		IQueryable<T> SqlQuery(string query, params object[] parameters);
	}
}
