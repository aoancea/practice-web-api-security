using Newtonsoft.Json;
using Phobos.Web.Infrastructure.Identity;
using System.Linq;
using System.Web.Mvc;

namespace Phobos.Web.Infrastructure.Filters
{
	public interface IAttachIdentityFilter : IActionFilter
	{
	}

	public class AttachIdentityFilter : IAttachIdentityFilter
	{
		public void OnActionExecuted(ActionExecutedContext filterContext)
		{
			System.Security.Claims.ClaimsPrincipal principal = filterContext.HttpContext.User as System.Security.Claims.ClaimsPrincipal;

			System.Security.Claims.Claim identityClaim = principal != null ? principal.Claims.SingleOrDefault(x => x.Type == IdentityConstants.IdentityClaimKey) : null;

			if (identityClaim != null)
			{
				Identity.Identity identity = JsonConvert.DeserializeObject<Identity.Identity>(identityClaim.Value);

				filterContext.HttpContext.AddIdentity(identity);
			}
		}

		public void OnActionExecuting(ActionExecutingContext filterContext)
		{
		}
	}
}