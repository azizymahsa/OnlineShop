using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Shopping.Authentication.Interfaces;
using Shopping.Authentication.Models;
using Shopping.Authentication.Repository.DbContexts;
using Shopping.Authentication.Repository.Entities;
using Shopping.Authentication.SeedWorks;
using Shopping.Authentication.SeedWorks.Exceptions;
using Shopping.Shared.Enums;

namespace Shopping.Authentication.Repository
{
    public class AppRepository : IAppRepository
    {
        private readonly UserStore<ApplicationUser> _userStore;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationRoleManager _roleManager;
        private readonly AuthContext _context;
        private const int ExpiredMinuteTime = 3;
        public AppRepository()
        {
            _context = new AuthContext();
            _userStore = new UserStore<ApplicationUser>(_context);
            _userManager = new UserManager<ApplicationUser>(_userStore);
            _roleManager = new ApplicationRoleManager(new RoleStore<IdentityRole>(_context));
        }
        public async Task DeActiveUser(string userId, AppType appType)
        {
            var user = _context.Users.OfType<ApplicationUser>().SingleOrDefault(p => p.Id == userId);
            if (user == null)
            {
                throw new CustomException("کاربر یافت نشد");
            }

            switch (appType)
            {
                case AppType.Customer:
                {
                    user.CustomerIsActive = false;
                    var refreshTokens = _context.RefreshTokens
                        .Where(p => p.Subject == user.UserName && p.ClientId == "CustomerUserApp");
                    foreach (var refreshToken in refreshTokens)
                    {
                        _context.RefreshTokens.Remove(refreshToken);
                    }

                    break;
                }
                case AppType.Shop:
                {
                    user.ShopIsActive = false;
                    var refreshTokens = _context.RefreshTokens
                        .Where(p => p.Subject == user.UserName && p.ClientId == "ShopUserApp");
                    foreach (var refreshToken in refreshTokens)
                    {
                        _context.RefreshTokens.Remove(refreshToken);
                    }

                    break;
                }
            }
            await _context.SaveChangesAsync();
        }

        public async Task ActiveUser(string userId, AppType appType)
        {
            var user = _context.Users.OfType<ApplicationUser>().SingleOrDefault(p => p.Id == userId);
            if (user == null)
            {
                throw new CustomException("کاربر یافت نشد");
            }
            switch (appType)
            {
                case AppType.Customer:
                    user.CustomerIsActive = true;
                    break;
                case AppType.Shop:
                    user.ShopIsActive = true;
                    break;
            }
            await _context.SaveChangesAsync();
        }

