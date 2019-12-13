using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TravelCat
{
    public class LoginCheck: ActionFilterAttribute
    {
        void Login(HttpContext context)
        {
            if (context.Session["id"] == null)
                context.Response.Redirect("/Loginadmin/Index"); //寫相對路徑
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            Login(HttpContext.Current);
        }
    }
}