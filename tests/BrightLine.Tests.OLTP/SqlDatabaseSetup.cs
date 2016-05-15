using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using Microsoft.Data.Tools.Schema.Sql.UnitTesting;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BrightLine.Tests.OLTP
{
	[TestClass()]
	public class SqlDatabaseSetup
	{

		[AssemblyInitialize()]
		public static void InitializeAssembly(TestContext ctx)
		{
			// SqlDatabaseTestClass.TestService.DeployDatabaseProject();
			Environment.SetEnvironmentVariable("VisualStudioVersion", "12.0"); 
		}
	}
}
