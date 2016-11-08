using System.Web.Mvc;

namespace Phobos.Web.Controllers
{
	public class LoginController : Controller
    {
        public ActionResult Login()
        {
            return View();
        }

		public ActionResult Signup()
		{
			return View();
		}
	}
}