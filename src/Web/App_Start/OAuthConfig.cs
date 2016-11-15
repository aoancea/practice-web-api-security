using Microsoft.Owin.Logging;
using Microsoft.Owin.Security.OAuth;
using Owin;

namespace Phobos.Web.App_Start
{
	public class OAuthConfig
	{
		public static void Register(SimpleInjector.Container container, IAppBuilder app)
		{
			OAuthBearerAuthenticationOptions oAuthBearerOptions = new OAuthBearerAuthenticationOptions()
			{
				AccessTokenFormat = container.GetInstance<Infrastructure.Security.IAccessTokenSecureDataFormat>()
			};

			app.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions());

			app.SetLoggerFactory(new Infrastructure.Logger.LoggerFactory());
		}
	}
}