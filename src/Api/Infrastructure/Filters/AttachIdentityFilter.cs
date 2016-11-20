using Newtonsoft.Json;
using System;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace Phobos.Api.Infrastructure.Filters
{
	public class AttachIdentityFilter : IActionFilter
	{
		public Task<HttpResponseMessage> ExecuteActionFilterAsync(HttpActionContext actionContext, CancellationToken cancellationToken, Func<Task<HttpResponseMessage>> continuation)
		{
			System.Security.Claims.ClaimsPrincipal principal = (System.Security.Claims.ClaimsPrincipal)actionContext.Request.GetRequestContext().Principal;

			Identity.Identity Identity = JsonConvert.DeserializeObject<Identity.Identity>(principal.Claims.Single(x => x.Type == Infrastructure.Identity.IdentityConstants.IdentityClaimKey).Value);

			actionContext.Request.Properties.Add(Infrastructure.Identity.IdentityConstants.IdentityRequestKey, Identity);

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