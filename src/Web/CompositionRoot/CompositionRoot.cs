using Owin;

namespace Phobos.Web.CompositionRoot
{
	public static class CompositionRoot
	{
		public static void Register(SimpleInjector.Container container, IAppBuilder app)
		{
			container.Register<Infrastructure.Security.IAccessTokenSecureDataFormat>(() => new Infrastructure.Security.AccessTokenSecureDataFormat(app), SimpleInjector.Lifestyle.Singleton);
			
			container.Register<Infrastructure.Identity.IIdentityProvider, Infrastructure.Identity.IdentityProvider>();

			container.Register<Infrastructure.Filters.IAttachIdentityFilter, Infrastructure.Filters.AttachIdentityFilter>();
			container.Register<Infrastructure.Filters.IIdentityProviderFilter, Infrastructure.Filters.IdentityProviderFilter>();

			container.Verify();
		}
	}
}