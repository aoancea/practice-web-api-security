using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(Phobos.Web.Startup))]

namespace Phobos.Web
{
	public class Startup
	{
		public void Configuration(IAppBuilder app)
		{
			ConfigureAuth(app);
		}


		public void ConfigureAuth(IAppBuilder app)
		{
		}
	}
}