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
			// For more information on how to configure your application, visit http://go.microsoft.com/fwlink/?LinkID=316888

			CompositionRoot.CompositionRoot.Register(CompositionRoot.CompositionRoot.Container, app);

			OAuthConfig.Register(app);

			WebApiConfig.Register(app);
		}
	}
}