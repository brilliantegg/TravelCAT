using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TravelCat.Models;
using TravelCat.ViewModels;

namespace TravelCat.Controllers
{
    public class HomeController : Controller
    {
        dbTravelCat db = new dbTravelCat();


        // GET: Home
        public ActionResult Index(string account)
        {
            WebIndexViewModel model = new WebIndexViewModel()
            {
                activity = db.activity.OrderBy(m => Guid.NewGuid()).ToList(),
                hotel=db.hotel.OrderBy(m => Guid.NewGuid()).ToList(),
                restaurant=db.restaurant.OrderBy(m => Guid.NewGuid()).ToList(),
                spot=db.spot.OrderBy(m => Guid.NewGuid()).ToList(),
                member=db.member.Where(m=>m.member_account==account).FirstOrDefault(),
                comment=db.comment.OrderBy(m=>m.comment_date).ToList()
            };

            return View(model);
        }
    }
}