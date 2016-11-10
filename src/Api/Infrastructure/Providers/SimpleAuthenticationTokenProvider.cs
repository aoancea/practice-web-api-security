using Microsoft.Owin.Security.Infrastructure;
using System;
using System.Threading.Tasks;

namespace Phobos.Api.Infrastructure.Providers
{
	public class SimpleAuthenticationTokenProvider : IAuthenticationTokenProvider
	{
		private readonly Helpers.IAccessTokenHelper accessTokenHelper;

		public SimpleAuthenticationTokenProvider(Helpers.IAccessTokenHelper accessTokenHelper)
		{
			this.accessTokenHelper = accessTokenHelper;
		}

		public Task CreateAsync(AuthenticationTokenCreateContext context)
		{
			// create refresh_token from access_token

			string refresh_token = accessTokenHelper.RefreshTokenFromAccessToken(context.Ticket);

			context.SetToken(refresh_token);

			return Task.FromResult<object>(null);
		}

		public Task ReceiveAsync(AuthenticationTokenReceiveContext context)
		{
			// here we simply get the refresh_token ticket

			context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { "http://localhost:63332" });

			context.DeserializeTicket(context.Token);

			return Task.FromResult<object>(null);
		}

		public void Create(AuthenticationTokenCreateContext context)
		{
			throw new NotImplementedException();
		}

		public void Receive(AuthenticationTokenReceiveContext context)
		{
			throw new NotImplementedException();
		}
	}
}