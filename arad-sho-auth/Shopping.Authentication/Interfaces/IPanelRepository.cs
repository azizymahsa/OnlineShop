using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Shopping.Authentication.Models;

namespace Shopping.Authentication.Interfaces
{
    public interface IPanelRepository
    {
      
        /// <summary>
        /// دریافت نقش های کاربر
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        Task<IList<string>> GetUserRoles(PanelUser user);
        /// <summary>
        /// دریافت نقش کاربر
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        Task<IdentityRole> GetUserRole(PanelUser user);
        /// <summary>
        /// انتساب نقش به کاربر
        /// </summary>
        /// <param name="user"></param>
        /// <param name="role"></param>
        /// <returns></returns>
        Task<IdentityResult> SetUserRole(PanelUser user, IdentityRole role);
        /// <summary>
        /// حذف نقش کاربر
        /// </summary>
        /// <param name="user"></param>
        /// <param name="role"></param>
        /// <returns></returns>
        Task<IdentityResult> RemoveUserRole(PanelUser user, IdentityRole role);
        /// <summary>
        /// حذف کاربر
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        Task DeleteUser(string userName);
        /// <summary>
        /// جستوجوی کاربر
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        Task<PanelUser> FindUser(string userName, string password);
        /// <summary>
        /// دریافت اطلاعات کاربر
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<PanelUser> FindUser(Guid userId);
        /// <summary>
        /// دریافت اطلاعات کاربر
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        Task<PanelUser> FindUser(string username);
        /// <summary>
        /// دریافت کاربران
        /// </summary>
        /// <returns></returns>
        IQueryable<PanelUser> FindAll();
        /// <summary>
        /// حذف کاربر
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        Task<IdentityResult> DeleteUser(PanelUser user);
        /// <summary>
        /// ریست کردن پسورد
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="oldPassword"></param>
        /// <param name="newPassword"></param>
        void ResetPassword(string userName, string oldPassword, string newPassword);
        /// <summary>
        /// ایجاد کاربر
        /// </summary>
        /// <param name="user"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        Task<IdentityResult> RegisterUserAsync(PanelUser user, string password);
        bool IsCorrectPassword(PanelUser user, string password);
        void ResetPassword(PanelUser user, string oldPassword, string newPassword);
        void UpdateUserInfo(string userName, string firstName, string lastName, string nationalCode,
            string mobileNumber, string email);
        Task<IdentityResult> RegisterUserWithRole(PanelUser user, IdentityRole role, string password);

    }
}