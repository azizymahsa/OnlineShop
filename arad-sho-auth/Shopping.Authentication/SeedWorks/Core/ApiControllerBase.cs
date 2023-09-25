using System.Net;
using System.Net.Http;
using System.Web.Http;
using Shopping.Authentication.Models.QueryModel;

namespace Shopping.Authentication.SeedWorks.Core
{
    public class ApiControllerBase : ApiController
    {
        protected HttpResponseMessage OkResult(string actionName)
        {
            return
                Request.CreateResponse(HttpStatusCode.OK,
                    CreateResponseModel($"{actionName} با موفقیت انجام شد", true));
        }
        protected HttpResponseMessage BadRequestResult(string actionName)
        {
            return
                Request.CreateResponse(HttpStatusCode.OK,
                    actionName.Contains("is already taken")
                        ? CreateResponseModel("نام کاربری این کاربر قبلا ایجاد شده است")
                        : CreateResponseModel($"{actionName} با خطا مواجه گردید"));
        }
        protected ResponseModel CreateResponseModel(string message, bool success = false)
        {
            return new ResponseModel
            {
                Message = message,
                Success = success,
            };
        }
    }
}