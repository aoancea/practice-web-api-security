using System.Web.Mvc;

namespace Phobos.Web.Controllers
{
	public class TemplatesController : Controller
	{
		public ActionResult LoginPartial()
		{
			return View();
		}
	}
}