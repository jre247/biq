using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

using BrightLine.Tests.Common;


namespace BrightLine.Tests.Unit
{
    [TestFixture]
    public class _SampleUnitTests
    {
        [Test]
        public void Can_Get_Resource_File()
        {
            // NOTE: all resource files should be stored in "_Resources" directory.
            //       In example below: "_SampleFiles.Sample.text", _SampleFiles is a 
            //       directory under the "_Resources" folder. and "." is used as a path separator.
            var content = ResourceLoader.Get("_SampleFiles.Sample.txt");
            Assert.AreEqual(content, "some sample data");
        }


        [Test, TestCaseSource("AddCases")]
        public void Can_Use_Inputs_In_Unit_Test(int a, int b, int result)
        {
            Assert.AreEqual(result, a + b);
        }


        // TEST DATA USED FOR UNIT-TESTS
        static object[] AddCases = 
        {
            new object[] {1, 2, 3},

            new object[] {2, 3, 5},
            new object[] {3, 4, 7}
        }; 
		
		
    }
}
