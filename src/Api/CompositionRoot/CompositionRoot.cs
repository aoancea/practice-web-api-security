using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Owin;
using Phobos.Api.Context;
using Phobos.Api.Infrastructure.Configuration;
using SimpleInjector.Integration.WebApi;

namespace Phobos.Api.CompositionRoot
{
	public static class CompositionRoot
	{
		public static readonly SimpleInjector.Container Container = new SimpleInjector.Container();

		public static void Register(SimpleInjector.Container container, IAppBuilder app)
		{
			container.Options.DefaultScopedLifestyle = new WebApiRequestLifestyle();

			container.Register<IConfig, Config>(SimpleInjector.Lifestyle.Singleton);

			container.Register(typeof(ApplicationDbContext), () => new ApplicationDbContext(), SimpleInjector.Lifestyle.Scoped);
			container.Register(typeof(UserManager<IdentityUser>), () => new UserManager<IdentityUser>(new UserStore<IdentityUser>(container.GetInstance<ApplicationDbContext>())), SimpleInjector.Lifestyle.Scoped);

			container.Register<Infrastructure.Security.IAccessTokenSecureDataFormat>(() => new Infrastructure.Security.AccessTokenSecureDataFormat(app), SimpleInjector.Lifestyle.Singleton);
			container.Register<Infrastructure.Security.IRefreshTokenSecureDataFormat>(() => new Infrastructure.Security.RefreshTokenSecureDataFormat(app), SimpleInjector.Lifestyle.Singleton);

			container.Register<Infrastructure.Helpers.IAccessTokenHelper, Infrastructure.Helpers.AccessTokenHelper>();
			container.Register<Infrastructure.Helpers.IExternalLoginHelper, Infrastructure.Helpers.ExternalLoginHelper>();

			container.Verify();
		}
	}
}