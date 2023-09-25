using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Shopping.Authentication.Interfaces;
using Shopping.Authentication.Models;
using Shopping.Authentication.Repository.DbContexts;
using Shopping.Authentication.SeedWorks;
using Shopping.Authentication.SeedWorks.Exceptions;

namespace Shopping.Authentication.Repository
{
    public class PanelAuthRepository : IPanelRepository
    {
        private readonly UserStore<PanelUser> _userStore;
        private readonly UserManager<PanelUser> _userManager;
        private readonly ApplicationRoleManager _roleManager;
        private readonly AuthContext _context;
        public PanelAuthRepository()
        {
            _context = new AuthContext();
            _userStore = new UserStore<PanelUser>(_context);
            _userManager = new UserManager<PanelUser>(_userStore);
            _roleManager = new ApplicationRoleManager(new RoleStore<IdentityRole>(_context));
        }
        public async Task<IList<string>> GetUserRoles(PanelUser user)
        {
            return await _userManager.GetRolesAsync(user.Id);
        }
        public async Task<IdentityRole> GetUserRole(PanelUser user)
        {
            return await _roleManager.FindByIdAsync(user.Roles.FirstOrDefault()?.RoleId);
        }
        public async Task<IdentityResult> SetUserRole(PanelUser user, IdentityRole role)
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
                var refreshTokens = _context.RefreshTokens.Where(item => item.Subject == user.UserName);
                foreach (var refreshToken in refreshTokens)
                {
                    _context.RefreshTokens.Remove(refreshToken);
                }
                _context.SaveChanges();
                return _userManager.AddToRole(user.Id, role.Name);
            }
            return null;
        }
        public async Task<IdentityResult> RemoveUserRole(PanelUser user, IdentityRole role)
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
        public async Task<PanelUser> FindUser(string userName, string password)
        {
            var user = await _userManager.FindAsync(userName, password);
            return user;
        }
        public async Task<PanelUser> FindUser(Guid userId)
        {
            return await _userManager.FindByIdAsync(userId.ToString());
        }
        public async Task<PanelUser> FindUser(string username)
        {
            return await _userManager.FindByNameAsync(username);
        }
        public IQueryable<PanelUser> FindAll()
        {
            return _userManager.Users;
        }
        public async Task<IdentityResult> DeleteUser(PanelUser user)
        {
            var refreshTokens = _context.RefreshTokens.Where(item => item.Subject == user.UserName);
            foreach (var refreshToken in refreshTokens)
            {
                _context.RefreshTokens.Remove(refreshToken);
            }
            _context.SaveChanges();
            return await _userManager.DeleteAsync(user);
        }
        public async void ResetPassword(string userName, string oldPassword, string newPassword)
        {
            var user = await _userManager.FindByNameAsync(userName);
            var passwordHash = _userManager.PasswordHasher.HashPassword(newPassword);
            await _userStore.SetPasswordHashAsync(user, passwordHash);
            await _userStore.UpdateAsync(user);
        }
        public async Task<IdentityResult> RegisterUserAsync(PanelUser user, string password)
        {
            var identityResult = await _userManager.CreateAsync(user, password);
            return identityResult;
        }
        public bool IsCorrectPassword(PanelUser user, string password)
        {
            return _userManager.CheckPasswordAsync(user, password).Result;
        }
        public void ResetPassword(PanelUser user, string oldPassword, string newPassword)
        {
            var identityResult = _userManager.ChangePassword(user.Id, oldPassword, newPassword);
            if (!identityResult.Succeeded)
            {
                throw new CustomException(identityResult.Errors.Aggregate("",
                    (current, error) => current + error + "\n"));
            }
        }
        public void UpdateUserInfo(string userName, string firstName, string lastName, string nationalCode, string mobileNumber, string email)
        {
            if (!nationalCode.IsValidNationalCode())
            {
                throw new CustomException("لطفا کد ملی را صحیح وارد نمایید ");
            }
            var user = _userManager.FindByName(userName);
            user.FirstName = firstName;
            user.LastName = lastName;
            user.NationalCode = nationalCode;
            user.PhoneNumber = mobileNumber;
            user.Email = email;
            _context.SaveChanges();
        }
        public async Task<IdentityResult> RegisterUserWithRole(PanelUser user, IdentityRole role, string password)
        {
            var identityResult = await _userManager.CreateAsync(user, password);
            if (identityResult.Succeeded)
            {
                if (_roleManager.FindByName(role.Name) == null)
                {
                    await _roleManager.CreateAsync(role);
                }
                _userManager.AddToRole(user.Id, role.Name);
            }
            return identityResult;
        }
    }
}