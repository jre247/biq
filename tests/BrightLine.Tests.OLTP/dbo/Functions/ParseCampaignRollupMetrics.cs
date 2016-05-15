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
	public class ParseCampaignRollupMetrics : SqlDatabaseTestClass
	{

		public ParseCampaignRollupMetrics()
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
			Microsoft.Data.Tools.Schema.Sql.UnitTesting.SqlDatabaseTestAction dbo_ParseCampaignRollupMetricsTest_TestAction;
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ParseCampaignRollupMetrics));
			this.dbo_ParseCampaignRollupMetricsTestData = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.SqlDatabaseTestActions();
			dbo_ParseCampaignRollupMetricsTest_TestAction = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.SqlDatabaseTestAction();
			// 
			// dbo_ParseCampaignRollupMetricsTest_TestAction
			// 
			resources.ApplyResources(dbo_ParseCampaignRollupMetricsTest_TestAction, "dbo_ParseCampaignRollupMetricsTest_TestAction");
			// 
			// dbo_ParseCampaignRollupMetricsTestData
			// 
			this.dbo_ParseCampaignRollupMetricsTestData.PosttestAction = null;
			this.dbo_ParseCampaignRollupMetricsTestData.PretestAction = null;
			this.dbo_ParseCampaignRollupMetricsTestData.TestAction = dbo_ParseCampaignRollupMetricsTest_TestAction;
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
		public void dbo_ParseCampaignRollupMetricsTest()
		{
			SqlDatabaseTestActions testActions = this.dbo_ParseCampaignRollupMetricsTestData;
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
		private SqlDatabaseTestActions dbo_ParseCampaignRollupMetricsTestData;
	}
}
