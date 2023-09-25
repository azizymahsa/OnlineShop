namespace Shopping.Authentication.Services.Commands
{
    public class LoginCommand
    {
        public string MobileNumber { get; set; }
        public string Code { get; set; }
    }
}