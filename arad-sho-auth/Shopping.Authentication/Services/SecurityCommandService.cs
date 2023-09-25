using System;
using System.Linq;
using Microsoft.AspNet.Identity.EntityFramework;
using Shopping.Authentication.Interfaces;
using Shopping.Authentication.Models;
using Shopping.Authentication.SeedWorks.Exceptions;
using Shopping.Authentication.Services.Commands;

namespace Shopping.Authentication.Services
{
    public class SecurityCommandService : ISecurityCommandService
    {
        private readonly IAppRepository _repository;
        public SecurityCommandService(IAppRepository repository)
        {
            _repository = repository;
        }

        /// <summary>
        /// ثبت نام کاربر
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public void RegisterUser(RegisterUserCommand command)
        {
            var identityResult = _repository.RegisterUserAsync(
                new ApplicationUser(command.UserName), command.Password).Result;
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
        /// <summary>
        /// انتساب نقش به کاربر
        /// </summary>
        /// <param name="command"></param>
        public void AssignUserRole(AssignUserRoleCommand command)
        {
            var user = _repository.FindUser(command.UserId).Result;
            if (user == null)
            {
                throw new CustomException("کاربر یافت نشد");
            }
            var identityResult = _repository.SetUserRole(user, new IdentityRole(command.RoleName)).Result;
            if (identityResult == null)
            {
                throw new CustomException("کاربر دارای این نقش می باشد");
            }
            if (!identityResult.Succeeded)
            {
                throw new CustomException(identityResult.Errors.Aggregate("",
                    (current, error) => current + error + "\n"));
            }
        }
        /// <summary>
        /// حذف نقش کاربر
        /// </summary>
        /// <param name="command"></param>
        public void RemoveUserRole(RemoveUserRoleCommand command)
        {
            var user = _repository.FindUser(command.UserId).Result;
            if (user == null)
            {
                throw new CustomException("کاربر یافت نشد");
            }
            var identityResult = _repository.RemoveUserRole(user, new IdentityRole
            {
                Name = command.RoleName
            }).Result;
            if (!identityResult.Succeeded)
            {
                throw new CustomException(identityResult.Errors.Aggregate("",
                    (current, error) => current + error + "\n"));
            }
        }
        /// <summary>
        /// حذف کاربر
        /// </summary>
        /// <param name="id"></param>
        public void DeleteUser(Guid id)
        {
            var user = _repository.FindUser(id).Result;
            if (user == null)
            {
                throw new CustomException("کاربر مورد نظر یافت نشد");
            }
            var identityResult = _repository.DeleteUser(user).Result;
            if (!identityResult.Succeeded)
            {
                throw new CustomException(identityResult.Errors.Aggregate("",
                    (current, error) => current + error + "\n"));
            }
        }
        public bool DeleteRefreshToken(string tokenId)
        {
            var result = _repository.RemoveRefreshToken(tokenId).Result;
            return result;
        }
        
    }
}