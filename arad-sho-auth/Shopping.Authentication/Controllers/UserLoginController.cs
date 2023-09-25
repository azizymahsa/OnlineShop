using System;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.AspNet.Identity;
using Shopping.Authentication.Interfaces;
using Shopping.Authentication.Models.QueryModel;
using Shopping.Authentication.SeedWorks.Exceptions;
using Shopping.Authentication.Services.Commands;

namespace Shopping.Authentication.Controllers
{
    public class UserLoginController : ApiController
    {
        private readonly IAppRepository _repository;
        private readonly IIdentityMessageService _identityMessageService;
        public UserLoginController(IAppRepository repository, IIdentityMessageService identityMessageService)
        {
            _repository = repository;
            _identityMessageService = identityMessageService;
        }
        public async Task<IHttpActionResult> Post(LoginRequestCommand command)
        {
            try
            {
                var verificationCode = await _repository.RegisterRequest(command.MobileNumber, command.AppType);
               Task.Run(()=> _identityMessageService.SendAsync(new IdentityMessage
                {
                    Destination = command.MobileNumber,
                    Body = verificationCode,
                    Subject = "CreateUser"
                }));
                return Ok(new ResponseModel
                {
                    Message = "ثبت نام با موفقیت انجام شد",
                    Success = true,
                });
            }
            catch (CustomException ex)
            {
                return Ok(new ResponseModel
                {
                    Message = ex.Message,
                    Success = false,
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