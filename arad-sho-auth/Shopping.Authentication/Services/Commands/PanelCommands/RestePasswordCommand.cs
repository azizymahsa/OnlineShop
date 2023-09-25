using System;

namespace Shopping.Authentication.Services.Commands.PanelCommands
{
    public class RestePasswordCommand
    {
        public Guid UserId { get; set; }
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }

    }
}