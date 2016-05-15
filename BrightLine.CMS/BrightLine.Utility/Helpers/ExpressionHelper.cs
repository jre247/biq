using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Linq.Expressions;
using BrightLine.Core;
using BrightLine.Common.Framework;
using BrightLine.Common.Utility;


namespace BrightLine.Utility.Helpers
{
	public class ExpressionHelper
	{
		/// <summary>
		/// Get the property name from the expression.
		/// e.g. GetPropertyName<Person>( p => p.FirstName);
		/// </summary>
		/// <typeparam name="T">Type of item.</typeparam>
		/// <param name="exp">Expression.</param>
		/// <returns>Property name.</returns>
		public static string GetPropertyName<T>(Expression<Func<T, object>> exp)
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

			var info = memberExpression.Member as PropertyInfo;
			return info.Name;
		}


        /// <summary>
        /// Get the property name from the expression.
        /// e.g. GetPropertyName<Person>( p => p.FirstName);
        /// </summary>
        /// <typeparam name="T">Type of item.</typeparam>
        /// <param name="exp">Expression.</param>
        /// <returns>Property name.</returns>
        public static string GetPropertyAccessPath<T>(Expression<Func<T, object>> exp)
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
                var firstProp = ((MemberExpression) memberExpression.Expression).Member.Name;
                name += firstProp + ".";
            }
            var info = memberExpression.Member as PropertyInfo;
            name += info.Name;
            return name;
        }




		/// <summary>
		/// Get the property name from the expression.
		/// e.g. GetPropertyName<Person>( p => p.FirstName);
		/// </summary>
		/// <typeparam name="T">Type of item.</typeparam>
		/// <param name="exp">Expression.</param>
		/// <returns>Property name.</returns>
		public static PropertyInfo GetPropertyInfo<T>(Expression<Func<T, object>> exp)
		{
			MemberExpression memberExpression = null;

			// Get memberexpression.
			if (exp.Body is MemberExpression)
				memberExpression = exp.Body as MemberExpression;

			else if (exp.Body is UnaryExpression)
			{
				var unaryExpression = exp.Body as UnaryExpression;
				if (unaryExpression != null && unaryExpression.Operand is MemberExpression)
					memberExpression = unaryExpression.Operand as MemberExpression;

				if (memberExpression != null && memberExpression.Expression is MemberExpression)
					memberExpression = memberExpression.Expression as MemberExpression;
			}
			if (memberExpression == null)
				throw new InvalidOperationException("Not a member access.");

			var info = memberExpression.Member as PropertyInfo;
			return info;
		}


		/// <summary>
		/// Gets a list of id to entity pairs.
		/// </summary>
		public static IDictionary<T, TEntity> KeyToEntityiesBy<T, TEntity>(Expression<Func<TEntity, object>> expr)
			where TEntity : EntityBase, IEntity, new()
		{
			// Example:
			// var pairs = GetKeyValues<ResourceType>( res => res.Name );
			// returns :
			//	1 => john
			//  2 => jane

			var svc = IoC.Container.GetInstance<ICrudService<TEntity>>();
			var all = svc.GetAll();
			var map = new Dictionary<T, TEntity>();

			// 1. get the property name from the expression.
			var propName = ExpressionHelper.GetPropertyName<TEntity>(expr);

			// 2. get the property info 
			var propInfo = typeof(TEntity).GetProperty(propName,
														System.Reflection.BindingFlags.Instance |
														System.Reflection.BindingFlags.Public);

			// 3. Now get all ids, name
			foreach (var item in all)
			{
				var key = (T)ReflectionHelper.TryGetValue(item, propInfo);
				var val = ReflectionHelper.TryGetStringValue(item, propInfo);
				map[key] = item;
			}
			return map;
		}
	}
}
