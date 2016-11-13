using Microsoft.Owin;
using Microsoft.Owin.Logging;
using Microsoft.Owin.Security.Google;
using Microsoft.Owin.Security.OAuth;
using Owin;
using Phobos.Api.App_Start;
using Phobos.Api.Infrastructure.Configuration;

[assembly: OwinStartup(typeof(Phobos.Api.Startup))]

namespace Phobos.Api
{
	public class Startup
	{
		public void Configuration(IAppBuilder app)
		{
			// For more information on how to configure your application, visit http://go.microsoft.com/fwlink/?LinkID=316888

			CompositionRoot.CompositionRoot.Register(CompositionRoot.CompositionRoot.Container, app);

			ConfigureOAuth(app);

			WebApiConfig.Register(app);
		}

		public void ConfigureOAuth(IAppBuilder app)
		{
			IConfig config = CompositionRoot.CompositionRoot.Container.GetInstance<IConfig>();

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