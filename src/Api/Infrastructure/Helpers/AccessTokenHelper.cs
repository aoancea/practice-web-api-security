using Microsoft.Owin.Security;
using System;
using System.Linq;
using System.Security.Claims;

namespace Phobos.Api.Infrastructure.Helpers
{
	public interface IAccessTokenHelper
	{
		AuthenticationTicket AccessTokenFromRefreshToken(AuthenticationTicket refreshTokenTicket);

		string RefreshTokenFromAccessToken(AuthenticationTicket accessTokenTicket);

		TimeSpan AccessTokenExpireTimeSpan();

		TimeSpan RefreshTokenExpireTimeSpan();
	}

	public class AccessTokenHelper : IAccessTokenHelper
	{
		private readonly Security.IAccessTokenSecureDataFormat accessTokenSecureDataFormat;
		private readonly Security.IRefreshTokenSecureDataFormat refreshTokenSecureDataFormat;

		public AccessTokenHelper(Security.IAccessTokenSecureDataFormat accessTokenSecureDataFormat, Security.IRefreshTokenSecureDataFormat refreshTokenSecureDataFormat)
		{
			this.accessTokenSecureDataFormat = accessTokenSecureDataFormat;
			this.refreshTokenSecureDataFormat = refreshTokenSecureDataFormat;
		}

		public AuthenticationTicket AccessTokenFromRefreshToken(AuthenticationTicket refreshTokenTicket)
		{
			Claim accessTokenClaim = refreshTokenTicket.Identity.Claims.First(claim => claim.Type == TokenIdentifiers.Access_Token);

			string access_token = accessTokenClaim.Value;

			AuthenticationTicket ticket = accessTokenSecureDataFormat.Unprotect(access_token);

			AuthenticationProperties newProperties = new AuthenticationProperties()
			{
				IssuedUtc = DateTime.UtcNow,
				ExpiresUtc = DateTime.UtcNow.Add(AccessTokenExpireTimeSpan())
			};

			ClaimsIdentity newIdentity = new ClaimsIdentity(ticket.Identity);

			AuthenticationTicket newTicket = new AuthenticationTicket(newIdentity, newProperties);

			return newTicket;
		}

		public string RefreshTokenFromAccessToken(AuthenticationTicket accessTokenTicket)
		{
			string access_token = accessTokenSecureDataFormat.Protect(accessTokenTicket);

			Claim accessTokenClaim = new Claim(TokenIdentifiers.Access_Token, access_token);

			ClaimsIdentity identity = new ClaimsIdentity(new Claim[1] { accessTokenClaim }, accessTokenTicket.Identity.AuthenticationType);

			AuthenticationProperties properties = new AuthenticationProperties()
			{
				IssuedUtc = DateTime.UtcNow,
				ExpiresUtc = DateTime.UtcNow.Add(RefreshTokenExpireTimeSpan())
			};

			AuthenticationTicket refreshTokenTicket = new AuthenticationTicket(identity, properties);

			return refreshTokenSecureDataFormat.Protect(refreshTokenTicket);
		}

		public TimeSpan AccessTokenExpireTimeSpan()
		{
			return TimeSpan.FromMinutes(30);
		}

		public TimeSpan RefreshTokenExpireTimeSpan()
		{
			return TimeSpan.FromMinutes(30);
		}
	}

	public static class TokenIdentifiers
	{
		public const string Access_Token = "claim.access_token";
	}
}