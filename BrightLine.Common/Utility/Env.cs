using BrightLine.Common.Utility.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrightLine.Common.Utility
{
	/// <summary>
	/// A specific environment such as "Dev", "Uat" "Prod" etc.
	/// </summary>
	public class EnvItem
	{
		public readonly string Name;
		public readonly EnvironmentType EnvType;


		public EnvItem(string name, EnvironmentType type)
		{
			Name = name;
			EnvType = type;
		}
	}



	/// <summary>
	/// Class to represent the currently selected environment.
	/// This is just a provider pattern.
	/// </summary>
	public class Env
	{
		private static EnvItem _envInstance = new EnvItem("local", EnvironmentType.LOCAL);


		#region Public Properties
		/// <summary>
		/// Name of current envionment selected during initialization. e.g. "london.prod" or "prod | uat | qa | dev".
		/// </summary>
		public static string Name
		{
			get { return _envInstance.Name; }
		}


		/// <summary>
		/// The environment type (prod, qa, etc ) of current selected environment
		/// </summary>
		public static EnvironmentType EnvType
		{
			get { return _envInstance.EnvType; }
		}


		/// <summary>
		/// Is current env type production.
		/// </summary>
		public static bool IsProd { get { return _envInstance.EnvType == EnvironmentType.PRO; } }


		/// <summary>
		/// Is current env type uat.
		/// </summary>
		public static bool IsUat { get { return _envInstance.EnvType == EnvironmentType.UAT; } }

		
		/// <summary>
		/// Is current env type Continuous integration
		/// </summary>
		public static bool IsCI { get { return _envInstance.EnvType == EnvironmentType.CI; } }


		/// <summary>
		/// Is current env type development.
		/// </summary>
		public static bool IsDev { get { return _envInstance.EnvType == EnvironmentType.DEV; } }


		/// <summary>
		/// Is current env type development.
		/// </summary>
		public static bool IsLocal { get { return _envInstance.EnvType == EnvironmentType.LOCAL; } }
		#endregion


		/// <summary>
		/// Initialize with current environment.
		/// </summary>
		/// <param name="environment"></param>
		private static void Init(EnvItem envItem)
		{
			_envInstance = envItem;
		}


		/// <summary>
		/// This is called from Global.asax via
		/// </summary>
		private static void LoadFromConfig()
		{
			var env = System.Configuration.ConfigurationManager.AppSettings["Environment"];

			switch (env)
			{
				case "LOCAL":
					Init(new EnvItem("local", EnvironmentType.LOCAL));
					break;
				case "DEV":
					Init(new EnvItem("dev",  EnvironmentType.DEV));
					break;
				case "CI":
					Init(new EnvItem("ci",   EnvironmentType.CI));
					break;
				case "UAT":
					Init(new EnvItem("uat",  EnvironmentType.UAT));
					break;
				case "PRO":
					Init(new EnvItem("pro", EnvironmentType.PRO));
					break;
				default:
					throw new ApplicationException(string.Format("Invalid Environment: {0}", env));
			}
		}
	}
}
