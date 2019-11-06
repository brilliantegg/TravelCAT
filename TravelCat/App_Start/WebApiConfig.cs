using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace TravelCat
{
    public static class WebApiConfig
    {


        public static void Register(HttpConfiguration config)
        {
            System.Web.Http.GlobalConfiguration.Configuration.Formatters.XmlFormatter.SupportedMediaTypes.Clear();


            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