        public async Task<string> RegisterRequest(string mobileNumber, AppType appType)
        {
            var userRole = appType == AppType.Customer ? UserRoleNames.CustomerRole : UserRoleNames.ShopRole;
            var user = _userManager.Users.SingleOrDefault(item => item.PhoneNumber == mobileNumber);
            if (user == null)
            {
                user = new ApplicationUser(mobileNumber)
                {
                    CodeExpireDate = DateTime.Now.AddMinutes(ExpiredMinuteTime),
                    TwoFactorEnabled = true,
                    PhoneNumber = mobileNumber,
                    PhoneNumberConfirmed = false
                };
                var result = await _userManager.CreateAsync(user);
                if (result != IdentityResult.Success)
                {
                    throw new CustomException("اشکال در ایجاد کاربر لطفا دوباره تلاش نمایید");
                }
                await SetUserRole(user, new IdentityRole(userRole));
            }
            else
            {
                switch (appType)
                {
                    case AppType.Customer when !user.CustomerIsActive:
                        throw new CustomException("کاربر شما غیر فعال شده است برای اطلاعات بیشتر با پشتیبانی تماس حاصل فرمایید");
                    case AppType.Shop when !user.ShopIsActive:
                        throw new CustomException("کاربر شما غیر فعال شده است برای اطلاعات بیشتر با پشتیبانی تماس حاصل فرمایید");
                }

                user.CodeExpireDate = DateTime.Now.AddMinutes(ExpiredMinuteTime);
                user.PhoneNumberConfirmed = false;
                await SetUserRole(user, new IdentityRole(userRole));
            }
            var code = await _userManager.GenerateChangePhoneNumberTokenAsync(user.Id, user.PhoneNumber);
            user.VerificationCode = code;
            await _userManager.UpdateAsync(user);
            return code;
        }
        /// <summary>
        /// دریافت نقش های کاربر
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task<IList<string>> GetUserRoles(ApplicationUser user)
        {
            return await _userManager.GetRolesAsync(user.Id);
        }
        /// <summary>
        /// دریافت نقش کاربر
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task<IdentityRole> GetUserRole(ApplicationUser user)
        {
            return await _roleManager.FindByIdAsync(user.Roles.FirstOrDefault()?.RoleId);
        }
        /// <summary>
        /// انتساب نقش به کاربر
        /// </summary>
        /// <param name="user"></param>
        /// <param name="role"></param>
        /// <returns></returns>
        public async Task<IdentityResult> SetUserRole(ApplicationUser user, IdentityRole role)
        {
            if (_roleManager.FindByName(role.Name) == null)
            {
                var roleResult = await _roleManager.CreateAsync(role);
                if (!roleResult.Succeeded)
                {
                    return roleResult;
                }
            }
            if (!_userManager.IsInRole(user.Id, role.Name))
            {
                return _userManager.AddToRole(user.Id, role.Name);
            }
            return null;
        }
        /// <summary>
        /// حذف نقش کاربر
        /// </summary>
        /// <param name="user"></param>
        /// <param name="role"></param>
        /// <returns></returns>
        public async Task<IdentityResult> RemoveUserRole(ApplicationUser user, IdentityRole role)
        {
            if (_userManager.IsInRole(user.Id, role.Name))
            {
                var refreshTokens = _context.RefreshTokens.Where(item => item.Subject == user.UserName);
                foreach (var refreshToken in refreshTokens)
                {
                    _context.RefreshTokens.Remove(refreshToken);
                }
                _context.SaveChanges();
                return await _userManager.RemoveFromRoleAsync(user.Id, role.Name);
            }
            return null;
        }
        /// <summary>
        /// حذف کاربر
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public async Task DeleteUser(string userName)
        {
            var refreshTokens = _context.RefreshTokens.Where(item => item.Subject == userName);
            foreach (var refreshToken in refreshTokens)
            {
                _context.RefreshTokens.Remove(refreshToken);
            }
            _context.SaveChanges();
            var user = await _userManager.FindByNameAsync(userName);
            await _userManager.DeleteAsync(user);
        }
        /// <summary>
        /// جستوجوی کاربر
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public async Task<ApplicationUser> FindUser(string userName, string password)
        {
            var user = await _userManager.FindAsync(userName, password);
            return user;
        }
        /// <summary>
        /// دریافت اطلاعات کاربر
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<ApplicationUser> FindUser(Guid userId)
        {
            return await _userManager.FindByIdAsync(userId.ToString());
        }
        /// <summary>
        /// دریافت اطلاعات کاربر
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public async Task<ApplicationUser> FindUser(string username)
        {
            return await _userManager.FindByNameAsync(username);
        }
        /// <summary>
        /// دریافت کاربران
        /// </summary>
        /// <returns></returns>
        public IQueryable<ApplicationUser> FindAll()
        {
            return _userManager.Users;
        }
        /// <summary>
        /// حذف کاربر
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task<IdentityResult> DeleteUser(ApplicationUser user)
        {
            var refreshTokens = _context.RefreshTokens.Where(item => item.Subject == user.UserName);
            foreach (var refreshToken in refreshTokens)
            {
                _context.RefreshTokens.Remove(refreshToken);
            }
            _context.SaveChanges();
            return await _userManager.DeleteAsync(user);
        }
        /// <summary>
        /// ریست کردن پسورد
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="oldPassword"></param>
        /// <param name="newPassword"></param>
        public async void ResetPassword(string userName, string oldPassword, string newPassword)
        {
            var user = await _userManager.FindByNameAsync(userName);
            var passwordHash = _userManager.PasswordHasher.HashPassword(newPassword);
            await _userStore.SetPasswordHashAsync(user, passwordHash);
            await _userStore.UpdateAsync(user);
        }
        /// <summary>
        /// جستوجوی کلاینت
        /// </summary>
        /// <param name="clientId"></param>
        /// <returns></returns>
        public Client FindClient(string clientId)
        {
            var client = _context.Clients.Find(clientId);
            return client;
        }
        /// <summary>
        /// ایجاد کاربر
        /// </summary>
        /// <param name="user"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public async Task<IdentityResult> RegisterUserAsync(ApplicationUser user, string password)
        {
            var identityResult = await _userManager.CreateAsync(user, password);
            return identityResult;
        }
        public async Task<bool> AddRefreshToken(RefreshToken token)
        {
            var existingToken =
                _context.RefreshTokens.SingleOrDefault(r => r.DeviceId == token.DeviceId);
            if (existingToken != null)
            {
                await RemoveRefreshToken(existingToken.Id);
            }
            _context.RefreshTokens.Add(token);
            return await _context.SaveChangesAsync() > 0;
        }
        public async Task<bool> RemoveRefreshToken(string refreshTokenId)
        {
            var refreshToken = await _context.RefreshTokens.FindAsync(refreshTokenId);

            if (refreshToken != null)
            {
                _context.RefreshTokens.Remove(refreshToken);
                return await _context.SaveChangesAsync() > 0;
            }
            return false;
        }
        public async Task<bool> RemoveRefreshToken(RefreshToken refreshToken)
        {
            _context.RefreshTokens.Remove(refreshToken);
            return await _context.SaveChangesAsync() > 0;
        }
        public async Task<RefreshToken> FindRefreshToken(string refreshTokenId)
        {
            var refreshToken = await _context.RefreshTokens.AsNoTracking().SingleOrDefaultAsync(item => item.Id == refreshTokenId);
            return refreshToken;
        }
        public List<RefreshToken> GetAllRefreshTokens()
        {
            return _context.RefreshTokens.AsNoTracking().ToList();
        }
    }
}