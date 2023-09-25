using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Shopping.Authentication.Models;
using Shopping.Authentication.Repository.Entities;
using Shopping.Shared.Enums;

namespace Shopping.Authentication.Interfaces
{
    public interface IAppRepository
    {
        Task DeActiveUser(string userId,AppType appType);
        Task ActiveUser(string userId,AppType appType);
        Task<string> RegisterRequest(string mobileNumber, AppType appType);
        Task<IList<string>> GetUserRoles(ApplicationUser user);
        Task<IdentityRole> GetUserRole(ApplicationUser user);
        Task<IdentityResult> SetUserRole(ApplicationUser user, IdentityRole role);
        Task<IdentityResult> RemoveUserRole(ApplicationUser user, IdentityRole role);
        Task DeleteUser(string userName);
        Task<ApplicationUser> FindUser(string userName, string password);
        Task<ApplicationUser> FindUser(Guid userId);
        Task<ApplicationUser> FindUser(string username);
        IQueryable<ApplicationUser> FindAll();
        Task<IdentityResult> DeleteUser(ApplicationUser user);
        void ResetPassword(string userName, string oldPassword, string newPassword);
        Client FindClient(string clientId);
        Task<IdentityResult> RegisterUserAsync(ApplicationUser user, string password);
        Task<bool> AddRefreshToken(RefreshToken token);
        Task<bool> RemoveRefreshToken(RefreshToken refreshToken);
        Task<RefreshToken> FindRefreshToken(string refreshTokenId);
        List<RefreshToken> GetAllRefreshTokens();
        Task<bool> RemoveRefreshToken(string refreshTokenId);
    }
}