using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using Newtonsoft.Json.Linq;
using Phobos.Api.Context;
using Phobos.Api.Infrastructure;
using Phobos.Api.Models;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Http;

namespace Phobos.Api.Controllers
{
	public class AccountController : ApiController
	{
		private static string[] requiredClaimTypes = new string[] { ClaimTypes.Name, "sub", "role" };

		private readonly ApplicationDbContext applicationDbContext;
		private readonly UserManager<IdentityUser> userManager;

		public AccountController()
		{
			applicationDbContext = new ApplicationDbContext();
			userManager = new UserManager<IdentityUser>(new UserStore<IdentityUser>(applicationDbContext));
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
		public async Task<IHttpActionResult> ExternalLogin(string provider, string error = null)
		{
			if (!User.Identity.IsAuthenticated)
				return new ChallengeResult(Request, provider, string.Empty);

			TimeSpan tokenExpiration = TimeSpan.FromDays(1);

			ClaimsIdentity identity = PrincipalToClaimsIdentityConverter((ClaimsPrincipal)User);
			AuthenticationProperties properties = new AuthenticationProperties()
			{
				IssuedUtc = DateTime.UtcNow,
				ExpiresUtc = DateTime.UtcNow.Add(tokenExpiration)
			};

			AuthenticationTicket ticket = new AuthenticationTicket(identity, properties);

			string access_token = Startup.OAuthBearerOptions.AccessTokenFormat.Protect(ticket);

			JObject tokenResponse = new JObject(
				new JProperty("access_token", access_token),
				new JProperty("token_type", "bearer"),
				new JProperty("expires_in", tokenExpiration.TotalSeconds.ToString()),
				new JProperty(".issued", ticket.Properties.IssuedUtc.ToString()),
				new JProperty(".expires", ticket.Properties.ExpiresUtc.ToString())
			);

			return await Task.FromResult(Ok(tokenResponse));
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

		private ClaimsIdentity PrincipalToClaimsIdentityConverter(ClaimsPrincipal principal)
		{
			ClaimsIdentity claimsIdentity = new ClaimsIdentity(Microsoft.Owin.Security.OAuth.OAuthDefaults.AuthenticationType);
			claimsIdentity.AddClaims(principal.Claims.Where(claim => requiredClaimTypes.Contains(claim.Type)));

			return claimsIdentity;
		}
	}
}