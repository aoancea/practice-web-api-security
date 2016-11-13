using Microsoft.Owin;
using Owin;
using Phobos.Api.App_Start;

[assembly: OwinStartup(typeof(Phobos.Api.Startup))]

namespace Phobos.Api
{
	public class Startup
	{
		public void Configuration(IAppBuilder app)
		{
			SimpleInjector.Container container = new SimpleInjector.Container();

			CompositionRoot.CompositionRoot.Register(container, app);

			OAuthConfig.Register(container, app);

			WebApiConfig.Register(container, app);
		}
	}
}