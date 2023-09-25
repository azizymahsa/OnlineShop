using System;
using System.Threading.Tasks;
using System.Web.Http;
using Shopping.Authentication.Interfaces;
using Shopping.Authentication.Models.QueryModel;
using Shopping.Authentication.Models.QueryModel.Dto;
using Shopping.Authentication.SeedWorks.Core;
using Shopping.Authentication.SeedWorks.Exceptions;
using Shopping.Authentication.Services.Commands;

namespace Shopping.Authentication.Controllers
{
    [RoutePrefix("api/User")]
    public class UserController : ApiControllerBase
    {
        private readonly IAppRepository _appRepository;
        private readonly ISecurityQueryService _securityQueryService;
        public UserController(ISecurityQueryService securityQueryService, IAppRepository appRepository)
        {
            _securityQueryService = securityQueryService;
            _appRepository = appRepository;
        }
        [Authorize]
        public IProfileDto Get(Guid id)
        {
            return _securityQueryService.GetUserInfo(id);
        }
        [Route("Active")]
        public async Task<IHttpActionResult> Put(ActiveUserCommand command)
        {
            try
            {
                await _appRepository.ActiveUser(command.UserId, command.AppType);
                return Ok(new ResponseModel
                {
                    Message = "فعال سازی کاربر",
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
        [Route("DeActive")]
        public async Task<IHttpActionResult> Put(DeActiveUserCommand command)
        {
            try
            {
                await _appRepository.DeActiveUser(command.UserId, command.AppType);
                return Ok(new ResponseModel
                {
                    Message = "فعال سازی کاربر",
                    Success = true,
                });
            }
            catch (Exception e)
            {
                return InternalServerError(e);
            }
        }

        [Route("VerficationCode")]
        public IHttpActionResult Get(string mobileNumber)
        {
            try
            {
                var result = _securityQueryService.GetUserCode(mobileNumber);
                return Ok(result);
            }
            catch (CustomException e)
            {
                return InternalServerError(e);
            }
        }
    }
}