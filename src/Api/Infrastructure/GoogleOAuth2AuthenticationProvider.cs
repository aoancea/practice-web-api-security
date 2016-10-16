using Microsoft.Owin.Security.Google;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Phobos.Api.Infrastructure
{
	public class GoogleOAuth2AuthenticationProvider : IGoogleOAuth2AuthenticationProvider
	{
		public void ApplyRedirect(GoogleOAuth2ApplyRedirectContext context)
		{
			context.Response.Redirect(context.RedirectUri + "&prompt=select_account&include_granted_scopes=true");
		}

		public Task Authenticated(GoogleOAuth2AuthenticatedContext context)
		{
			context.Identity.AddClaim(new Claim("ExternalAccessToken", context.AccessToken));
			return Task.FromResult<object>(null);
		}

		public Task ReturnEndpoint(GoogleOAuth2ReturnEndpointContext context)
		{
			return Task.FromResult<object>(null);
		}
	}
}