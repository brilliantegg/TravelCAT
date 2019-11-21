﻿using System;
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
            ViewBag.getTime = DateTime.Now.ToString("yyyy/MM/dd");
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            destinationsViewModel model = new destinationsViewModel()
            {
                activity = db.activity.Where(m => m.activity_id == id).FirstOrDefault(),
                comment = db.comment.Where(m => m.tourism_id == id).OrderByDescending(m=>m.comment_date).ToList(),
                message = db.message.Where(m => m.tourism_id == id).OrderByDescending(m => m.msg_time).ToList(),
                comment_emoji_details = db.comment_emoji_details.ToList(),
                member_profile = db.member_profile.ToList(),
                member = db.member.ToList(),
            };
            if (model == null)
            {
                return HttpNotFound();
            }
            ViewBag.id = id;
            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult createMessage( message message)
        {
            message.msg_time = DateTime.Now;
            if (ModelState.IsValid)
            {
                db.message.Add(message);
                db.SaveChanges();
                return RedirectToRoute(new { controller = "web_activities", action = "Details", id = message.tourism_id });

            }

            return RedirectToRoute(new { controller = "web_activities", action = "Details", id = message.tourism_id });
        }
        
    }
}