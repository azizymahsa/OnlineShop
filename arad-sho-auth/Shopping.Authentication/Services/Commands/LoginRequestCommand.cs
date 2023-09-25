using Shopping.Authentication.SeedWorks.Enums;
using Shopping.Shared.Enums;

namespace Shopping.Authentication.Services.Commands
{
    public class LoginRequestCommand
    {
        public AppType AppType { get; set; }
        public string MobileNumber { get; set; }
    }
}