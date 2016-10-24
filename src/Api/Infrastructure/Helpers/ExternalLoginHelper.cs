﻿using Microsoft.Owin.Security;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using System.Security.Claims;

namespace Phobos.Api.Infrastructure.Helpers
{
	public interface IExternalLoginHelper
	{
		JObject ExternalLoginToken(ClaimsPrincipal principal);
	}

	public class ExternalLoginHelper : IExternalLoginHelper
	{
		private static string[] requiredClaimTypes = new string[] { ClaimTypes.Name, "sub", "role" };

		private readonly ISecureDataFormat<AuthenticationTicket> accessTokenFormat;
		private readonly IAccessTokenHelper accessTokenHelper;

		public ExternalLoginHelper(ISecureDataFormat<AuthenticationTicket> accessTokenFormat, IAccessTokenHelper accessTokenHelper)
		{
			this.accessTokenFormat = accessTokenFormat;
			this.accessTokenHelper = accessTokenHelper;
		}

		public JObject ExternalLoginToken(ClaimsPrincipal principal)
		{
			TimeSpan tokenExpiration = accessTokenHelper.AccessTokenExpireTimeSpan();

			ClaimsIdentity identity = ConvertPrincipalToClaimsIdentity(principal);

			AuthenticationProperties properties = new AuthenticationProperties()
			{
				IssuedUtc = DateTime.UtcNow,
				ExpiresUtc = DateTime.UtcNow.Add(tokenExpiration)
			};

			AuthenticationTicket ticket = new AuthenticationTicket(identity, properties);

			string access_token = accessTokenFormat.Protect(ticket);

			return new JObject(
				new JProperty("access_token", access_token),
				new JProperty("token_type", "bearer"),
				new JProperty("expires_in", tokenExpiration.TotalSeconds.ToString()),
				new JProperty(".issued", ticket.Properties.IssuedUtc.ToString()),
				new JProperty(".expires", ticket.Properties.ExpiresUtc.ToString())
			);
		}


		private ClaimsIdentity ConvertPrincipalToClaimsIdentity(ClaimsPrincipal principal)
		{
			ClaimsIdentity claimsIdentity = new ClaimsIdentity(Microsoft.Owin.Security.OAuth.OAuthDefaults.AuthenticationType);
			claimsIdentity.AddClaims(principal.Claims.Where(claim => requiredClaimTypes.Contains(claim.Type)));

			return claimsIdentity;
		}
	}
}