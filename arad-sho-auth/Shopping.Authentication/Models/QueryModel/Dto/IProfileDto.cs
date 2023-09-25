using System;
using System.Collections.Generic;

namespace Shopping.Authentication.Models.QueryModel.Dto
{
    public interface IProfileDto
    {
        /// <summary>
        /// نام
        /// </summary>
        string FirstName { get; set; }
        /// <summary>
        /// نام خانوادگی
        /// </summary>
        string LastName { get; set; }
        /// <summary>
        /// شناسه کاربر
        /// </summary>
        Guid UserId { get; set; }
        /// <summary>
        /// نقش ها
        /// </summary>
        IEnumerable<string> Roles { get; set; }
        string Email { get; set; }
        string NationalCode { get; set; }
        string MobileNumber { get; set; }
    }
}