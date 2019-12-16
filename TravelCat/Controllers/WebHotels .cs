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
    public class WebHotelsController : Controller
    {
        private dbTravelCat db = new dbTravelCat();

        // GET: web_activities
        public ActionResult Index()
        {
            return View(db.hotel.OrderByDescending(m => m.latitude).Take(100).ToList());
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
                hotel = db.hotel.Where(m => m.hotel_id == id).FirstOrDefault(),
                comment = db.comment.Where(m => m.tourism_id == id).OrderByDescending(m=>m.comment_date).ToList(),
                message = db.message.Where(m => m.tourism_id == id).OrderByDescending(m => m.msg_time).ToList(),
                comment_emoji_details = db.comment_emoji_details.ToList(),
                message_emoji_details = db.message_emoji_details.ToList(),
                member_profile = db.member_profile.ToList(),
                member = db.member.ToList(),
                collections_detail = db.collections_detail.Where(m => m.tourism_id == id).ToList(),
            };
            if (model == null)
            {
                return HttpNotFound();
            }
            ViewBag.tourismId = id;
            List<spot> spot = db.spot.Where(m => m.district == model.hotel.district).OrderByDescending(m => m.spot_id).Take(3).ToList();
            ViewBag.tourismId = id;
            ViewBag.spot = spot;
            return View(model);
        }     
        
    }
}