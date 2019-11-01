using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TravelCat.Controllers
{
    public class destinationsController : Controller
    {
        // GET: destinations
        public ActionResult Index()
        {
            return View();
        }
    }
}