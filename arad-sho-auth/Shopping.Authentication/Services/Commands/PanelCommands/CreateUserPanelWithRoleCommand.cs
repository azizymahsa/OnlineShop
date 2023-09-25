namespace Shopping.Authentication.Services.Commands.PanelCommands
{
    public class CreateUserPanelWithRoleCommand
    {
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MobileNumber { get; set; }
        public string Password { get; set; }
        public string NationalCode { get; set; }
        public string Email { get; set; }
        public string RoleName { get; set; }
    }
}