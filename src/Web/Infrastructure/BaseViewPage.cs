namespace Phobos.Web.Infrastructure
{
	public abstract class BaseViewPage<TModel> : System.Web.Mvc.WebViewPage<TModel>
	{
		public Identity.Identity Identity { get { return ViewBag.Identity; } }
	}
}