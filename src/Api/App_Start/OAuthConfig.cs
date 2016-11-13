using Microsoft.Owin;
using Microsoft.Owin.Logging;
using Microsoft.Owin.Security.Google;
using Microsoft.Owin.Security.OAuth;
using Owin;

namespace Phobos.Api.App_Start
{
	public class OAuthConfig
	{
		public static void Register(IAppBuilder app)
		{
			Infrastructure.Configuration.IConfig config = CompositionRoot.CompositionRoot.Container.GetInstance<Infrastructure.Configuration.IConfig>();

			app.UseExternalSignInCookie(Microsoft.AspNet.Identity.DefaultAuthenticationTypes.ExternalCookie);

			OAuthAuthorizationServerOptions oAuthServerOptions = new OAuthAuthorizationServerOptions()
			{
				AllowInsecureHttp = true,
				TokenEndpointPath = new PathString("/token"),
				AccessTokenExpireTimeSpan = CompositionRoot.CompositionRoot.Container.GetInstance<Infrastructure.Helpers.IAccessTokenHelper>().AccessTokenExpireTimeSpan(),
				Provider = new Infrastructure.Providers.SimpleOAuthAuthorizationServerProvider(CompositionRoot.CompositionRoot.Container.GetInstance<Infrastructure.Helpers.IAccessTokenHelper>()),
				RefreshTokenProvider = new Infrastructure.Providers.SimpleAuthenticationTokenProvider(CompositionRoot.CompositionRoot.Container.GetInstance<Infrastructure.Helpers.IAccessTokenHelper>()),
				AccessTokenFormat = CompositionRoot.CompositionRoot.Container.GetInstance<Infrastructure.Security.IAccessTokenSecureDataFormat>(),
				RefreshTokenFormat = CompositionRoot.CompositionRoot.Container.GetInstance<Infrastructure.Security.IRefreshTokenSecureDataFormat>()
			};

			OAuthBearerAuthenticationOptions oAuthBearerOptions = new OAuthBearerAuthenticationOptions()
			{
				AccessTokenFormat = CompositionRoot.CompositionRoot.Container.GetInstance<Infrastructure.Security.IAccessTokenSecureDataFormat>()
			};

			app.UseOAuthAuthorizationServer(oAuthServerOptions);
			app.UseOAuthBearerAuthentication(oAuthBearerOptions);

			app.UseGoogleAuthentication(new GoogleOAuth2AuthenticationOptions()
			{
				ClientId = config.GoogleClientId,
				ClientSecret = config.GoogleClientSecret,
				Provider = new Infrastructure.Providers.GoogleOAuth2AuthenticationProvider()
			});

			app.SetLoggerFactory(new Infrastructure.Logger.LoggerFactory());
		}
	}
}