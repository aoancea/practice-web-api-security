using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Newtonsoft.Json.Linq;
using Phobos.Api.Infrastructure;
using Phobos.Api.Models;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace Phobos.Api.Controllers
{
	public class AccountController : ApiController
	{
		private readonly UserManager<IdentityUser> userManager;
		private readonly Infrastructure.Helpers.IExternalLoginHelper externalLoginHelper;

		public AccountController(UserManager<IdentityUser> userManager, Infrastructure.Helpers.IExternalLoginHelper externalLoginHelper)
		{
			this.userManager = userManager;
			this.externalLoginHelper = externalLoginHelper;
		}

		[HttpPost]
		public async Task<IHttpActionResult> Register(User user)
		{
			if (ModelState.IsValid)
			{
				IdentityUser identityUser = new IdentityUser
				{
					UserName = user.UserName,

				};

				IdentityResult result = await userManager.CreateAsync(identityUser, user.Password);

				IHttpActionResult httpResult = GetErrorResult(result);

				if (httpResult != null)
					return httpResult;

				return Ok();
			}

			return BadRequest(ModelState);
		}

		[HttpGet]
		[HostAuthentication(DefaultAuthenticationTypes.ExternalCookie)]
		public async Task<IHttpActionResult> ExternalLogin(string provider, string redirect_uri, string error = null)
		{
			if (!User.Identity.IsAuthenticated)
				return new ChallengeResult(Request, provider, string.Empty);

			JObject tokenResponse = externalLoginHelper.ExternalLoginToken((ClaimsPrincipal)User);

			if (string.IsNullOrEmpty(redirect_uri))
				return await Task.FromResult(Ok(tokenResponse));
			else
				return await Task.FromResult(Redirect(redirect_uri + "#" + ToUriString(tokenResponse)));
		}

		private IHttpActionResult GetErrorResult(IdentityResult result)
		{
			if (result == null)
			{
				return InternalServerError();
			}

			if (!result.Succeeded)
			{
				if (result.Errors != null)
				{
					foreach (string error in result.Errors)
					{
						ModelState.AddModelError("", error);
					}
				}

				if (ModelState.IsValid)
				{
					// No ModelState errors are available to send, so just return an empty BadRequest.
					return BadRequest();
				}

				return BadRequest(ModelState);
			}

			return null;
		}


		private string ToUriString(JObject jObject)
		{
			StringBuilder sb = new StringBuilder();

			foreach (KeyValuePair<string, JToken> prop in jObject)
			{
				sb.Append(string.Format("{0}={1}", prop.Key, prop.Value.ToString()));
				sb.Append("&");
			}

			return sb.ToString();
		}
	}
}