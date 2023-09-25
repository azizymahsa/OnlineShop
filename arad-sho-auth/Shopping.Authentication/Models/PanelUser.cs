using System;
using Microsoft.AspNet.Identity.EntityFramework;
using Shopping.Authentication.SeedWorks;
using Shopping.Authentication.SeedWorks.Exceptions;

namespace Shopping.Authentication.Models
{
    public class PanelUser : IdentityUser
    {
        protected PanelUser()
        {
        }

        public PanelUser(string userName, string firstName, string lastName, string nationalCode, string phoneNumber, string email) : base(userName)
        {
            if (!nationalCode.IsValidNationalCode())
            {
                throw new CustomException("لطفا کد ملی را صحیح وارد نمایید ");
            }
            FirstName = firstName;
            LastName = lastName;
            NationalCode = nationalCode;
            RegisterDate = DateTime.Now;
            PhoneNumber = phoneNumber;
            Email = email;
        }
        /// <summary>
        /// نام
        /// </summary>
        public string FirstName { get; set; }
        /// <summary>
        /// نام خانوادگی
        /// </summary>
        public string LastName { get; set; }
        public DateTime RegisterDate { get; set; }
        public string NationalCode { get; set; }
    }
}