using System;
using System.Linq;
using Shopping.Authentication.Interfaces;
using Shopping.Authentication.Models.QueryModel;
using Shopping.Authentication.Models.QueryModel.Dto;
using Shopping.Authentication.SeedWorks.Exceptions;

namespace Shopping.Authentication.Services
{
    public class SecurityQueryService : ISecurityQueryService
    {
        private readonly IPanelRepository _repository;
        private readonly IAppRepository _appRepository;
        public SecurityQueryService(IPanelRepository repository, IAppRepository appRepository)
        {
            _repository = repository;
            _appRepository = appRepository;
        }
        /// <summary>
        /// دریافت اطلاعات کاربر
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public IProfileDto GetUserInfo(Guid userId)
        {
            var applicationUser = _repository.FindUser(userId).Result;
            return new ProfileDto
            {
                Roles = _repository.GetUserRoles(applicationUser).Result,
                UserId = Guid.Parse(applicationUser.Id),
                FirstName = applicationUser.FirstName,
                LastName = applicationUser.LastName
            };
        }

        public string GetUserCode(string mobileNumber)
        {
            var user = _appRepository.FindAll().FirstOrDefault(item => item.PhoneNumber == mobileNumber);
            if (user == null)
            {
                throw new CustomException("کاربر یافت نشد");
            }
            return user.VerificationCode;
        }
    }
}