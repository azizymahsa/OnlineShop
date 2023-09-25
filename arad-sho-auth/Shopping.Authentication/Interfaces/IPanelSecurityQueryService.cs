using System;
using System.Collections.Generic;
using Shopping.Authentication.Models.QueryModel.Dto;

namespace Shopping.Authentication.Interfaces
{
    public interface IPanelSecurityQueryService
    {
        /// <summary>
        /// دریافت اطلاعات کاربر
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        IProfileDto GetUserInfo(Guid userId);
        /// <summary>
        /// دریافت کاربران
        /// </summary>
        /// <returns></returns>
        IEnumerable<IUserDto> GetUsers();
    }
}