using System;
using Shopping.Authentication.Services.Commands;

namespace Shopping.Authentication.Interfaces
{
    public interface ISecurityCommandService
    {
        /// <summary>
        /// ثبت نام کاربر
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        void RegisterUser(RegisterUserCommand command);
        /// <summary>
        /// انتساب نقش به کاربر
        /// </summary>
        /// <param name="command"></param>
        void AssignUserRole(AssignUserRoleCommand command);
        /// <summary>
        /// حذف نقش کاربر
        /// </summary>
        /// <param name="command"></param>
        void RemoveUserRole(RemoveUserRoleCommand command);
        /// <summary>
        /// حذف کاربر
        /// </summary>
        /// <param name="id"></param>
        void DeleteUser(Guid id);

        bool DeleteRefreshToken(string tokenId);
    }
}