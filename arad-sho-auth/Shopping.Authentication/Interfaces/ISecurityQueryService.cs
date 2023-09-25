using System;
using Shopping.Authentication.Models.QueryModel.Dto;

namespace Shopping.Authentication.Interfaces
{
    public interface ISecurityQueryService
    {
        /// <summary>
        /// دریافت اطلاعات کاربر
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        IProfileDto GetUserInfo(Guid userId);

        string GetUserCode(string mobileNumber);
    }
}