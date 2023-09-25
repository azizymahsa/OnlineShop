using System;
using System.Collections.Generic;
using Shopping.Authentication.Models.QueryModel.Dto;

namespace Shopping.Authentication.Models.QueryModel
{
    public class ProfileDto : IProfileDto
    {
        /// <summary>
        /// نام
        /// </summary>
        public string FirstName { get; set; }
        /// <summary>
        /// نام خانوادگی
        /// </summary>
        public string LastName { get; set; }
        /// <summary>
        /// شناسه کاربر
        /// </summary>
        public Guid UserId { get; set; }
        /// <summary>
        /// نقش ها
        /// </summary>
        public IEnumerable<string> Roles { get; set; }

        public string Email { get; set; }
        public string NationalCode { get; set; }
        public string MobileNumber { get; set; }
        
    }
}