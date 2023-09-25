using System;
using System.Collections.Generic;
using System.Web.Http;
using Shopping.Authentication.Interfaces;
using Shopping.Authentication.Models.QueryModel;
using Shopping.Authentication.Models.QueryModel.Dto;
using Shopping.Authentication.SeedWorks.Core;
using Shopping.Authentication.Services.Commands.PanelCommands;

namespace Shopping.Authentication.Controllers.Panel
{
    public class UserPanelController: ApiControllerBase
    {
        private readonly IPanelSecutiytCommandService _panelSecutiytCommandService;
        private readonly IPanelSecurityQueryService _panelSecurityQueryService;
        public UserPanelController(IPanelSecutiytCommandService panelSecutiytCommandService, IPanelSecurityQueryService panelSecurityQueryService)
        {
            _panelSecutiytCommandService = panelSecutiytCommandService;
            _panelSecurityQueryService = panelSecurityQueryService;
        }
        public  IHttpActionResult Post(CreateUserPanelWithRoleCommand command)
        {
            try
            {
                _panelSecutiytCommandService.CreateUserWithRole(command);
                return Ok(new ResponseModel
                {
                    Message = "ثبت نام کاربر با موفقیت انجام شد",
                    Success = true,
                });
            }
            catch (Exception e)
            {
                return Ok(new ResponseModel
                {
                    Message = e.Message,
                    Success = false,
                });
            }
        }

     
        public IHttpActionResult Put(UpdateUserPanelCommand command)
        {
            try
            {
                _panelSecutiytCommandService.UpdateUserPanelInfo(command);
                return Ok(new ResponseModel
                {
                    Message = "ویرایش  کاربر با موفقیت انجام شد",
                    Success = true,
                });
            }
            catch (Exception e)
            {
                return Ok(new ResponseModel
                {
                    Message = e.Message,
                    Success = false,
                });
            }
        }

        
        public IEnumerable<IUserDto> Get()
        {
            return _panelSecurityQueryService.GetUsers();
        }
        public IHttpActionResult Delete(Guid id)
        {
            try
            {
                _panelSecutiytCommandService.DeleteUser(id);
                return Ok(new ResponseModel
                {
                    Message = "حذف کاربر با موفقیت انجام شد",
                    Success = true,
                });
            }
            catch (Exception e)
            {
                return Ok(new ResponseModel
                {
                    Message = e.Message,
                    Success = false,
                });
            }
        }
    }
}