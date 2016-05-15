using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using BrightLine.Common.Core.Attributes;

namespace BrightLine.Core
{
	public class CrudService<TEntity> : ICrudService<TEntity> where TEntity : EntityBase, new()
	{
		protected IRepository<TEntity> Repository;

		#region Events

		protected event EventHandler<CrudBeforeEventArgs> Creating;
		protected event EventHandler<CrudAfterEventArgs> Created;
		protected event EventHandler<CrudBeforeEventArgs> Updating;
		protected event EventHandler<CrudAfterEventArgs> Updated;
		protected event EventHandler<CrudBeforeEventArgs> Deleting;
		protected event EventHandler<CrudAfterEventArgs> Deleted;
		protected class CrudBeforeEventArgs
		{
			public bool Cancel { get; set; }
		}
		protected class CrudAfterEventArgs
		{
			public Exception Exception { get; set; }
		}

		#endregion

		public CrudService(IRepository<TEntity> repo)
		{
			Repository = repo;
		}

		#region Public methods

		public virtual TEntity Create(TEntity entity)
		{
			CallBefore(Creating, entity);
			InsertInternal(entity);

			SaveInternal();

			return entity;
		}

		/// <summary>
		/// *Note: When the SuppressSave parameter is equal to true then no entity changes are pushed to the database by Entity Framework
		/// </summary>
		/// <param name="entity"></param>
		/// <param name="suppressSave"></param>
		/// <returns></returns>
		public virtual TEntity Create(TEntity entity, bool suppressSave)
		{
			CallBefore(Creating, entity);
			InsertInternal(entity);

			if (!suppressSave)
				SaveInternal();

			return entity;
		}

		public virtual List<TEntity> Create(IEnumerable<TEntity> entities)
		{
			if (entities == null)
				return new List<TEntity>();

			foreach (var entity in entities)
			{
				CallBefore(Creating, entity);
				InsertInternal(entity);
			}

			SaveInternal();
			return entities.ToList();
		}

		public virtual TEntity Get(int id, params string[] inclusions)
		{
			var entity = Repository.Get(id, inclusions);
			return entity;
		}

		public virtual IQueryable<TEntity> GetAll(bool includeDeleted = false, params string[] inclusions)
		{
			var all = Repository.GetAll(includeDeleted, inclusions) ?? new EnumerableQuery<TEntity>(new TEntity[0]);
			return all;
		}

		public virtual IQueryable<TEntity> Where(Expression<Func<TEntity, bool>> predicate, bool includeDeleted = false, params string[] inclusions)
		{
			var where = Repository.Where(predicate, includeDeleted, inclusions) ?? new EnumerableQuery<TEntity>(new TEntity[0]);
			return where;
		}

		public virtual TEntity Update(TEntity entity)
		{
			CallBefore(Updating, entity);
			UpdateInternal(entity);

			SaveInternal();
			return entity;
		}

		/// <summary>
		/// *Note: When the SuppressSave parameter is equal to true then no entity changes are pushed to the database by Entity Framework
		/// </summary>
		/// <param name="entity"></param>
		/// <param name="suppressSave"></param>
		/// <returns></returns>
		public virtual TEntity Update(TEntity entity, bool suppressSave)
		{
			CallBefore(Updating, entity);
			UpdateInternal(entity);

			if (!suppressSave)
				SaveInternal();

			return entity;
		}

		public virtual List<TEntity> Update(IEnumerable<TEntity> entities)
		{
			if (entities == null)
				return new List<TEntity>();

			foreach (var entity in entities)
			{
				CallBefore(Updating, entity);
				UpdateInternal(entity);
			}

			SaveInternal();
			return entities.ToList();
		}

		public virtual TEntity Upsert(TEntity entity)
		{
			if (entity == null)
				return null;

			if (entity.IsNewEntity)
				Create(entity);
			else
				Update(entity);

			SaveInternal();
			return entity;
		}

		public virtual TEntity Upsert(TEntity entity, bool suppressSave)
		{
			if (entity == null)
				return null;

			if (entity.IsNewEntity)
				Create(entity, suppressSave);
			else
				Update(entity, suppressSave);

			if (!suppressSave)
				SaveInternal();
			
			return entity;
		}

