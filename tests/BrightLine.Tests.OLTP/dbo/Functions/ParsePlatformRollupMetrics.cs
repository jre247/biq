using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;
using Microsoft.Data.Tools.Schema.Sql.UnitTesting;
using Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BrightLine.Tests.OLTP
{
	[TestClass()]
	public class ParsePlatformRollupMetrics : SqlDatabaseTestClass
	{

		public ParsePlatformRollupMetrics()
		{
			InitializeComponent();
		}

		[TestInitialize()]
		public void TestInitialize()
		{
			base.InitializeTest();
		}
		[TestCleanup()]
		public void TestCleanup()
		{
			base.CleanupTest();
		}

		#region Designer support code

		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			Microsoft.Data.Tools.Schema.Sql.UnitTesting.SqlDatabaseTestAction dbo_ParsePlatformRollupMetricsTest_TestAction;
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ParsePlatformRollupMetrics));
			this.dbo_ParsePlatformRollupMetricsTestData = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.SqlDatabaseTestActions();
			dbo_ParsePlatformRollupMetricsTest_TestAction = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.SqlDatabaseTestAction();
			// 
			// dbo_ParsePlatformRollupMetricsTest_TestAction
			// 
			resources.ApplyResources(dbo_ParsePlatformRollupMetricsTest_TestAction, "dbo_ParsePlatformRollupMetricsTest_TestAction");
			// 
			// dbo_ParsePlatformRollupMetricsTestData
			// 
			this.dbo_ParsePlatformRollupMetricsTestData.PosttestAction = null;
			this.dbo_ParsePlatformRollupMetricsTestData.PretestAction = null;
			this.dbo_ParsePlatformRollupMetricsTestData.TestAction = dbo_ParsePlatformRollupMetricsTest_TestAction;
		}

		#endregion


		#region Additional test attributes
		//
		// You can use the following additional attributes as you write your tests:
		//
		// Use ClassInitialize to run code before running the first test in the class
		// [ClassInitialize()]
		// public static void MyClassInitialize(TestContext testContext) { }
		//
		// Use ClassCleanup to run code after all tests in a class have run
		// [ClassCleanup()]
		// public static void MyClassCleanup() { }
		//
		#endregion


		[TestMethod()]
		public void dbo_ParsePlatformRollupMetricsTest()
		{
			SqlDatabaseTestActions testActions = this.dbo_ParsePlatformRollupMetricsTestData;
			// Execute the pre-test script
			// 
			System.Diagnostics.Trace.WriteLineIf((testActions.PretestAction != null), "Executing pre-test script...");
			SqlExecutionResult[] pretestResults = TestService.Execute(this.PrivilegedContext, this.PrivilegedContext, testActions.PretestAction);
			try
			{
				// Execute the test script
				// 
				System.Diagnostics.Trace.WriteLineIf((testActions.TestAction != null), "Executing test script...");
				SqlExecutionResult[] testResults = TestService.Execute(this.ExecutionContext, this.PrivilegedContext, testActions.TestAction);
			}
			finally
			{
				// Execute the post-test script
				// 
				System.Diagnostics.Trace.WriteLineIf((testActions.PosttestAction != null), "Executing post-test script...");
				SqlExecutionResult[] posttestResults = TestService.Execute(this.PrivilegedContext, this.PrivilegedContext, testActions.PosttestAction);
			}
		}
		private SqlDatabaseTestActions dbo_ParsePlatformRollupMetricsTestData;
	}
}
