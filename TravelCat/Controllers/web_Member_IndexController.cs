using System;
using System.Collections.Generic;
using System.Linq;
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
        public ActionResult Index(string memberId = "M000002")
        {
            MemberIndexViewModels model = new MemberIndexViewModels()
            {
                member = db.member.Where(m => m.member_id == memberId).FirstOrDefault(),
                member_profile = db.member_profile.Where(m=>m.member_id == memberId).FirstOrDefault(),

            };
            return View(model);
        }
    }
}