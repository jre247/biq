using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using BrightLine.Common.Core.Attributes;


namespace BrightLine.Core
{
	public interface ICrudService<TEntity> where TEntity : EntityBase, IEntity, new()
	{
		TEntity Create(TEntity item);
		TEntity Create(TEntity item, bool suppressSave);
		List<TEntity> Create(IEnumerable<TEntity> items);

		TEntity Get(int id, params string[] inclusions);
		IQueryable<TEntity> GetAll(bool includeDeleted = false, params string[] inclusions);
		IQueryable<TEntity> Where(Expression<Func<TEntity, bool>> func, bool includeDeleted = false, params string[] inclusions);

		TEntity Update(TEntity o);
		TEntity Update(TEntity o, bool suppressSave);
		List<TEntity> Update(IEnumerable<TEntity> items);
		TEntity Upsert(TEntity item);
		List<TEntity> Upsert(IEnumerable<TEntity> items);
		TEntity Upsert(TEntity entity, bool suppressSave);

		void Delete(int id, DeleteTypes deleteType = DeleteTypes.Hard);
		void Delete(Expression<Func<TEntity, object>> predicate, DeleteTypes deleteType = DeleteTypes.Hard, params int[] ids);

		TEntity Cud(TEntity item, DeleteTypes deleteType = DeleteTypes.Hard);
		List<TEntity> Cud(IEnumerable<TEntity> items, DeleteTypes deleteType = DeleteTypes.Hard);

		void Save();
		void Restore(int id);
		IQueryable<TEntity> SqlQuery(string query, params object[] parameters);
	}
}