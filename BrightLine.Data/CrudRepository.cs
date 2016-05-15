using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using BrightLine.Common.Core.Attributes;
using BrightLine.Common.Utility;
using BrightLine.Core;
using System.Reflection;

namespace BrightLine.Data
{
	public class CrudRepository<T> : IRepository<T> where T : class, IEntity, new()
	{
		#region IRepository<T> Members

		public virtual Dictionary<IEntity, IEnumerable<string>> Changes
		{
			get
			{
				var changes = new Dictionary<IEntity, IEnumerable<string>>();
				var entries = OLTPContextSingleton.Instance.ChangeTracker.Entries().Where(e => e.State == EntityState.Modified);
				foreach (var entry in entries)
				{
					var entity = OLTPContextSingleton.Instance.Entry(entry.Entity);
					var changed = entity.CurrentValues.PropertyNames.Where(p => entity.Property(p).IsModified);
					changes.Add((IEntity)entity.Entity, changed);
				}

				return changes;
			}
		}

		public void Insert(T o)
		{
			if (o == null)
				return;

			if (o.DateCreated.Equals(DateTime.MinValue))
				o.DateCreated = DateTime.UtcNow; // Only set Date if Date has not already been set
			o.DateUpdated = DateTime.UtcNow;
			o.DateDeleted = null;
			OLTPContextSingleton.Instance.Set<T>().Add(o);
		}

		public T Get(int id, params string[] inclusions)
		{
			IQueryable<T> set = OLTPContextSingleton.Instance.Set<T>();
			var includes = inclusions.Aggregate(set, (current, include) => current.Include(include));
			var entity = new EnumerableQuery<T>(includes).FirstOrDefault(t => t.Id == id);
			return entity;
		}

		public virtual IQueryable<T> GetAll(bool includeDeleted = false, params string[] inclusions)
		{
			IQueryable<T> set = OLTPContextSingleton.Instance.Set<T>();
			var includes = inclusions.Aggregate(set, (current, include) => current.Include(include));
			var list = includes.Where(o => includeDeleted || o.IsDeleted == false);
			return list;
		}

		public virtual IQueryable<T> Where(Expression<Func<T, bool>> predicate, bool showDeleted = false,
										   params string[] inclusions)
		{
			if (predicate == null)
				return new EnumerableQuery<T>(new T[0]);

			IQueryable<T> set = OLTPContextSingleton.Instance.Set<T>();
			var includes = inclusions.Aggregate(set, (current, include) => current.Include(include));
			var where = includes.Where(predicate);
			if (!showDeleted)
				where = where.Where(o => o.IsDeleted == false);

			return where;
		}

		public void Update(T o)
		{
			if (o == null)
				return;

			o.DateUpdated = DateTime.UtcNow;
		}

		public void Delete(T o, DeleteTypes deleteType = DeleteTypes.Hard)
		{
			if (o == null)
				return;

			var softDelete = (deleteType == DeleteTypes.Soft || deleteType == DeleteTypes.SoftCascade);
			if (softDelete)
			{
				var today = DateTime.UtcNow;
				o.DateUpdated = today;
				o.DateDeleted = today;
				o.IsDeleted = true;
				if (deleteType == DeleteTypes.SoftCascade)
					CascadeDelete(o);
			}
			else if (OLTPContextSingleton.Instance.Set<T>().Find(o.Id) != null)
				OLTPContextSingleton.Instance.Set<T>().Remove(o);
		}

		public void Delete(int[] ids, DeleteTypes deleteType = DeleteTypes.Hard)
		{
			var entities = this.Where((e) => ids.Contains(e.Id));
			foreach (var entity in entities)
			{
				Delete(entity, deleteType);
			}
		}

		public void Delete(Expression<Func<T, object>> predicate, params int[] ids)
		{
			this.Delete(predicate, DeleteTypes.Hard, ids);
		}

		public virtual void Delete(Expression<Func<T, object>> predicate, DeleteTypes deleteType = DeleteTypes.Hard,
								   params int[] ids)
		{
			// NOTE: Entity framework does not support batch updates/deletes.
			// You have to pull in each object from the database into memory to update the objects.

			// WORKAROUND:
			// This is a batch delete using an expression tree but with limitations for safety.
			// 1. Only works on numbers. ( to prevent any potential unsafe sql text )
			// 2. Only supports 1 filter column which again has to be a numeric column and can be either a simple numeric property or foreign key

			// EXAMPLES:
			// 1. IoC.AdResults.Delete( result => result.Id, new int[] { 2, 3, 9 });   
			// 2. IoC.AdResults.Delete( result => result.Ad.Id, new int[] { 5, 7, 8 });
			// 
			// In first example, Id is a simple integer so sql generated is 
			//     update AdResult set IsDeleted = 1 where Id in ( 2, 3, 9 )
			// 
			// In second example, Ad is an property of type object and is virtual ( which means its a relationship ).
			// In this case the column in the sql update will be appended with "_Id" as it is the naming convention for entity framework.
			// So the sql generated will be :
			//    updated AdResult set IsDeleted = 1 where Ad_Id in  ( 5, 7, 8 );
			if (predicate == null)
				throw new ArgumentException("Must provide an expression to determine filter for bulk delete operation.");
			if (ids == null || ids.Length == 0)
				throw new ArgumentException("Must provide a list of ids to delete for batch deletes.");

			var tableName = typeof(T).Name;
			var columnName = GetEntityFrameworkColumnName(predicate);

			var clause = string.Join(", ", ids);
			var softDelete = (deleteType == DeleteTypes.Soft || deleteType == DeleteTypes.SoftCascade);
			var sql = (softDelete)
							 ? string.Format("update {0} set isDeleted=1, DateDeleted=getdate() where {1} in ({2});", tableName,
											 columnName, clause)
							 : string.Format("delete from {0} where {1} in ({2});", tableName, columnName, clause);
			OLTPContextSingleton.Instance.Database.ExecuteSqlCommand(sql);
		}

