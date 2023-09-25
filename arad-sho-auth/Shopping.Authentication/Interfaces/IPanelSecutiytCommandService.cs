using System;
using Shopping.Authentication.Services.Commands.PanelCommands;

namespace Shopping.Authentication.Interfaces
{
    public interface IPanelSecutiytCommandService
    {
        void CreateUser(CraeteUserPanelCommand command);
        void CreateUserWithRole(CreateUserPanelWithRoleCommand command);
        void DeleteUser(Guid id);
        void UpdateUserPassword(RestePasswordCommand command);
        void UpdateUserPanelInfo(UpdateUserPanelCommand command);
    }
}