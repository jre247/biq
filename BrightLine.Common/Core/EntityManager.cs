using System;
using System.Collections.Generic;
using System.Linq;
using BrightLine.Common.Core;
using BrightLine.Common.Core.Attributes;
using BrightLine.Common.Framework;
using BrightLine.Common.Utility;
using System.Reflection;
using BrightLine.Common.Framework.Exceptions;

namespace BrightLine.Core
{
	/// <summary>
	/// General purpose Entity manager to perform CRUD on entities without using generics.
	/// This is useful for a single Controller which can manage several differnt but simple entities.
	/// </summary>
	public class EntityManager
	{
		private readonly Type _entityType;
		private object _service;
		private static EntityManager _manager;

		/// <summary>
		/// Entity manager.
		/// </summary>
		/// <param name="entityType"></param>
		private EntityManager(Type entityType)
		{
			_entityType = (entityType.FullName.Contains("System.Data.Entity.DynamicProxies")) ? entityType.BaseType : entityType;
			Model = _entityType.Name;
		}

		public string Model { get; private set; }

		#region Public methods

		public static EntityManager GetManager(string model, bool throwOnNonEntity = true)
		{
			var type = ReflectionHelper.TryGetType(model);
			if (type == null)
				throw new TypeLoadException("Could not load " + model + " type.");

			if (string.IsNullOrWhiteSpace(model))
				throw new ArgumentException("model must be provided to EntityManager.GetManager.", "model");

			_manager = EntityManager.GetManager(type, throwOnNonEntity);
			return _manager;
		}

		public static EntityManager GetManager(Type type, bool throwOnNonEntity = true)
		{
			if (type.GetInterface("IEntity") == null && throwOnNonEntity)
				throw new ArgumentException("Type must inherit from IEntity", "type");

			if (_manager != null && _manager.IsType(type))
				return _manager;

			_manager = new EntityManager(type);
			return _manager;
		}

		/// <summary>
		/// Deletes the entity with the id.
		/// </summary>
		/// <param name="id"></param>
		/// <param name="deleteType"></param>
		/// <returns></returns>
		public bool Delete(int id, DeleteTypes deleteType = DeleteTypes.Hard)
		{
			if (Get(id) == null)
				return false;

			Invoke("Delete", new object[] { id, deleteType });
			IEntity deleted = null;
			if (deleteType == DeleteTypes.Hard)
				deleted = Get(id);

			return (deleted == null);
		}

