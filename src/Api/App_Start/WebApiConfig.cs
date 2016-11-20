using Owin;
using SimpleInjector.Integration.WebApi;
using System.Net.Http.Formatting;
using System.Web.Http;

namespace Phobos.Api.App_Start
{
	public class WebApiConfig
	{
		public static void Register(SimpleInjector.Container container, IAppBuilder app)
		{
			HttpConfiguration config = new HttpConfiguration();

			config.EnableCors(new Infrastructure.Providers.HttpCorsPolicyProvider());

			config.MapHttpAttributeRoutes();

			config.Routes.MapHttpRoute(
				name: "DefaultApi",
				routeTemplate: "api/v1/{controller}/{uid}",
				defaults: new { uid = RouteParameter.Optional }
			);

			config.Formatters.Clear();
			config.Formatters.Add(new JsonMediaTypeFormatter());

			config.Formatters.JsonFormatter.SerializerSettings.Formatting = Newtonsoft.Json.Formatting.Indented;
			config.Formatters.JsonFormatter.SerializerSettings.DateTimeZoneHandling = Newtonsoft.Json.DateTimeZoneHandling.Utc;

			config.Filters.Add(container.GetInstance<Infrastructure.Filters.IAttachIdentityFilter>());

			config.DependencyResolver = new SimpleInjectorWebApiDependencyResolver(container);

			app.UseWebApi(config);
		}
	}
}