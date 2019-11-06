using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TravelCat.Models;

namespace TravelCat.Controllers
{
    public class AdminController : Controller
    {
        dbTravelCat db = new dbTravelCat();

        // GET: Admin
        public ActionResult Index()
        {
            return View(db.admin.ToList());
        }
        public ActionResult Home()
        {
            return View();
        }
    }
}