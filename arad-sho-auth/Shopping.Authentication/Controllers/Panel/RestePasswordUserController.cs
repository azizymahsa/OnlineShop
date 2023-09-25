using System;
using System.Web.Http;
using Shopping.Authentication.Interfaces;
using Shopping.Authentication.Models.QueryModel;
using Shopping.Authentication.Models.QueryModel.Dto;
using Shopping.Authentication.SeedWorks.Core;
using Shopping.Authentication.Services.Commands.PanelCommands;

namespace Shopping.Authentication.Controllers.Panel
{
    public class RestePasswordUserController: ApiControllerBase
    {
        private readonly IPanelSecutiytCommandService _panelSecutiytCommandService;
        private readonly IPanelSecurityQueryService _panelSecurityQueryService;
        public RestePasswordUserController(IPanelSecutiytCommandService panelSecutiytCommandService, IPanelSecurityQueryService panelSecurityQueryService)
        {
            _panelSecutiytCommandService = panelSecutiytCommandService;
            _panelSecurityQueryService = panelSecurityQueryService;
        }
        public IHttpActionResult Put(RestePasswordCommand command)
        {
            try
            {
                _panelSecutiytCommandService.UpdateUserPassword(command);
                return Ok(new ResponseModel
                {
                    Message = "تغییر رمز  با موفقیت انجام شد",
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
        public IProfileDto Get(Guid userId)
        {
            return _panelSecurityQueryService.GetUserInfo(userId);
        }
    }
}