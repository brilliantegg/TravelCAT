using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TravelCat.Models;
using TravelCat.ViewModels;

namespace TravelCat.Controllers
{
    public class web_activitiesController : Controller
    {
        private dbTravelCat db = new dbTravelCat();

        // GET: web_activities
        public ActionResult Index()
        {
            return View(db.activity.ToList());
        }

        // GET: web_activities/Details/5
        public ActionResult Details(string id)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            destinationsViewModel model = new destinationsViewModel()
            {
                activity = db.activity.Where(m => m.activity_id == id).FirstOrDefault(),
                comment = db.comment.Where(m => m.tourism_id == id).FirstOrDefault(),
                //message = db.message.Where(m => m.comment_id == comment.).FirstOrDefault(),
            };
            if (model == null)
            {
                return HttpNotFound();
            }
            ViewBag.id = id;
            return View(model);
        }


    }
}