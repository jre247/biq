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
	public class GetCampaignDate : SqlDatabaseTestClass
	{

		public GetCampaignDate()
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
			Microsoft.Data.Tools.Schema.Sql.UnitTesting.SqlDatabaseTestAction dbo_GetCampaignDateTest_TestAction;
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GetCampaignDate));
			Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.ScalarValueCondition campaignDateBegin;
			Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.ScalarValueCondition campaignEndDate;
			Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.ScalarValueCondition campaignDneBegin;
			Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.ScalarValueCondition campaignDneEnd;
			this.dbo_GetCampaignDateTestData = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.SqlDatabaseTestActions();
			dbo_GetCampaignDateTest_TestAction = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.SqlDatabaseTestAction();
			campaignDateBegin = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.ScalarValueCondition();
			campaignEndDate = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.ScalarValueCondition();
			campaignDneBegin = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.ScalarValueCondition();
			campaignDneEnd = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.ScalarValueCondition();
			// 
			// dbo_GetCampaignDateTest_TestAction
			// 
			dbo_GetCampaignDateTest_TestAction.Conditions.Add(campaignDateBegin);
			dbo_GetCampaignDateTest_TestAction.Conditions.Add(campaignEndDate);
			dbo_GetCampaignDateTest_TestAction.Conditions.Add(campaignDneBegin);
			dbo_GetCampaignDateTest_TestAction.Conditions.Add(campaignDneEnd);
			resources.ApplyResources(dbo_GetCampaignDateTest_TestAction, "dbo_GetCampaignDateTest_TestAction");
			// 
			// campaignDateBegin
			// 
			campaignDateBegin.ColumnNumber = 1;
			campaignDateBegin.Enabled = true;
			campaignDateBegin.ExpectedValue = "2015-04-13";
			campaignDateBegin.Name = "campaignDateBegin";
			campaignDateBegin.NullExpected = false;
			campaignDateBegin.ResultSet = 1;
			campaignDateBegin.RowNumber = 1;
			// 
			// campaignEndDate
			// 
			campaignEndDate.ColumnNumber = 1;
			campaignEndDate.Enabled = true;
			campaignEndDate.ExpectedValue = "2015-04-26";
			campaignEndDate.Name = "campaignEndDate";
			campaignEndDate.NullExpected = false;
			campaignEndDate.ResultSet = 2;
			campaignEndDate.RowNumber = 1;
			// 
			// campaignDneBegin
			// 
			campaignDneBegin.ColumnNumber = 1;
			campaignDneBegin.Enabled = true;
			campaignDneBegin.ExpectedValue = null;
			campaignDneBegin.Name = "campaignDneBegin";
			campaignDneBegin.NullExpected = true;
			campaignDneBegin.ResultSet = 3;
			campaignDneBegin.RowNumber = 1;
			// 
			// campaignDneEnd
			// 
			campaignDneEnd.ColumnNumber = 1;
			campaignDneEnd.Enabled = true;
			campaignDneEnd.ExpectedValue = null;
			campaignDneEnd.Name = "campaignDneEnd";
			campaignDneEnd.NullExpected = true;
			campaignDneEnd.ResultSet = 4;
			campaignDneEnd.RowNumber = 1;
			// 
			// dbo_GetCampaignDateTestData
			// 
			this.dbo_GetCampaignDateTestData.PosttestAction = null;
			this.dbo_GetCampaignDateTestData.PretestAction = null;
			this.dbo_GetCampaignDateTestData.TestAction = dbo_GetCampaignDateTest_TestAction;
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
		public void dbo_GetCampaignDateTest()
		{
			SqlDatabaseTestActions testActions = this.dbo_GetCampaignDateTestData;
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
		private SqlDatabaseTestActions dbo_GetCampaignDateTestData;
	}
}
