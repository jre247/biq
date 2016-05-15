using BrightLine.Common.Core.Attributes;
using BrightLine.Common.Utility;
using BrightLine.Core;
using BrightLine.Utility.Helpers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;


namespace BrightLine.Tests.Common
{
	/// <summary>
	/// UNIT - Test  Implementation.
	/// 
	/// NOTE: This is only used for UNIT-TESTS:
	/// The real repository is RepositorySql which actually connects to a database.
	/// </summary>
	/// <typeparam name="T">The type of the entity.</typeparam>
	public class EntityRepositoryInMemory<T> : IRepository<T> where T : class, IEntity, new()
	{
		private Dictionary<int, T> _itemsMap;

		public EntityRepositoryInMemory()
		{
			_itemsMap = new Dictionary<int, T>();
		}

		public virtual void Insert(T o)
		{
			if (o == null)
				return;

			if (o.Id == 0)
			{
				if (_itemsMap.Keys.Count > 0)
					o.Id = _itemsMap.Keys.Max() + 1;
				else
					o.Id = 1;
			}

			o.DateCreated = DateTime.UtcNow;
			_itemsMap.Add(o.Id, o);
		}
		
		public virtual T Get(int id, params string[] inclusions)
		{
			if (_itemsMap == null || _itemsMap.Count == 0)
				return default(T);

			if (!_itemsMap.ContainsKey(id))
				return default(T);

			return _itemsMap[id];
		}

		public virtual IQueryable<T> GetAll(bool includeDeleted = false, params string[] inclusions)
		{
			var clonedList = ShallowClone(false);
			return clonedList.AsQueryable<T>();
		}

		public virtual IQueryable<T> Where(Expression<Func<T, bool>> predicate, bool showDeleted = false, params string[] inclusions)
		{
			// Get unique cloned (shallow copy ) list.
			var filtered = ShallowClone(showDeleted);

			// Only get non-soft deleted items.
			if (predicate == null || filtered == null || filtered.Count == 0)
				return filtered as IQueryable<T>;

			var queryable = filtered.AsQueryable<T>();
			var result = queryable.Where(predicate);
			return result;
		}
		
		public virtual void Update(T o)
		{
			if (o == null)
				return;

			if (!_itemsMap.ContainsKey(o.Id))
				return;

			o.DateUpdated = DateTime.UtcNow;
			_itemsMap[o.Id] = o;
		}
		
		public void Delete(T o, DeleteTypes deleteType = DeleteTypes.Hard)
		{
			if (o == null)
				return;

			if (!_itemsMap.ContainsKey(o.Id))
				return;

			var softDelete = (deleteType == DeleteTypes.Soft || deleteType == DeleteTypes.SoftCascade);
			if (softDelete)
			{
				o.IsDeleted = true;
				o.DateDeleted = DateTime.UtcNow;
				if (deleteType == DeleteTypes.SoftCascade)
					CascadeDelete(o);
			}
			else
				_itemsMap.Remove(o.Id);
		}
		
		public void Delete(int[] ids, DeleteTypes deleteType = DeleteTypes.Hard)
		{
			var entities = Where((e) => ids.Contains(e.Id));
			foreach (var entity in entities)
			{
				Delete(entity, deleteType);
			}
		}
		
		public void Delete(Expression<Func<T, object>> predicate, params int[] ids)
		{
			Delete(predicate, DeleteTypes.Hard, ids);
		}
		
		public void Delete(Expression<Func<T, object>> predicate, DeleteTypes deleteType = DeleteTypes.Hard, params int[] ids)
		{
			if (ids == null || ids.Length == 0)
				return;

			if (predicate == null)
				throw new ArgumentException("Must provide an expression to determine filter for bulk delete operation.");

			var deleted = new List<T>();
			var pi = ExpressionHelper.GetPropertyInfo(predicate);
			foreach (var item in _itemsMap.Values)
			{
				if (pi.GetGetMethod().IsVirtual)
				{
					var itemProperty = item.GetType().GetProperty(pi.Name);
					var propertyValue = itemProperty.GetValue(item) as IEntity;
					if (propertyValue == null)
						continue;

					var propertyId = propertyValue.Id;
					if (ids.Contains(propertyId))
						deleted.Add(item);
				}
				else
					deleted.Add(item);
			}

			foreach (var delete in deleted)
				Delete(delete, deleteType);
		}
		
		public void Save()
		{

		}
		
		public void Restore(T o)
		{
			if (o == null)
				return;

			if (!_itemsMap.ContainsKey(o.Id))
				return;

			o.DateUpdated = DateTime.UtcNow;
			o.DateDeleted = null;
			o.IsDeleted = false;
		}
		
		public IQueryable<T> SqlQuery(string query, params object[] parameters)
		{
			return new EnumerableQuery<T>(new T[0]);
		}

		#region Private methods

		private List<T> ShallowClone(bool showDeleted)
		{
			return _itemsMap.Select(im => im.Value).Where(i => showDeleted || !i.IsDeleted).ToList();
		}

		private void CascadeDelete(T instance)
		{
			if (instance == null)
				return;

			var type = instance.GetType();
			if (type.FullName.Contains("System.Data.Entity.DynamicProxies"))
				type = type.BaseType;
			foreach (var property in type.GetProperties())
			{
				var entityType = property.PropertyType;
				var propertyType = property.PropertyType;
				var value = ReflectionHelper.TryGetValue(instance, property);
				if (value == null)
					continue;

				if (propertyType != typeof(string) && propertyType.GetInterface("IEnumerable", false) != null)
				{
					entityType = propertyType.GetGenericArguments()[0];
					if (entityType.GetInterface("IEntity", false) == null)
						continue;
				}

				if (!property.GetCustomAttributes(typeof(CascadeSoftDeleteAttribute), true).Any())
					continue;

				if (propertyType.GetInterface("IEnumerable", false) != null)
				{
					var em = EntityManager.GetManager(entityType);
					foreach (var v in value as IEnumerable)
					{
						var entity = v as IEntity;
						if (entity == null)
							continue;

						em.Delete(entity.Id, DeleteTypes.SoftCascade);
					}
				}
				else if (propertyType.GetInterface("IEntity", false) != null)
				{
					var em = EntityManager.GetManager(entityType);
					em.Delete(((IEntity)value).Id, DeleteTypes.SoftCascade);
				}
			}
		}
		#endregion

		public Dictionary<IEntity, IEnumerable<string>> Changes
		{
			get { throw new NotImplementedException(); }
		}
	}
}