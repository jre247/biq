using BrightLine.Common.Utility.Spreadsheets;
using BrightLine.Tests.Common;
using NUnit.Framework;
using System;


namespace BrightLine.Tests.Unit.Common.Spreadsheets
{
	[TestFixture]
	public class SpreadsheetEntityTests
	{
		private ISpreadsheetReader GetReader(string currentSheet)
		{
            //var filepath = @"C:\Code\Brightline\tests\BrightLine.Tests\_Resources\_SampleFiles\ExcelLoadTests.xlsx";
		    var bytes = ResourceLoader.ReadBytes(@"Excel.ExcelLoadTests.xlsx");
            var reader = SpreadsheetHelper.GetReader(bytes);
            reader.SetCurrentSheet(currentSheet);
		    return reader;
		}


        private EntityMapper<SampleEntityForSpreadsheet> GetMapper()
        {
            var mapper = new EntityMapper<SampleEntityForSpreadsheet>();
            mapper.AddField("Name", "Name", typeof(string), true, "", 5, 20);
            mapper.AddField("Total", "Total", typeof(double), true, "", 1, 1);
            mapper.AddField("Is Active", "IsActive", typeof(bool), true, "", 1, 1);
            mapper.AddField("Is Ready", "IsReady", typeof(bool), true, "", 1, 1);
            mapper.AddField("Start date", "StartDate", typeof(DateTime), true, "", 1, 1);
            mapper.EntityType = typeof(SampleEntityForSpreadsheet);
            mapper.RegisterProperties();
            return mapper;
        }
        

        [Test]
        public void CanLoadRowOfEntities()
        {
            var reader = GetReader("EntityTests");

            var mapper = GetMapper();
            mapper.Reader = reader;

            var entities = mapper.MapEntitiesAcross("EntityTests", 2, 0, 5, 10);

            Assert.AreEqual(entities.Count, 10);

            var startDate = new DateTime(2014, 1, 1);
            for (var ndx = 0; ndx < entities.Count; ndx++)
            {
                var entity = entities[ndx];

                Assert.AreEqual(entity.Name, "Text Value " + (ndx + 1).ToString());
                Assert.AreEqual(entity.Total, Convert.ToDouble(ndx +1 ));
                Assert.AreEqual(entity.IsActive, true);
                Assert.AreEqual(entity.IsReady, false);
                Assert.AreEqual(entity.StartDate, startDate.Date);

                startDate = startDate.Date.AddDays(1);
            }
        }


        [Test]
        public void CanLoadColumnOfEntities()
        {
            var reader = GetReader("EntityTests2");

            var mapper = GetMapper();
            mapper.Reader = reader;

            var entities = mapper.MapEntitiesDown("EntityTests2", 2, 1, 4, 5);

            Assert.AreEqual(entities.Count, 4);

            var startDate = new DateTime(2014, 1, 1);
            for (var ndx = 0; ndx < entities.Count; ndx++)
            {
                var entity = entities[ndx];

                Assert.AreEqual(entity.Name, "Text " + (ndx + 1).ToString());
                Assert.AreEqual(entity.Total, Convert.ToDouble(ndx + 1));
                Assert.AreEqual(entity.IsActive, true);
                Assert.AreEqual(entity.IsReady, false);
                Assert.AreEqual(entity.StartDate, startDate.Date);

                startDate = startDate.Date.AddDays(1);
            }
        }


	    //[Test]
	    //public void CanLoadTableOfEntities()
	    //{
        //    var mapper = new EntityMapper();
	    //    mapper.AddField<string>("Label", true, "");
        //    mapper.AddField<string>("Name", true, "");
        //    mapper.AddField<string>("TypeName", true, "");
        //    mapper.AddField<bool>("Required", false, "false");
        //    mapper.AddField<int>("MaxLength", false, "-1");
        //    mapper.AddField<int>("MinLength", false, "-1");
        //    mapper.EntityType = typeof(EntityField);
	    //}


	    class SampleEntityForSpreadsheet
        {
            public string Name { get; set; }
            public double Total { get; set; }
            public bool IsActive { get; set; }
            public bool IsReady { get; set; }
            public DateTime StartDate { get; set; }
        }
	}
}
