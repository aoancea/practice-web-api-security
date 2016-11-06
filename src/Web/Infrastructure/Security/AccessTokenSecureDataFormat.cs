using Microsoft.Owin.Security.DataProtection;

namespace Phobos.Web.Infrastructure.Security
{
	public interface IAccessTokenSecureDataFormat : Microsoft.Owin.Security.ISecureDataFormat<Microsoft.Owin.Security.AuthenticationTicket>
	{ }

	public class AccessTokenSecureDataFormat : Microsoft.Owin.Security.DataHandler.TicketDataFormat, IAccessTokenSecureDataFormat
	{
		public AccessTokenSecureDataFormat(Owin.IAppBuilder app)
			: base(app.CreateDataProtector(typeof(Microsoft.Owin.Security.OAuth.OAuthBearerAuthenticationMiddleware).Namespace, "Access_Token", "v1")) { }
	}
}