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
	public class ConvertDate : SqlDatabaseTestClass
	{

		public ConvertDate()
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
			Microsoft.Data.Tools.Schema.Sql.UnitTesting.SqlDatabaseTestAction dbo_ConvertDateTest_TestAction;
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ConvertDate));
			Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.ScalarValueCondition correctDate20150505;
			Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.ScalarValueCondition correctDate2015050500;
			Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.ScalarValueCondition correctDate201518;
			this.dbo_ConvertDateTestData = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.SqlDatabaseTestActions();
			dbo_ConvertDateTest_TestAction = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.SqlDatabaseTestAction();
			correctDate20150505 = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.ScalarValueCondition();
			correctDate2015050500 = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.ScalarValueCondition();
			correctDate201518 = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.ScalarValueCondition();
			// 
			// dbo_ConvertDateTest_TestAction
			// 
			dbo_ConvertDateTest_TestAction.Conditions.Add(correctDate20150505);
			dbo_ConvertDateTest_TestAction.Conditions.Add(correctDate2015050500);
			dbo_ConvertDateTest_TestAction.Conditions.Add(correctDate201518);
			resources.ApplyResources(dbo_ConvertDateTest_TestAction, "dbo_ConvertDateTest_TestAction");
			// 
			// correctDate20150505
			// 
			correctDate20150505.ColumnNumber = 1;
			correctDate20150505.Enabled = true;
			correctDate20150505.ExpectedValue = "2015-05-05";
			correctDate20150505.Name = "correctDate20150505";
			correctDate20150505.NullExpected = false;
			correctDate20150505.ResultSet = 1;
			correctDate20150505.RowNumber = 1;
			// 
			// correctDate2015050500
			// 
			correctDate2015050500.ColumnNumber = 1;
			correctDate2015050500.Enabled = true;
			correctDate2015050500.ExpectedValue = "2015-05-05";
			correctDate2015050500.Name = "correctDate2015050500";
			correctDate2015050500.NullExpected = false;
			correctDate2015050500.ResultSet = 2;
			correctDate2015050500.RowNumber = 1;
			// 
			// dbo_ConvertDateTestData
			// 
			this.dbo_ConvertDateTestData.PosttestAction = null;
			this.dbo_ConvertDateTestData.PretestAction = null;
			this.dbo_ConvertDateTestData.TestAction = dbo_ConvertDateTest_TestAction;
			// 
			// correctDate201518
			// 
			correctDate201518.ColumnNumber = 1;
			correctDate201518.Enabled = true;
			correctDate201518.ExpectedValue = "2015-05-02";
			correctDate201518.Name = "correctDate201518";
			correctDate201518.NullExpected = false;
			correctDate201518.ResultSet = 3;
			correctDate201518.RowNumber = 1;
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
		public void dbo_ConvertDateTest()
		{
			SqlDatabaseTestActions testActions = this.dbo_ConvertDateTestData;
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
		private SqlDatabaseTestActions dbo_ConvertDateTestData;
	}
}
