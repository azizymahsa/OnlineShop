using System;
using System.Web.Http;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.DataHandler;
using Microsoft.Owin.Security.OAuth;
using Owin;
using Shopping.Authentication.BusConfigs;
using Shopping.Authentication.Providers;
using Shopping.Authentication.SeedWorks.Helpers;

namespace Shopping.Authentication
{
    public class Startup
    {
        public static OAuthBearerAuthenticationOptions OAuthBearerOptions { get; private set; }
        public void Configuration(IAppBuilder app)
        {
            HttpConfiguration config = new HttpConfiguration();
            WebApiConfig.Register(config);
            BusConfiguratorService.Configure();
            ConfigureOAuth(app);
            app.UseCors(Microsoft.Owin.Cors.CorsOptions.AllowAll);
            app.UseWebApi(config);
        }
        public static void ConfigureOAuth(IAppBuilder app)
        {
            var oAuthServerOptions = new OAuthAuthorizationServerOptions
            {
                AllowInsecureHttp = true,
                TokenEndpointPath = new PathString("/token"),
                AccessTokenExpireTimeSpan = TimeSpan.FromDays(365),
                Provider = new AuthorizationServerProvider(),
                AuthenticationType = "Bearer",
                AuthenticationMode = AuthenticationMode.Active,
                AccessTokenFormat = new TicketDataFormat(MachineKeyDataProtectorHelper.New()),
                RefreshTokenProvider = new AuthorizationRefreshTokenProvider()
            };
            app.UseOAuthAuthorizationServer(oAuthServerOptions);

            OAuthBearerOptions = new OAuthBearerAuthenticationOptions
            {
                AuthenticationMode = AuthenticationMode.Active,
                AccessTokenFormat = new TicketDataFormat(MachineKeyDataProtectorHelper.New())
            };
            app.UseOAuthBearerAuthentication(OAuthBearerOptions);
        }
    }
}