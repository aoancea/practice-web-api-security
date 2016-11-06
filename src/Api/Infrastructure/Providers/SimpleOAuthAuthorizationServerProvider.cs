﻿using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OAuth;
using Phobos.Api.Context;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Phobos.Api.Infrastructure.Providers
{
	public class SimpleOAuthAuthorizationServerProvider : OAuthAuthorizationServerProvider
	{
		private readonly Helpers.IAccessTokenHelper accessTokenHelper;

		public SimpleOAuthAuthorizationServerProvider(Helpers.IAccessTokenHelper accessTokenHelper)
		{
			this.accessTokenHelper = accessTokenHelper;
		}

		public override Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
		{
			context.Validated();

			return Task.FromResult<object>(null);
		}

		public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
		{
			using (ApplicationDbContext dbContext = new ApplicationDbContext())
			{
				var userManager = new UserManager<IdentityUser>(new UserStore<IdentityUser>(dbContext));

				IdentityUser user = await userManager.FindAsync(context.UserName, context.Password);

				if (user == null)
				{
					context.SetError("invalid_grant", "The user name or password is incorrect.");
					return;
				}
			}

			ClaimsIdentity identity = new ClaimsIdentity(context.Options.AuthenticationType);
			identity.AddClaim(new Claim(ClaimTypes.Name, context.UserName));
			identity.AddClaim(new Claim("sub", context.UserName));
			identity.AddClaim(new Claim("role", "user"));

			var ticket = new AuthenticationTicket(identity, new AuthenticationProperties());
			context.Validated(ticket);
		}

		public override Task TokenEndpoint(OAuthTokenEndpointContext context)
		{
			foreach (KeyValuePair<string, string> property in context.Properties.Dictionary)
			{
				context.AdditionalResponseParameters.Add(property.Key, property.Value);
			}

			return Task.FromResult<object>(null);
		}

		public override Task GrantRefreshToken(OAuthGrantRefreshTokenContext context)
		{
			// issue access_token based on refresh_token
			// we will get the access_token from a claim that's on the refresh_token

			AuthenticationTicket ticket = accessTokenHelper.AccessTokenFromRefreshToken(context.Ticket);

			context.Validated(ticket);

			return Task.FromResult<object>(null);
		}
	}
}