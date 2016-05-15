using BrightLine.Common.Utility.Spreadsheets;
using BrightLine.Tests.Common;
using NUnit.Framework;
using System;


namespace BrightLine.Tests.Unit.Common.Spreadsheets
{
	[TestFixture]
	public class SpreadsheetUnitTests
	{
		private ISpreadsheetReader GetReader(string currentSheet)
		{
            //var filepath = @"C:\Code\Brightline\tests\BrightLine.Tests\_Resources\_SampleFiles\ExcelLoadTests.xlsx";
		    var bytes = ResourceLoader.ReadBytes(@"Excel.ExcelLoadTests.xlsx");
            var reader = SpreadsheetHelper.GetReader(bytes);
            reader.SetCurrentSheet(currentSheet);
		    return reader;
		}


        [Test]
		public void CanLoadColumnOfNumbers()
		{
            var reader = GetReader("ColumnTests");
		    var data = reader.ReadColumnOfNumbers(2, 1, 24, true);

		    Assert.AreEqual(data.Count, 24);
            for (var ndx = 0; ndx < data.Count; ndx++)
            {
                Assert.AreEqual(data[ndx], Convert.ToDouble(ndx + 1));
            }
		}


        [Test]
        public void CanLoadColumnOfStrings()
        {
            var reader = GetReader("ColumnTests");
            var data = reader.ReadColumnOfStrings(2, 0, 24, true);

            Assert.AreEqual(data.Count, 24);
            for (var ndx = 0; ndx < data.Count; ndx++)
            {
                Assert.AreEqual(data[ndx], "Text Value " + (ndx + 1).ToString());
            }
        }


        [Test]
        public void CanLoadColumnOfBoolsTrue()
        {
            var reader = GetReader("ColumnTests");
            var data = reader.ReadColumnOfBools(2, 2, 24, true);

            Assert.AreEqual(data.Count, 24);
            for (var ndx = 0; ndx < data.Count; ndx++)
            {
                Assert.AreEqual(data[ndx], true);
            }
        }


        [Test]
        public void CanLoadColumnOfBoolsFalse()
        {
            var reader = GetReader("ColumnTests");
            var data = reader.ReadColumnOfBools(2, 3, 24, true);

            Assert.AreEqual(data.Count, 24);
            for (var ndx = 0; ndx < data.Count; ndx++)
            {
                Assert.AreEqual(data[ndx], false);
            }
        }


        [Test]
        public void CanLoadRowOfNumbers()
        {
            var reader = GetReader("RowTests");
            var data = reader.ReadRowOfNumbers(3, 0, 5, true, false);

            Assert.AreEqual(data.Count, 5);
            for (var ndx = 0; ndx < data.Count; ndx++)
            {
                Assert.AreEqual(data[ndx], Convert.ToDouble(ndx + 1));
            }
        }


        [Test]
        public void CanLoadRowOfStrings()
        {
            var reader = GetReader("RowTests");
            var data = reader.ReadRowOfStrings(2, 0, 5, true, false);

            Assert.AreEqual(data.Count, 5);
            for (var ndx = 0; ndx < data.Count; ndx++)
            {
                Assert.AreEqual(data[ndx], "Text Value " + (ndx + 1).ToString());
            }
        }


        [Test]
        public void CanLoadRowOfBoolsTrue()
        {
            var reader = GetReader("RowTests");
            var data = reader.ReadRowOfBools(4, 0, 5, true, false);

            Assert.AreEqual(data.Count, 5);
            for (var ndx = 0; ndx < data.Count; ndx++)
            {
                Assert.AreEqual(data[ndx], true);
            }
        }


        [Test]
        public void CanLoadRowOfBoolsFalse()
        {
            var reader = GetReader("RowTests");
            var data = reader.ReadRowOfBools(5, 0, 5, true, false);

            Assert.AreEqual(data.Count, 5);
            for (var ndx = 0; ndx < data.Count; ndx++)
            {
                Assert.AreEqual(data[ndx], false);
            }
        }
	}
}
