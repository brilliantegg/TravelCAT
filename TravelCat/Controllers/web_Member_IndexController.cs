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

            };
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
    }
}