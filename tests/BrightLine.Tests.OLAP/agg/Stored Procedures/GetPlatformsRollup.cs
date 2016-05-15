using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;
using Microsoft.Data.Tools.Schema.Sql.UnitTesting;
using Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BrightLine.Tests.OLAP
{
	[TestClass()]
	public class GetPlatformsRollup : SqlDatabaseTestClass
	{

		public GetPlatformsRollup()
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
			Microsoft.Data.Tools.Schema.Sql.UnitTesting.SqlDatabaseTestAction agg_GetPlatformsRollupTest_TestAction;
			Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.ExpectedSchemaCondition expectedSchemaCondition1;
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GetPlatformsRollup));
			Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.ChecksumCondition checksumCondition1;
			this.agg_GetPlatformsRollupTestData = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.SqlDatabaseTestActions();
			agg_GetPlatformsRollupTest_TestAction = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.SqlDatabaseTestAction();
			expectedSchemaCondition1 = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.ExpectedSchemaCondition();
			checksumCondition1 = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.ChecksumCondition();
			// 
			// agg_GetPlatformsRollupTest_TestAction
			// 
			agg_GetPlatformsRollupTest_TestAction.Conditions.Add(expectedSchemaCondition1);
			agg_GetPlatformsRollupTest_TestAction.Conditions.Add(checksumCondition1);
			resources.ApplyResources(agg_GetPlatformsRollupTest_TestAction, "agg_GetPlatformsRollupTest_TestAction");
			// 
			// expectedSchemaCondition1
			// 
			expectedSchemaCondition1.Enabled = true;
			expectedSchemaCondition1.Name = "expectedSchemaCondition1";
			resources.ApplyResources(expectedSchemaCondition1, "expectedSchemaCondition1");
			expectedSchemaCondition1.Verbose = false;
			// 
			// checksumCondition1
			// 
			checksumCondition1.Checksum = "605639065";
			checksumCondition1.Enabled = true;
			checksumCondition1.Name = "checksumCondition1";
			// 
			// agg_GetPlatformsRollupTestData
			// 
			this.agg_GetPlatformsRollupTestData.PosttestAction = null;
			this.agg_GetPlatformsRollupTestData.PretestAction = null;
			this.agg_GetPlatformsRollupTestData.TestAction = agg_GetPlatformsRollupTest_TestAction;
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
		public void agg_GetPlatformsRollupTest()
		{
			SqlDatabaseTestActions testActions = this.agg_GetPlatformsRollupTestData;
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
		private SqlDatabaseTestActions agg_GetPlatformsRollupTestData;
	}
}
