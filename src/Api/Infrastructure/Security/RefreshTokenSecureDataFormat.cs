using Microsoft.Owin.Security.DataProtection;

namespace Phobos.Api.Infrastructure.Security
{
	public interface IRefreshTokenSecureDataFormat : Microsoft.Owin.Security.ISecureDataFormat<Microsoft.Owin.Security.AuthenticationTicket>
	{ }

	public class RefreshTokenSecureDataFormat : Microsoft.Owin.Security.DataHandler.TicketDataFormat, IRefreshTokenSecureDataFormat
	{
		public RefreshTokenSecureDataFormat(Owin.IAppBuilder app)
			: base(app.CreateDataProtector(typeof(Microsoft.Owin.Security.OAuth.OAuthBearerAuthenticationMiddleware).Namespace, "Refresh_Token", "v1")) { }
	}
}