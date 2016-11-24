using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using Newtonsoft.Json.Linq;
using System;
using System.Security.Claims;

namespace Phobos.Api.Infrastructure.Helpers
{
	public interface IExternalLoginHelper
	{
		JObject ExternalLoginToken(ClaimsPrincipal externalIdentity);
	}

	public class ExternalLoginHelper : IExternalLoginHelper
	{
		private readonly Security.IAccessTokenSecureDataFormat accessTokenSecureDataFormat;
		private readonly IAccessTokenHelper accessTokenHelper;
		private readonly UserManager<IdentityUser> userManager;

		public ExternalLoginHelper(
			Security.IAccessTokenSecureDataFormat accessTokenSecureDataFormat,
			IAccessTokenHelper accessTokenHelper,
			UserManager<IdentityUser> userManager)
		{
			this.accessTokenSecureDataFormat = accessTokenSecureDataFormat;
			this.accessTokenHelper = accessTokenHelper;
			this.userManager = userManager;
		}

		public JObject ExternalLoginToken(ClaimsPrincipal externalClaimsIdentity)
		{
			TimeSpan tokenExpiration = accessTokenHelper.AccessTokenExpireTimeSpan();

			ExternalLoginModel externalLoginModel = FromExternalClaimsIdentity(externalClaimsIdentity);

			IdentityUser identityUser = userManager.Find(new UserLoginInfo(externalLoginModel.LoginProvider, externalLoginModel.ProviderKey));

			if (identityUser == null)
			{
				identityUser = new IdentityUser(externalLoginModel.Email) { Email = externalLoginModel.Email };

				userManager.Create(identityUser);

				userManager.AddLogin(identityUser.Id, new UserLoginInfo(externalLoginModel.LoginProvider, externalLoginModel.ProviderKey));
			}

			ClaimsIdentity claimsIdentity = GenerateClaimsIdentity(externalLoginModel, identityUser);

			AuthenticationProperties properties = new AuthenticationProperties()
			{
				IssuedUtc = DateTime.UtcNow,
				ExpiresUtc = DateTime.UtcNow.Add(tokenExpiration)
			};

			AuthenticationTicket ticket = new AuthenticationTicket(claimsIdentity, properties);

			string access_token = accessTokenSecureDataFormat.Protect(ticket);

			return new JObject(
				new JProperty("access_token", access_token),
				new JProperty("token_type", "bearer"),
				new JProperty("expires_in", tokenExpiration.TotalSeconds.ToString()),
				new JProperty(".issued", ticket.Properties.IssuedUtc.ToString()),
				new JProperty(".expires", ticket.Properties.ExpiresUtc.ToString())
			);
		}

		private ClaimsIdentity GenerateClaimsIdentity(ExternalLoginModel externalLoginModel, IdentityUser identityUser)
		{
			Identity.Identity identity = new Identity.Identity()
			{
				UID = new Guid(identityUser.Id),
				Name = externalLoginModel.Email
			};

			ClaimsIdentity claimsIdentity = new ClaimsIdentity(Microsoft.Owin.Security.OAuth.OAuthDefaults.AuthenticationType);
			claimsIdentity.AddClaim(new Claim(Identity.IdentityConstants.IdentityClaimKey, Newtonsoft.Json.JsonConvert.SerializeObject(identity)));

			return claimsIdentity;
		}

		private ExternalLoginModel FromExternalClaimsIdentity(ClaimsPrincipal externalClaimsIdentity)
		{
			Claim providerKeyClaim = externalClaimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

			Claim emailClaim = externalClaimsIdentity.FindFirst(ClaimTypes.Email);

			return new ExternalLoginModel()
			{
				ProviderKey = providerKeyClaim.Value,
				LoginProvider = providerKeyClaim.Issuer,
				Email = emailClaim.Value
			};
		}

		private class ExternalLoginModel
		{
			public string ProviderKey { get; set; }

			public string LoginProvider { get; set; }

			public string Email { get; set; }
		}
	}
}