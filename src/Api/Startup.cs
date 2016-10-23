using Microsoft.Owin;
using Microsoft.Owin.Logging;
using Microsoft.Owin.Security.Google;
using Microsoft.Owin.Security.OAuth;
using Owin;
using Phobos.Api.App_Start;
using Phobos.Api.Infrastructure.Configuration;
using System;
using System.Web.Http;

[assembly: OwinStartup(typeof(Phobos.Api.Startup))]

namespace Phobos.Api
{
	public class Startup
	{
		public void Configuration(IAppBuilder app)
		{
			// For more information on how to configure your application, visit http://go.microsoft.com/fwlink/?LinkID=316888

			CompositionRoot.CompositionRoot.Register(CompositionRoot.CompositionRoot.Container, app);

			HttpConfiguration config = new HttpConfiguration();

			ConfigureOAuth(app);

			WebApiConfig.Register(config);
			app.UseWebApi(config);
		}

		public void ConfigureOAuth(IAppBuilder app)
		{
			IConfig config = CompositionRoot.CompositionRoot.Container.GetInstance<IConfig>();

			app.UseExternalSignInCookie(Microsoft.AspNet.Identity.DefaultAuthenticationTypes.ExternalCookie);

			OAuthAuthorizationServerOptions oAuthServerOptions = new OAuthAuthorizationServerOptions()
			{
				AllowInsecureHttp = true,
				TokenEndpointPath = new PathString("/token"),
				AccessTokenExpireTimeSpan = TimeSpan.FromMinutes(30),
				Provider = new Infrastructure.Providers.SimpleOAuthAuthorizationServerProvider(),
				AccessTokenFormat = CompositionRoot.CompositionRoot.Container.GetInstance<Microsoft.Owin.Security.ISecureDataFormat<Microsoft.Owin.Security.AuthenticationTicket>>()
			};

			OAuthBearerAuthenticationOptions oAuthBearerOptions = new OAuthBearerAuthenticationOptions()
			{
				AccessTokenFormat = CompositionRoot.CompositionRoot.Container.GetInstance<Microsoft.Owin.Security.ISecureDataFormat<Microsoft.Owin.Security.AuthenticationTicket>>()
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