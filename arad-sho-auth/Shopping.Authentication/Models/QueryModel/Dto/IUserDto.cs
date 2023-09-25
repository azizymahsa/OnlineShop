using System;
using System.Collections.Generic;

namespace Shopping.Authentication.Models.QueryModel.Dto
{
    public interface IUserDto
    {
        /// <summary>
        /// شناسه کاربر 
        /// </summary>
        Guid UserId { get; set; }
        /// <summary>
        /// نام کاربری
        /// </summary>
        string Username { get; set; }
        /// <summary>
        /// نام
        /// </summary>
        string FirstName { get; set; }
        /// <summary>
        /// نام خانوادگی
        /// </summary>
        string LastName { get; set; }
        /// <summary>
        /// نقش های کاربر
        /// </summary>
        IList<string> Roles { get; set; }
        string Email { get; set; }
        string NationalCode { get; set; }
        string MobileNumber { get; set; }
    }
}