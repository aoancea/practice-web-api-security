using System.Web.Mvc;

namespace Phobos.Web
{
	public class FilterConfig
	{
		public static void RegisterGlobalFilters(GlobalFilterCollection filters, SimpleInjector.Container container)
		{
			filters.Add(container.GetInstance<Infrastructure.Filters.IIdentityProviderFilter>());
			filters.Add(container.GetInstance<Infrastructure.Filters.IAttachIdentityFilter>());
			filters.Add(new HandleErrorAttribute());
		}
	}
}