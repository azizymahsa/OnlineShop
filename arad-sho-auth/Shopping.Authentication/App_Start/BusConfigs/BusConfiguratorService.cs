using System;
using Castle.MicroKernel.Registration;
using MassTransit;
using Shopping.Authentication.EventHandlers;

namespace Shopping.Authentication.BusConfigs
{
    public class BusConfiguratorService
    {
        public static void Configure()
        {
            var busControl = Bus.Factory.CreateUsingRabbitMq(cfg =>
            {
                cfg.Host(new Uri("rabbitmq://" + ApiSettings.Default.RabbitMQ_ServerName), h =>
                {
                    h.Username(ApiSettings.Default.RabbitMQ_UserName);
                    h.Password(ApiSettings.Default.RabbitMQ_Password);
                });

                cfg.ReceiveEndpoint("shopping_bus", endpoint =>
                    {
                     
                        endpoint.UseMessageScope();


                        endpoint.Consumer<UserActivationEventHandler>();

                    });
            });
            busControl.Start();
            Bootstrapper.WindsorContainer.Register(Component.For<IBus>().Instance(busControl).LifestyleSingleton());
        }
    }
}