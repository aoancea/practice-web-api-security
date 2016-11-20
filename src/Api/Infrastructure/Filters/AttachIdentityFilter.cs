using Newtonsoft.Json;
using System;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Controllers;

namespace Phobos.Api.Infrastructure.Filters
{
	public interface IAttachIdentityFilter : System.Web.Http.Filters.IActionFilter
	{
	}

	public class AttachIdentityFilter : IAttachIdentityFilter
	{
		public Task<HttpResponseMessage> ExecuteActionFilterAsync(HttpActionContext actionContext, CancellationToken cancellationToken, Func<Task<HttpResponseMessage>> continuation)
		{
			System.Security.Claims.ClaimsPrincipal principal = actionContext.Request.GetRequestContext().Principal as System.Security.Claims.ClaimsPrincipal;

			System.Security.Claims.Claim identityClaim = principal != null ? principal.Claims.SingleOrDefault(x => x.Type == Identity.IdentityConstants.IdentityClaimKey) : null;

			if (identityClaim != null)
			{
				Identity.Identity Identity = JsonConvert.DeserializeObject<Identity.Identity>(identityClaim.Value);

				actionContext.Request.Properties.Add(Infrastructure.Identity.IdentityConstants.IdentityRequestKey, Identity);
			}

			return continuation();
		}

		public bool AllowMultiple
		{
			get
			{
				return false;
			}
		}
	}
}