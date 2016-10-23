using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Phobos.Api.Context;
using SimpleInjector.Integration.WebApi;

namespace Phobos.Api.CompositionRoot
{
	public static class CompositionRoot
	{
		public static readonly SimpleInjector.Container Container = new SimpleInjector.Container();

		public static void Register(SimpleInjector.Container container)
		{
			container.Options.DefaultScopedLifestyle = new WebApiRequestLifestyle();

			container.Register(typeof(ApplicationDbContext), () => new ApplicationDbContext(), SimpleInjector.Lifestyle.Scoped);
			container.Register(typeof(UserManager<IdentityUser>), () => new UserManager<IdentityUser>(new UserStore<IdentityUser>(container.GetInstance<ApplicationDbContext>())), SimpleInjector.Lifestyle.Scoped);

			container.Register<Infrastructure.IExternalLoginHelper, Infrastructure.ExternalLoginHelper>();

			container.Verify();
		}
	}
}