using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using Microsoft.Data.Tools.Schema.Sql.UnitTesting;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Data.Tools.Schema.Sql.UnitTesting.Configuration;

namespace BrightLine.Tests.OLAP
{
	[TestClass()]
	public class SqlDatabaseSetup
	{
		[AssemblyInitialize()]
		public static void InitializeAssembly(TestContext ctx)
		{
			//SqlDatabaseTestClass.TestService.DeployDatabaseProject();
			Environment.SetEnvironmentVariable("VisualStudioVersion", "12.0");
		}
	}
}
