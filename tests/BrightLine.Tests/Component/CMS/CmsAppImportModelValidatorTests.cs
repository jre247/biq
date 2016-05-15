
using BrightLine.CMS.AppImport;
using NUnit.Framework;

namespace BrightLine.Tests.Component.CMS
{
    [TestFixture]
    public class CmsAppImportModelValidatorTests
    {
        [Test]
        public void Can_Pass_Validation()
        {
            var validator = new AppImportModelValidator();
        }
    }
}
