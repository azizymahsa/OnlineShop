using System.Web.Http;

namespace Shopping.Authentication.Controllers
{
    [Authorize]
    public class TestController : ApiController
    {
        public bool Get()
        {
            return true;
        }
    }
}