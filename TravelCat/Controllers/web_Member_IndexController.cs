using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TravelCat.Models;
using TravelCat.ViewModels;

namespace TravelCat.Controllers
{
    [Authorize(Roles ="Confirmed,UnConfirmed")]
    public class web_Member_IndexController : Controller
    {
        dbTravelCat db = new dbTravelCat();

        // GET: web_Member_Index
        public ActionResult Index(string id)
        {
            MemberIndexViewModels model = new MemberIndexViewModels()
            {
                member = db.member.Find(id),
                member_profile = db.member_profile.Find(id),
                comment = db.comment.OrderByDescending(m=>m.comment_id).ToList(),
                follow_list = db.follow_list.ToList()
            };
            ViewBag.memberId = id;
            return View(model);
        }
        public ActionResult EditMemberProfile (string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            member member = db.member.Find(id);


            if (member == null)
            {
                return HttpNotFound();
            }
            return View(member);
        }
        [HttpPost]
        public string getfollowed(string member_id,string followed_id)
        {
            string response = "done";
            follow_list follower = new follow_list();
            follower.follow_date = DateTime.Now;
            follower.member_id = member_id;
            follower.followed_id = followed_id;
            db.follow_list.Add(follower);
            db.SaveChanges();
            return response;
        }
    }
}