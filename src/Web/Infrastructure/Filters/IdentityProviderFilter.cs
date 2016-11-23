using System.Web.Mvc;

namespace Phobos.Web.Infrastructure.Filters
{
	public interface IIdentityProviderFilter : IActionFilter
	{ }

	public class IdentityProviderFilter : IIdentityProviderFilter
	{
		private readonly Identity.IIdentityProvider identityProvider;

		public IdentityProviderFilter(Identity.IIdentityProvider identityProvider)
		{
			this.identityProvider = identityProvider;
		}

		public void OnActionExecuted(ActionExecutedContext filterContext)
		{
			filterContext.Controller.ViewBag.Identity = identityProvider.Identity(filterContext.RequestContext.HttpContext);
		}

		public void OnActionExecuting(ActionExecutingContext filterContext)
		{

		}
	}
}