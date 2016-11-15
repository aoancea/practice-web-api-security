using Microsoft.Owin;
using Microsoft.Owin.Logging;
using Microsoft.Owin.Security.OAuth;
using Owin;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

[assembly: OwinStartup(typeof(Phobos.Web.Startup))]

namespace Phobos.Web
{
	public class Startup
	{
		public void Configuration(IAppBuilder app)
		{
			SimpleInjector.Container container = new SimpleInjector.Container();

			CompositionRoot.CompositionRoot.Register(container, app);

			OAuthBearerAuthenticationOptions oAuthBearerOptions = new OAuthBearerAuthenticationOptions()
			{
				AccessTokenFormat = container.GetInstance<Infrastructure.Security.IAccessTokenSecureDataFormat>()
			};

			app.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions());

			app.SetLoggerFactory(new Infrastructure.Logger.LoggerFactory());

			AreaRegistration.RegisterAllAreas();
			FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
			RouteConfig.RegisterRoutes(RouteTable.Routes);
			BundleConfig.RegisterBundles(BundleTable.Bundles);
		}
	}
}