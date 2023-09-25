using System;

namespace Shopping.Authentication.Services.Commands
{
    /// <summary>
    /// انتساب نقش به کاربر
    /// </summary>
    public class AssignUserRoleCommand
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