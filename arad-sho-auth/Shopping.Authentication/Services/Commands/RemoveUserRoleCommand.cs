using System;

namespace Shopping.Authentication.Services.Commands
{
    /// <summary>
    /// حذف نقش کاربر
    /// </summary>
    public class RemoveUserRoleCommand
    {
        /// <summary>
        /// نام کاربری 
        /// </summary>
        public Guid UserId { get; set; }
        /// <summary>
        /// نقش
        /// </summary>
        public string RoleName { get; set; }
    }
}