using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security.DataProtection;
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

			container.Register<IAccessTokenSecureDataFormat>(() => new AccessTokenSecureDataFormat(app), SimpleInjector.Lifestyle.Singleton);
			container.Register<IRefreshTokenSecureDataFormat>(() => new RefreshTokenSecureDataFormat(app), SimpleInjector.Lifestyle.Singleton);

			container.Register<Infrastructure.Helpers.IAccessTokenHelper, Infrastructure.Helpers.AccessTokenHelper>();
			container.Register<Infrastructure.Helpers.IExternalLoginHelper, Infrastructure.Helpers.ExternalLoginHelper>();

			container.Verify();
		}

		public interface IAccessTokenSecureDataFormat : Microsoft.Owin.Security.ISecureDataFormat<Microsoft.Owin.Security.AuthenticationTicket>
		{ }

		public interface IRefreshTokenSecureDataFormat : Microsoft.Owin.Security.ISecureDataFormat<Microsoft.Owin.Security.AuthenticationTicket>
		{ }


		public class AccessTokenSecureDataFormat : Microsoft.Owin.Security.DataHandler.TicketDataFormat, IAccessTokenSecureDataFormat
		{
			public AccessTokenSecureDataFormat(IAppBuilder app)
				: base(app.CreateDataProtector(typeof(Microsoft.Owin.Security.OAuth.OAuthBearerAuthenticationMiddleware).Namespace, "Access_Token", "v1")) { }
		}

		public class RefreshTokenSecureDataFormat : Microsoft.Owin.Security.DataHandler.TicketDataFormat, IRefreshTokenSecureDataFormat
		{
			public RefreshTokenSecureDataFormat(IAppBuilder app)
				: base(app.CreateDataProtector(typeof(Microsoft.Owin.Security.OAuth.OAuthBearerAuthenticationMiddleware).Namespace, "Refresh_Token", "v1")) { }
		}
	}
}