		public void Save()
		{
			try
			{
				//HACK (from Dmitry): required lazy loaded properties will be missing when SaveChanges is called
				//	workaround by accessing all lazy loaded properties before save
				var errors = OLTPContextSingleton.Instance.GetValidationErrors();
				foreach (var error in errors)
				{
					// HACK: continued, touch all required properties up the object graph.
					var entity = error.Entry.Entity;
					TouchRequired(entity, error);
				}

                var isValid = ValidateChangedEntities();

                if (!isValid)
                    throw new ValidationException();

				OLTPContextSingleton.Instance.SaveChanges();
			}
			catch (DbEntityValidationException e)
			{
				foreach (var eve in e.EntityValidationErrors)
				{
					Debug.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
									eve.Entry.Entity.GetType().Name, eve.Entry.State);
					foreach (var ve in eve.ValidationErrors)
					{
						Debug.WriteLine("- Property: \"{0}\", Error: \"{1}\"", ve.PropertyName, ve.ErrorMessage);
					}
				}
				throw;
			}
		}

        private static bool ValidateChangedEntities()
        {
            var isValid = true;
            var entries = OLTPContextSingleton.Instance.ChangeTracker.Entries().Where(e => e.State == EntityState.Modified
                || e.State == EntityState.Added).ToList();
           foreach (var entry in entries)
            {
                if (!((IEntity)entry.Entity).IsValid())
                {
                    isValid = false;
                    break;
                }
            }
            return isValid;
        }

		public virtual void Restore(T o)
		{
			if (o == null)
				return;

			o.DateUpdated = DateTime.UtcNow;
			o.DateDeleted = null;
			o.IsDeleted = false;
		}

		public virtual IQueryable<T> SqlQuery(string query, params object[] parameters)
		{
			if (string.IsNullOrWhiteSpace(query))
				return new EnumerableQuery<T>(new T[0]);

			var set = OLTPContextSingleton.Instance.Set<T>();
			var sql = set.SqlQuery(query, parameters);
			return new EnumerableQuery<T>(sql);
		}

		#endregion

		#region Private methods

		private static void TouchRequired(object instance, DbEntityValidationResult error)
		{
			if (instance == null)
				return;
			var t = instance.GetType();
			if (t == typeof(object))
				return;

			var bt = t.BaseType;
			foreach (var property in bt.GetProperties())
			{
				var btp = bt.GetProperty(property.Name);
				if (btp == null || !btp.GetCustomAttributes(typeof(RequiredAttribute), true).Any())
					continue;

				var value = property.GetValue(instance, null);
				if (value != null)
					TouchRequired(value, error);
			}
		}

		private static void CascadeDelete(T instance)
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
					foreach (var v in value as IQueryable)
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

		/// <summary>
		/// Get the property name from the expression.
		/// e.g. GetPropertyName<Person>( p => p.FirstName);
		/// </summary>
		/// <typeparam name="T">Type of item.</typeparam>
		/// <param name="exp">Expression.</param>
		/// <returns>Property name.</returns>
		private static string GetEntityFrameworkColumnName<T>(Expression<Func<T, object>> exp)
		{
			var name = GetPropertyAccessPath<T>(exp);
			if (!string.IsNullOrEmpty(name))
				return name.Replace(".", "_");
			return name;
		}

		/// <summary>
		/// Get the property name from the expression.
		/// e.g. GetPropertyName<Person>( p => p.FirstName);
		/// </summary>
		/// <typeparam name="T">Type of item.</typeparam>
		/// <param name="exp">Expression.</param>
		/// <returns>Property name.</returns>
		private static string GetPropertyAccessPath<T>(Expression<Func<T, object>> exp)
		{
			MemberExpression memberExpression = null;

			// Get memberexpression.
			if (exp.Body is MemberExpression)
				memberExpression = exp.Body as MemberExpression;

			if (exp.Body is UnaryExpression)
			{
				var unaryExpression = exp.Body as UnaryExpression;
				if (unaryExpression.Operand is MemberExpression)
					memberExpression = unaryExpression.Operand as MemberExpression;
			}

			if (memberExpression == null)
				throw new InvalidOperationException("Not a member access.");

			string name = "";

			// Nested.
			if (memberExpression.Expression != null && memberExpression.Expression.NodeType == ExpressionType.MemberAccess)
			{
				var firstProp = ((MemberExpression)memberExpression.Expression).Member.Name;
				name += firstProp + ".";
			}
			var info = memberExpression.Member as PropertyInfo;
			name += info.Name;
			return name;
		}


		#endregion
	}
}