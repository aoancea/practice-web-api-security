﻿using Microsoft.Owin;
using Microsoft.Owin.Logging;
using Microsoft.Owin.Security.Google;
using Microsoft.Owin.Security.OAuth;
using Owin;
using Phobos.Api.App_Start;
using Phobos.Api.Infrastructure;
using System;
using System.Web.Http;

[assembly: OwinStartup(typeof(Phobos.Api.Startup))]

namespace Phobos.Api
{
	public class Startup
	{
		public static OAuthBearerAuthenticationOptions OAuthBearerOptions { get; private set; }

		public void Configuration(IAppBuilder app)
		{
			// For more information on how to configure your application, visit http://go.microsoft.com/fwlink/?LinkID=316888

			HttpConfiguration config = new HttpConfiguration();

			ConfigureOAuth(app);

			WebApiConfig.Register(config);
			app.UseWebApi(config);
		}

		public void ConfigureOAuth(IAppBuilder app)
		{
			app.UseExternalSignInCookie(Microsoft.AspNet.Identity.DefaultAuthenticationTypes.ExternalCookie);

			OAuthAuthorizationServerOptions OAuthServerOptions = new OAuthAuthorizationServerOptions()
			{
				AllowInsecureHttp = true,
				TokenEndpointPath = new PathString("/token"),
				AccessTokenExpireTimeSpan = TimeSpan.FromMinutes(30),
				Provider = new SimpleOAuthAuthorizationServerProvider()
			};

			OAuthBearerOptions = new OAuthBearerAuthenticationOptions();

			app.UseOAuthAuthorizationServer(OAuthServerOptions);
			app.UseOAuthBearerAuthentication(OAuthBearerOptions);

			app.UseGoogleAuthentication(new GoogleOAuth2AuthenticationOptions()
			{
				ClientId = "xxx",
				ClientSecret = "xxx",
				Provider = new Infrastructure.GoogleOAuth2AuthenticationProvider()
			});

			app.SetLoggerFactory(new Infrastructure.Logger.LoggerFactory());
		}
	}
}