using Microsoft.Owin.Security;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;

namespace Phobos.Api.Infrastructure
{
	public class ChallengeResult : IHttpActionResult
	{
		private readonly HttpRequestMessage request;
		private readonly string provider;
		private readonly string redirectUri;

		public ChallengeResult(HttpRequestMessage request, string provider, string redirectUri)
		{
			this.provider = provider;
			this.request = request;
			this.redirectUri = redirectUri;
		}

		public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
		{
			AuthenticationProperties properties = new AuthenticationProperties() { RedirectUri = redirectUri };

			request.GetOwinContext().Authentication.Challenge(properties, provider);

			HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.Unauthorized);
			response.RequestMessage = request;

			return Task.FromResult(response);
		}
	}
}