		public virtual List<TEntity> Upsert(IEnumerable<TEntity> entities)
		{
			if (entities == null)
				return new List<TEntity>();

			foreach (var entity in entities)
			{
				if (entity.IsNewEntity)
					Create(entity);
				else
					Update(entity);
			}

			SaveInternal();
			return entities.ToList();
		}

		public virtual void Delete(int id, DeleteTypes deleteType = DeleteTypes.Hard)
		{
			var entity = Repository.Get(id);
			CallBefore(Deleting, entity);
			DeleteInternal(entity, deleteType);
		}

		public virtual void Delete(Expression<Func<TEntity, object>> predicate, DeleteTypes deleteType = DeleteTypes.Hard, params int[] ids)
		{
			CallBefore(Deleting, null);
			DeleteInternal(predicate, deleteType, ids);

			SaveInternal();
		}

		public virtual TEntity Cud(TEntity entity, DeleteTypes deleteType = DeleteTypes.Hard)
		{
			if (entity == null)
				return null;

			if (entity.IsNewEntity)
				Create(entity);
			else if (entity.IsDeleted)
				Delete(entity.Id, deleteType);
			else
				Update(entity);

			SaveInternal();
			return entity;
		}

		public virtual List<TEntity> Cud(IEnumerable<TEntity> entities, DeleteTypes deleteType = DeleteTypes.Hard)
		{
			var cu = new List<TEntity>();
			foreach (var entity in entities)
			{
				if (entity.IsNewEntity)
					Create(entity);
				else if (entity.IsDeleted)
					Delete(entity.Id, deleteType);
				else
					Update(entity);
			}

			SaveInternal();
			return cu;
		}

		//public virtual void Save()
		//{
		//	SaveInternal();
		//}

		public virtual void Restore(int id)
		{
			var entity = Repository.Get(id);
			Repository.Restore(entity);
			SaveInternal();
		}

		public virtual IQueryable<TEntity> SqlQuery(string query, params object[] parameters)
		{
			var results = Repository.SqlQuery(query, parameters) ?? new EnumerableQuery<TEntity>(new TEntity[0]);
			return results;
		}

		public void Save()
		{
			SaveInternal();
		}

		#endregion

		#region Private methods

		private void SaveInternal()
		{
			Repository.Save();
		}

		private void UpdateInternal(TEntity entity)
		{
			Repository.Update(entity);
		}

		private void InsertInternal(TEntity entity)
		{
			Repository.Insert(entity);
		}

		private void DeleteInternal(TEntity entity, DeleteTypes deleteType)
		{
			Repository.Delete(entity, deleteType);
		}

		private void DeleteInternal(Expression<Func<TEntity, object>> predicate, DeleteTypes deleteType = DeleteTypes.Hard,
								   params int[] ids)
		{
			Repository.Delete(predicate, deleteType: deleteType, ids: ids);
		}

		/// <summary>
		/// Calls the before action handler if it exists
		/// </summary>
		/// <param name="handler">The handler</param>
		/// <param name="entity">Entity associated with operation</param>
		/// <returns>True if call succeeded or handler is null, false if should be cancelled.</returns>
		private static bool CallBefore(EventHandler<CrudBeforeEventArgs> handler, TEntity entity)
		{
			var handle = handler;
			if (handle == null)
				return true;

			var args = new CrudBeforeEventArgs();
			handle(entity, args);
			return !args.Cancel;
		}

		/// <summary>
		/// Calls the after action handler if it exists
		/// </summary>
		/// <param name="handler">The handler</param>
		/// <param name="entity">Entity associated with operation</param>
		/// <returns>True if call succeeded or handler is null, false if exception has been raised.</returns>
		private static bool CallAfter(EventHandler<CrudAfterEventArgs> handler, TEntity entity)
		{
			var handle = handler;
			if (handle == null)
				return true;

			var args = new CrudAfterEventArgs();
			handle(entity, args);
			return (args.Exception == null);
		}

		#endregion

		
	}
}