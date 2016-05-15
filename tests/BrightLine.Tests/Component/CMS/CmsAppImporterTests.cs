using BrightLine.CMS.AppImport;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace BrightLine.Tests.Component.CMS
{
	[TestFixture]
	public class CmsAppImporterTests
	{
		[Test]
		public void Can_Import_From_Excel()
		{
			//var path = @"C:\Code\Brightline\proto\appmodels_loreal_v8.xlsx";
			//var exportPath =  @"C:\Code\Brightline\proto\CMS_Export\V8_test\";
			//var importer = new AppImporter();
			//importer.ImportFromPath(path, "loreal");
			//importer.SaveToFolder(exportPath, true);
		}


        [Test]
        public void Can_Massage_Column_Names()
        {
            var testcases = new List<Tuple<string, string>>()
                {
                    new Tuple<string, string>("ACTIVATION NAME (SPOTLIGHT ONLY)", "activationName"),
                    new Tuple<string, string>("VIDEO THUMBNAIL FILE NAME", "videoThumbnailFileName"),
                    new Tuple<string, string>("IN-HOUSE?", "in_house"),
                    new Tuple<string, string>("TRT", "trt")
                };
            foreach (var test in testcases)
            {
                var result = AppImporterHelper.MassageName(test.Item1);
                Assert.AreEqual(result, test.Item2);
            }
        }
    }
}
