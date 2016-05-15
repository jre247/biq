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
	public class GetAdName : SqlDatabaseTestClass
	{

		public GetAdName()
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
			Microsoft.Data.Tools.Schema.Sql.UnitTesting.SqlDatabaseTestAction dbo_GetAdNameTest_TestAction;
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GetAdName));
			Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.ScalarValueCondition adName;
			Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.ScalarValueCondition adNameWithAdTypeName;
			Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.ScalarValueCondition adTypeNamewithNoAdName;
			Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.ScalarValueCondition adTypeAndAdTypeGroup;
			this.dbo_GetAdNameTestData = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.SqlDatabaseTestActions();
			dbo_GetAdNameTest_TestAction = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.SqlDatabaseTestAction();
			adName = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.ScalarValueCondition();
			adNameWithAdTypeName = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.ScalarValueCondition();
			adTypeNamewithNoAdName = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.ScalarValueCondition();
			adTypeAndAdTypeGroup = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.ScalarValueCondition();
			// 
			// dbo_GetAdNameTest_TestAction
			// 
			dbo_GetAdNameTest_TestAction.Conditions.Add(adName);
			dbo_GetAdNameTest_TestAction.Conditions.Add(adNameWithAdTypeName);
			dbo_GetAdNameTest_TestAction.Conditions.Add(adTypeNamewithNoAdName);
			dbo_GetAdNameTest_TestAction.Conditions.Add(adTypeAndAdTypeGroup);
			resources.ApplyResources(dbo_GetAdNameTest_TestAction, "dbo_GetAdNameTest_TestAction");
			// 
			// adName
			// 
			adName.ColumnNumber = 1;
			adName.Enabled = true;
			adName.ExpectedValue = "Ad Name";
			adName.Name = "adName";
			adName.NullExpected = false;
			adName.ResultSet = 1;
			adName.RowNumber = 1;
			// 
			// adNameWithAdTypeName
			// 
			adNameWithAdTypeName.ColumnNumber = 1;
			adNameWithAdTypeName.Enabled = true;
			adNameWithAdTypeName.ExpectedValue = "Ad Name";
			adNameWithAdTypeName.Name = "adNameWithAdTypeName";
			adNameWithAdTypeName.NullExpected = false;
			adNameWithAdTypeName.ResultSet = 2;
			adNameWithAdTypeName.RowNumber = 1;
			// 
			// adTypeNamewithNoAdName
			// 
			adTypeNamewithNoAdName.ColumnNumber = 1;
			adTypeNamewithNoAdName.Enabled = true;
			adTypeNamewithNoAdName.ExpectedValue = "Ad Type Name";
			adTypeNamewithNoAdName.Name = "adTypeNamewithNoAdName";
			adTypeNamewithNoAdName.NullExpected = false;
			adTypeNamewithNoAdName.ResultSet = 3;
			adTypeNamewithNoAdName.RowNumber = 1;
			// 
			// adTypeAndAdTypeGroup
			// 
			adTypeAndAdTypeGroup.ColumnNumber = 1;
			adTypeAndAdTypeGroup.Enabled = true;
			adTypeAndAdTypeGroup.ExpectedValue = "Ad Type Group - Ad Type Name";
			adTypeAndAdTypeGroup.Name = "adTypeAndAdTypeGroup";
			adTypeAndAdTypeGroup.NullExpected = false;
			adTypeAndAdTypeGroup.ResultSet = 4;
			adTypeAndAdTypeGroup.RowNumber = 1;
			// 
			// dbo_GetAdNameTestData
			// 
			this.dbo_GetAdNameTestData.PosttestAction = null;
			this.dbo_GetAdNameTestData.PretestAction = null;
			this.dbo_GetAdNameTestData.TestAction = dbo_GetAdNameTest_TestAction;
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
		public void dbo_GetAdNameTest()
		{
			SqlDatabaseTestActions testActions = this.dbo_GetAdNameTestData;
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
		private SqlDatabaseTestActions dbo_GetAdNameTestData;
	}
}
