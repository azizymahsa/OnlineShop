using System;
using System.Linq;
using Microsoft.AspNet.Identity.EntityFramework;
using Shopping.Authentication.Interfaces;
using Shopping.Authentication.Models;
using Shopping.Authentication.SeedWorks.Exceptions;
using Shopping.Authentication.Services.Commands.PanelCommands;

namespace Shopping.Authentication.Services
{
    public class PanelSecurityCommandService : IPanelSecutiytCommandService
    {
        private readonly IPanelRepository _panelRepository;
        public PanelSecurityCommandService(IPanelRepository panelRepository)
        {
            _panelRepository = panelRepository;
        }
        public void CreateUser(CraeteUserPanelCommand command)
        {
            var identityResult =
                _panelRepository.RegisterUserAsync(
                    new PanelUser(command.UserName, command.FirstName, command.LastName,
                        command.NationalCode, command.MobilNumber, command.Email), command.Password).Result;
            if (identityResult == null)
            {
                throw new CustomException("Internal Server Error");
            }
            if (!identityResult.Succeeded)
            {
                throw new CustomException(identityResult.Errors.Aggregate("",
                    (current, error) => current + error + "\n"));
            }
        }

        public void CreateUserWithRole(CreateUserPanelWithRoleCommand command)
        {
            var user = new PanelUser(command.UserName, command.FirstName, command.LastName, command.NationalCode, command.MobileNumber, command.Email);
            var identityResult = _panelRepository
                .RegisterUserWithRole(user, new IdentityRole(command.RoleName), command.Password).Result;
            if (identityResult == null)
            {
                throw new CustomException(" Internal Server Error");
            }
            if (!identityResult.Succeeded)
            {
                throw new CustomException(identityResult.Errors.Aggregate("",
                    (current, error) => current + error + "\n"));
            }
        }


        public void UpdateUserPanelInfo(UpdateUserPanelCommand command)
        {
            var user = _panelRepository.FindUser(command.UserId).Result;
            if (user == null)
            {
                throw new CustomException("کاربر یافت نشد");
            }
            _panelRepository.UpdateUserInfo(user.UserName, command.FirstName, command.LastName, command.NationalCode, command.MobileNumber, command.Email);
        }

        public void DeleteUser(Guid id)
        {
            var user = _panelRepository.FindUser(id).Result;
            if (user == null)
            {
                throw new CustomException("کاربر مورد نظر یافت نشد");
            }
            var identityResult = _panelRepository.DeleteUser(user).Result;
            if (!identityResult.Succeeded)
            {
                throw new CustomException(identityResult.Errors.Aggregate("",
                    (current, error) => current + error + "\n"));
            }
        }

        public void UpdateUserPassword(RestePasswordCommand command)
        {
            var user = _panelRepository.FindUser(command.UserId).Result;
            if (user == null)
            {
                throw new CustomException("کاربر یافت نشد");
            }
            var isCorrectPassword = _panelRepository.IsCorrectPassword(user, command.OldPassword);
            if (!isCorrectPassword)
            {
                throw new CustomException("رمز عبور صحیح نمی باشد");
            }
            _panelRepository.ResetPassword(user, command.OldPassword, command.NewPassword);
        }
    }
}