using BrightLine.Common.Framework;

namespace BrightLine.Tests.Common
{
    public class IocSetup
    {
        public static void Setup(IocRegistration container)
        {
            IoC.Container = container.GetContainer() as SimpleInjector.Container;
        }
    }
}
