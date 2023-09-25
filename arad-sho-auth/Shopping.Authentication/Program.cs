using System;
using NLog;
using Topshelf;

namespace Shopping.Authentication
{
    class Program
    {
        // ReSharper disable once UnusedParameter.Local
        static void Main(string[] args)
        {
            try
            {
                var host = HostFactory.New(x =>
                {
                    x.UseNLog();
                    x.Service<ApiStart>(s =>
                    {
                        s.ConstructUsing(name => new ApiStart());
                        s.WhenStarted(tc => tc.Start());
                        s.WhenStopped(tc => tc.Stop());
                    });

                    x.OnException(ex =>
                    {
                        var logger = LogManager.GetCurrentClassLogger();
                        logger.Error(ex);
                    });
                    x.EnableShutdown();
#if (DEBUG)
                    x.RunAsLocalService();
#else
                x.RunAsNetworkService();
#endif
                    x.StartAutomaticallyDelayed();
                    x.SetDescription("Auth API");
                    x.SetDisplayName("Auth API");
                    x.SetServiceName("AuthAPI");

                    x.EnableServiceRecovery(rc =>
                    {
                        rc.OnCrashOnly();
                        rc.RestartService(0); 
                        rc.RestartService(1); 
                        rc.RestartService(5); 
                        rc.SetResetPeriod(1); 
                    });
                });
                host.Run();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
