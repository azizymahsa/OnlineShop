using Shopping.Shared.Enums;

namespace Shopping.Authentication.Services.Commands
{
    public class ActiveUserCommand
    {
        public string UserId { get; set; }
        public AppType AppType { get; set; }
    }
    public class DeActiveUserCommand
    {
        public string UserId { get; set; }
        public AppType AppType { get; set; }
    }
}