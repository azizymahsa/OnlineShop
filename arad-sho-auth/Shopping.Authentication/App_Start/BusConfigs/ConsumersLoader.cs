using System;
using Castle.Windsor;
using Shopping.Authentication.Installer;

namespace Shopping.Authentication.BusConfigs
{
    public class ConsumersLoader
    {
        public static IWindsorContainer Load(IWindsorContainer container)
        {
            if (container == null)
                throw new ArgumentNullException("container", "Container cannot be null...");

            //todo 
            container.Install(new ApiServiceInstaller());

            return container;
        }
    }
}