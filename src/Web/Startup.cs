using Microsoft.Owin;
using Owin;
using Phobos.Web.App_Start;
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

			OAuthConfig.Register(container, app);

			AreaRegistration.RegisterAllAreas();
			FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
			RouteConfig.RegisterRoutes(RouteTable.Routes);
			BundleConfig.RegisterBundles(BundleTable.Bundles);
		}
	}
}