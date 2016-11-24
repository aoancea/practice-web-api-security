using Owin;

namespace Phobos.Web.CompositionRoot
{
	public static class CompositionRoot
	{
		public static void Register(SimpleInjector.Container container, IAppBuilder app)
		{
			container.Register<Infrastructure.Security.IAccessTokenSecureDataFormat>(() => new Infrastructure.Security.AccessTokenSecureDataFormat(app), SimpleInjector.Lifestyle.Singleton);
			
			container.Register<Infrastructure.Filters.IAttachIdentityFilter, Infrastructure.Filters.AttachIdentityFilter>();

			container.Verify();
		}
	}
}