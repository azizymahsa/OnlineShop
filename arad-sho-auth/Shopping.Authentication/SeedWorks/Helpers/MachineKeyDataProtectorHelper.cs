using Microsoft.Owin.Security.OAuth;

namespace Shopping.Authentication.SeedWorks.Helpers
{
    public static class MachineKeyDataProtectorHelper
    {
        public static MachineKeyDataProtector New()
        {
            return new MachineKeyDataProtector(typeof(OAuthBearerAuthenticationMiddleware).Namespace, "Access_Token", "v1");
        }
    }
}