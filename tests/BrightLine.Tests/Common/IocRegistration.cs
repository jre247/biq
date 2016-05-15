using System;
using System.Collections.Generic;

namespace BrightLine.Tests.Common
{
	public class IocRegistration
	{
		private SimpleInjector.Container _container = new SimpleInjector.Container();
		private List<Type> _registrations = new List<Type>();
		/// <summary>
		/// Gets the underlying ioc container.
		/// </summary>
		/// <returns></returns>
		public object GetContainer()
		{
			return _container;
		}

		/// <summary>
		/// Register the service and implementation
		/// </summary>
		/// <typeparam name="TService"></typeparam>
		/// <typeparam name="TImplementation"></typeparam>
		public void Register<TService>() where TService : class
		{
			_container.Register<TService>();
			_registrations.Add(typeof(TService));
		}

		/// <summary>
		/// Register the service and implementation
		/// </summary>
		/// <typeparam name="TService"></typeparam>
		/// <typeparam name="TImplementation"></typeparam>
		public void Register<TService, TImplementation>(bool singleton = true)
			where TService : class
			where TImplementation : class, TService
		{
			var lifeStyle = singleton
						  ? SimpleInjector.Lifestyle.Singleton
						  : SimpleInjector.Lifestyle.Transient;
			_container.Register<TService, TImplementation>(lifeStyle);
			_registrations.Add(typeof(TService));
		}

		/// <summary>
		/// Register the service and implementation
		/// </summary>
		/// <typeparam name="TService"></typeparam>
		public void Register<TService>(Func<TService> instanceCreator) where TService : class
		{
			_container.Register<TService>(instanceCreator);
			_registrations.Add(typeof(TService));
		}

		/// <summary>
		/// Register the service and implementation
		/// </summary>
		/// <typeparam name="TService"></typeparam>
		public void RegisterSingleton<TService>(Func<TService> instanceCreator) where TService : class
		{
			_container.Register<TService>(instanceCreator, SimpleInjector.Lifestyle.Singleton);
			_registrations.Add(typeof(TService));
		}

		//public HttpContext CreateHttpContext(User user)
		//{
		//	var context = new HttpContext(new HttpRequest("", "http://tempuri.org", ""), new HttpResponse(new StringWriter()));
		//	// User is logged in
		//	string username = "";
		//	string[] roles = new string[0];
		//	if (user == null)
		//		return context;

		//	username = user.Email;
		//	roles = user.Roles.Select(r => r.Name).ToArray();
		//	var ticket = new FormsAuthenticationTicket(username, false, 20);
		//	var identity = new CustomIdentity(ticket);
		//	var principal = new CustomPrincipal(identity);
		//	context.User = principal;
		//	return context;
		//}

		public bool HasRegistered<T>()
		{
			var exists = _registrations.Contains(typeof(T));
			return exists;
		}
	}
}
