using BrightLine.CMS;
using BrightLine.CMS.AppImport;
using NUnit.Framework;


namespace BrightLine.Tests.Component.CMS
{
    [TestFixture]
    public class CmsFixesTests
    {
        [Test(Description = "Defect IQ-236: Handling of list of texts ( separated by comma ',' ) does not preserve whitespace in each text")]
        public void Can_Parse_List_Of_Texts()
        {
            var text = " Marilyn\t Monroe , The US\n President , Walt's Brother\\, Roy , Elvis\r\n Presley ";

            var items = AppImporterHelper.ParseStringList(text, false);

            // Removed tab after US
            Assert.AreEqual(items[0], "Marilyn Monroe");

            // Removed newline in form of \n ( inserted in excel instead of \r\n )
            Assert.AreEqual(items[1], "The US President");

            // Escaped ',' as in \,
            Assert.AreEqual(items[2], "Walt's Brother, Roy");

            // Removed newline \r\n after Elvis 
            Assert.AreEqual(items[3], "Elvis Presley");
        }


        [Test(Description = "Defect IQ-236: Handling of list of texts ( separated by comma ',' ) does not preserve whitespace in each text")]
        public void Can_Parse_List_Of_Refs()
        {
            var text = "1, 2 ";
            var items = AppImporterHelper.ParseStringList(text, true);
            Assert.AreEqual(items[0], "1");
            Assert.AreEqual(items[1], "2");
        }


        [Test(Description = "Defect IQ-250: Ensure spreadhseet cannot be uploaded for incorrect app")]
        public void Can_Ensure_FileName_Is_Anything_If_No_Short_Display()
        {
            CmsRules.EnsureCorrectFileName(null, "the latest app file.xls");
            CmsRules.EnsureCorrectFileName(string.Empty, "the latest app file.xls");
        }


        [Test(Description = "Defect IQ-250: Ensure spreadhseet cannot be uploaded for incorrect app")]
        [ExpectedException]
        public void Can_Ensure_File_Name()
        {
            CmsRules.EnsureCorrectFileName("DisneyParks", "loreal_the latest app file.xls");
        }
    }
}