		/// <summary>
		/// Restores the entity with the id.
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public int Restore(int id)
		{
			Invoke("Restore", new object[] { id });
			return id;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="type"></param>
		/// <returns></returns>
		public bool IsType(Type type)
		{
			var isType = (type == _entityType);
			return isType;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="method"></param>
		/// <param name="args"></param>
		/// <returns></returns>
		public object Invoke(string method, params object[] args)
		{
			if (string.IsNullOrWhiteSpace(method))
				throw new ArgumentException("Method name not provided.");

			if (_service == null)
				_service = IoC.Resolve(_entityType);

			var parameterTypes = args.Select(a => a.GetType()).ToArray();
			var mi = _service.GetType().GetMethod(method, parameterTypes);
			if (mi == null)
				throw new ArgumentException("Method provided does not exist.", "method");

			object output = null;
			try
			{
				if (mi.ReturnType == typeof(void))
					mi.Invoke(_service, args);
				else
					output = mi.Invoke(_service, args);
			}
			catch (TargetInvocationException tiex)
			{
				throw tiex.InnerException;
			}

			return output;
		}

		#endregion

		#region IEntity methods

		/// <summary>
		/// Creates the entity
		/// </summary>
		public IEntity Create(IEntity entity)
		{
			Invoke("Create", new object[] { entity });
			return entity;
		}

		/// <summary>
		/// Updates the entity
		/// </summary>
		/// <param name="entity"></param>
		public IEntity Update(IEntity entity)
		{
			Invoke("Update", new object[] { entity });
			return entity;
		}

		/// <summary>
		/// Upserts the entity
		/// </summary>
		/// <param name="entity"></param>
		public IEntity Upsert(IEntity entity)
		{
			Invoke("Upsert", new object[] { entity });
			return entity;
		}

		/// <summary>
		/// Gets the entity with the id supplied.
		/// </summary>
		/// <param name="id"></param>
		/// <param name="inclusions"></param>
		/// <returns></returns>
		public IEntity Get(int id, params string[] inclusions)
		{
			return Invoke("Get", new object[] { id, inclusions }) as IEntity;
		}

		/// <summary>
		/// Gets the entity with the id supplied or a default IEntity derived instance.
		/// </summary>
		/// <param name="id"></param>
		/// <param name="inclusions"></param>
		/// <returns></returns>
		public IEntity GetOrNew(int id, params string[] inclusions)
		{
			var e = Invoke("Get", new object[] { id, inclusions }) as IEntity;
			return e ?? (IEntity)Activator.CreateInstance(_entityType);
		}

		/// <summary>
		/// Gets all the entities.
		/// </summary>
		/// <returns></returns>
		public IEnumerable<IEntity> GetAll(bool includeDeleted = false, params string[] inclusions)
		{
			return Invoke("GetAll", new object[] { includeDeleted, inclusions }) as IEnumerable<IEntity>;
		}

		/// <summary>
		/// Gets all the lookup values.
		/// </summary>
		/// <returns></returns>
		public IEnumerable<ILookup> GetLookup(bool includeDeleted = false, params string[] inclusions)
		{
			var entities = Invoke("GetAll", new object[] { includeDeleted, inclusions }) as IEnumerable<IEntity>;
			return entities.ToLookups<IEntity>("Name").ToArray();
		}

		#endregion

		#region Generic calls

		/// <summary>
		/// Creates the entity
		/// </summary>
		public TEntity Create<TEntity>(TEntity entity) where TEntity : class, IEntity, new()
		{
			Invoke("Create", new object[] { entity });
			return entity;
		}

		/// <summary>
		/// Updates the entity
		/// </summary>
		/// <param name="entity"></param>
		public TEntity Update<TEntity>(TEntity entity) where TEntity : class, IEntity, new()
		{
			Invoke("Update", new object[] { entity });
			return entity;
		}

		/// <summary>
		/// Upserts the entity
		/// </summary>
		/// <param name="entity"></param>
		public TEntity Upsert<TEntity>(TEntity entity) where TEntity : class, IEntity, new()
		{
			Invoke("Upsert", new object[] { entity });
			return entity;
		}

		/// <summary>
		/// Gets the entity with the id supplied.
		/// </summary>
		/// <param name="id"></param>
		/// <param name="inclusions"></param>
		/// <returns></returns>
		public TEntity Get<TEntity>(int id, params string[] inclusions) where TEntity : class, IEntity, new()
		{
			return Invoke("Get", new object[] { id, inclusions }) as TEntity;
		}

		/// <summary>
		/// Gets the entity with the id supplied or a default IEntity derived instance.
		/// </summary>
		/// <param name="id"></param>
		/// <param name="inclusions"></param>
		/// <returns></returns>
		public TEntity GetOrNew<TEntity>(int id, params string[] inclusions) where TEntity : class, IEntity, new()
		{
			var e = Invoke("Get", new object[] { id, inclusions }) as TEntity;
			return e ?? Activator.CreateInstance(_entityType) as TEntity;
		}

		/// <summary>
		/// Gets all the entities.
		/// </summary>
		/// <returns></returns>
		public IEnumerable<TEntity> GetAll<TEntity>(bool includeDeleted = false, params string[] inclusions) where TEntity : class, IEntity, new()
		{
			return Invoke("GetAll", new object[] { includeDeleted, inclusions }) as IEnumerable<TEntity>;
		}

		/// <summary>
		/// Determines whether the EntityManager is an instance of the generic type T.
		/// </summary>
		/// <typeparam name="T">The type to check against.</typeparam>
		/// <returns>True if the EntityManager is of type T, false otherwise.</returns>
		public bool IsInstance<T>()
		{
			var isInstance = (_entityType.GetInterface(typeof(T).Name) != null);
			return isInstance;
		}

		#endregion
	}
}