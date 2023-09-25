using System;
using System.Collections.Generic;
using System.Linq;
using Shopping.Authentication.Interfaces;
using Shopping.Authentication.Models.QueryModel;
using Shopping.Authentication.Models.QueryModel.Dto;

namespace Shopping.Authentication.Services
{
    public class PanelSecurityQueryService: IPanelSecurityQueryService
    {
        private readonly IPanelRepository _panelRepository;
        public PanelSecurityQueryService(IPanelRepository panelRepository)
        {
            _panelRepository = panelRepository;
        }
        public IProfileDto GetUserInfo(Guid userId)
        {
            var panelUser = _panelRepository.FindUser(userId).Result;
            return new ProfileDto
            {
                LastName = panelUser.LastName,
                FirstName = panelUser.FirstName,
                Roles = _panelRepository.GetUserRoles(panelUser).Result,
                UserId = Guid.Parse(panelUser.Id),
                MobileNumber = panelUser.PhoneNumber,
                Email = panelUser.Email,
                NationalCode = panelUser.NationalCode
                
            };
        }

        public IEnumerable<IUserDto> GetUsers()
        {
            var panelUser = _panelRepository.FindAll().ToList();
            return panelUser.Select(item => new UserDto
            {
                FirstName = item.FirstName,
                LastName = item.LastName,
                Username = item.UserName,
                UserId = Guid.Parse(item.Id),
                Roles = _panelRepository.GetUserRoles(item).Result,
                MobileNumber = item.PhoneNumber,
                Email = item.Email,
                NationalCode = item.NationalCode

            }).ToList();
        }
    }
}