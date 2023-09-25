using System;

namespace Shopping.Authentication.Services.Commands.PanelCommands
{
    public class UpdateUserPanelCommand
    {
        public Guid UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MobileNumber { get; set; }
        public string NationalCode { get; set; }
        public string Email { get; set; }
    }
}