using Castle.Windsor;
using Shopping.Authentication.Installer;

namespace Shopping.Authentication
{
    public static class Bootstrapper
    {
        public static IWindsorContainer WindsorContainer { get; set; }
        static Bootstrapper()
        {
            WindsorContainer = new WindsorContainer();
        }
        public static void Run()
        {
            WindsorContainer.Install(new ApiServiceInstaller());
        }
        public static void ShutDown()
        {
            WindsorContainer?.Dispose();
        }
    }
}