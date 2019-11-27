using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TravelCat.Models;
using TravelCat.ViewModels;

namespace TravelCat.Controllers
{
    public class SearchController : Controller
    {
        dbTravelCat db = new dbTravelCat();
        int pageSize = 15;


        //GET: Search
        public ActionResult Index(string q = null, int page = 1)
        {
            ViewBag.q = q;
            Session["pg"] = page;
            SearchViewModel model = new SearchViewModel();

            if (!String.IsNullOrEmpty(q))
            {
                 model = new SearchViewModel
                {
                    activity = db.activity.Where(s => s.activity_intro.Contains(q) || s.activity_title.Contains(q) || s.city.Contains(q) || s.district.Contains(q)).ToList(),
                    hotel = db.hotel.Where(s => s.hotel_intro.Contains(q) || s.hotel_title.Contains(q) || s.city.Contains(q) || s.district.Contains(q)).ToList(),
                    restaurant = db.restaurant.Where(s => s.restaurant_intro.Contains(q) || s.restaurant_title.Contains(q) || s.city.Contains(q) || s.district.Contains(q)).ToList(),
                    spot = db.spot.Where(s => s.spot_intro.Contains(q) || s.spot_title.Contains(q) || s.city.Contains(q) || s.district.Contains(q)).ToList()
                };
            }
            else
            {
                model = new SearchViewModel
                {
                    activity = db.activity.OrderBy(s => s.activity_id).ToList(),
                    hotel = db.hotel.OrderBy(s => s.hotel_id).ToList(),
                    restaurant = db.restaurant.OrderBy(s => s.restaurant_id).ToList(),
                    spot = db.spot.OrderBy(s => s.spot_id).ToList()
                };
            }
            return View(model);
        }
        public ActionResult Activity(string q = null, int page = 1)
        {
            ViewBag.q = q;
            Session["pg"] = page;
            if (!String.IsNullOrEmpty(q))
            {
                var activity = db.activity.Where(s => s.activity_intro.Contains(q) || s.activity_title.Contains(q) || s.city.Contains(q) || s.district.Contains(q)).ToList();

                return View(activity.OrderBy(m => m.activity_id).ToPagedList(page, pageSize));
            }
            else
            {
                return View();
            }
        }
        public ActionResult Hotel(string q = null, int page = 1)
        {
            ViewBag.q = q;
            Session["pg"] = page;
            if (!String.IsNullOrEmpty(q))
            {

                var hotel = db.hotel.Where(s => s.hotel_intro.Contains(q) || s.hotel_title.Contains(q) || s.city.Contains(q) || s.district.Contains(q)).ToList();

                return View(hotel.OrderBy(m => m.hotel_id).ToPagedList(page, pageSize));
            }
            else
            {
                return View();
            }
        }
        public ActionResult Restaurant(string q = null, int page = 1)
        {
            ViewBag.q = q;
            Session["pg"] = page;
            if (!String.IsNullOrEmpty(q))
            {
                var restaurant = db.restaurant.Where(s => s.restaurant_intro.Contains(q) || s.restaurant_title.Contains(q) || s.city.Contains(q) || s.district.Contains(q)).ToList();

                return View(restaurant.OrderBy(m => m.restaurant_id).ToPagedList(page, pageSize));
            }
            else
            {
                return View();
            }
        }
        public ActionResult Spot(string q = null, int page = 1)
        {
            ViewBag.q = q;
            Session["pg"] = page;
            if (!String.IsNullOrEmpty(q))
            {

                var spot = db.spot.Where(s => s.spot_intro.Contains(q) || s.spot_title.Contains(q) || s.city.Contains(q) || s.district.Contains(q)).ToList();

                return View(spot.OrderBy(m => m.spot_id).ToPagedList(page, pageSize));
            }
            else
            {
                return View();
            }
        }
    }
}