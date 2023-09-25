using System.Web.Http;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using MassTransit;
using Shopping.Authentication.Repository.DbContexts;

namespace Shopping.Authentication.Installer
{
    public class ApiServiceInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Classes.FromThisAssembly().BasedOn<ApiController>().LifestyleTransient());
            container.Register(Classes.FromThisAssembly().Pick().WithServiceAllInterfaces().LifestyleTransient());
            //container.Register(Classes.FromThisAssembly().BasedOn<IConsumer>().LifestyleTransient());
            AuthContext.ExecuteMigration();
        }
    }
}