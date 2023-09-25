using System;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Shopping.Authentication.Models
{
    public class ApplicationUser : IdentityUser
    {
        protected ApplicationUser()
        {
        }
        public ApplicationUser(string userName) : base(userName)
        {
            RegisterDate = DateTime.Now;
            ShopIsActive = true;
            CustomerIsActive = true;
        }
        public DateTime RegisterDate { get; set; }
        public DateTime? CodeExpireDate { get; set; }
        public string VerificationCode { get; set; }
        public bool ShopIsActive { get; set; }
        public bool CustomerIsActive { get; set; }
    }
}