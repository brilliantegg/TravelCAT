using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using System.Web.SessionState;
using System.Web.Http;
using System.Data.Entity;





namespace TravelCat
{
    //public class Global : System.Web.HttpApplication
    //{
    //    protected void Application_Start(object sender, EventArgs e)
    //    {
    //        AreaRegistration.RegisterAllAreas();
    //        WebApiConfig.Register(System.Web.Http.GlobalConfiguration.Configuration);
    //        RouteConfig.RegisterRoutes(RouteTable.Routes);
    //        GlobalConfiguration.Configure(WebApiConfig.Register);
    //    }
    //}
    public class MvcApplication : System.Web.HttpApplication
    {

        protected void Application_Start(object sender, EventArgs e)
        {
            AreaRegistration.RegisterAllAreas();
            WebApiConfig.Register(System.Web.Http.GlobalConfiguration.Configuration);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            //System.Web.Http.GlobalConfiguration.Configure(WebApiConfig.Register);

            System.Web.Http.GlobalConfiguration.Configuration.EnsureInitialized();


            System.Web.Http.GlobalConfiguration.Configuration.Formatters.JsonFormatter.SerializerSettings
            .ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
            System.Web.Http.GlobalConfiguration.Configuration.Formatters
                .Remove(System.Web.Http.GlobalConfiguration.Configuration.Formatters.XmlFormatter);

        }

    }
}
