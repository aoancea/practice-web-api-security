using System.Web.Http;

namespace Phobos.Api.Controllers
{
    public class AccountController : ApiController
    {
        public IHttpActionResult Register()
        {
            return Ok();
        }
    }
}