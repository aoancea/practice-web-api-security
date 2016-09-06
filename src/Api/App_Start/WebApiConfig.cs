using System.Web.Http;

namespace Phobos.Api.App_Start
{
    public class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/v1/{controller}/{uid}",
                defaults: new { uid = RouteParameter.Optional }
            );
        }
    }
}