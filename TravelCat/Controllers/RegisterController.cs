using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TravelCat.Models;
using TravelCat.ViewModels;

namespace TravelCat.Controllers
{

    public class RegisterController : Controller
    {
        dbTravelCat db = new dbTravelCat();

        // GET: Register
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Index(MemberViewModels memberViewModels,HttpPostedFileBase photo)
        {
            string mem_id = db.Database.SqlQuery<string>("Select dbo.GetmemberId()").FirstOrDefault();
            memberViewModels.member.member_id = mem_id;
            memberViewModels.profile.member_id = mem_id;
            memberViewModels.profile.member_score = 0;
            memberViewModels.profile.create_time = DateTime.Now;

            string fileName = "";
            if (photo!= null)
            {
                if (photo.ContentLength > 0)
                {
                    string t = photo.FileName;
                    fileName = mem_id + "_" + DateTime.Now.ToString().Replace("/", "").Replace(":", "").Replace(" ", "")+ Path.GetExtension(t);
                    photo.SaveAs(Server.MapPath("~/images/member/" + fileName));
                    memberViewModels.profile.profile_photo = fileName;
                }
            }

            db.member.Add(memberViewModels.member);
            db.member_profile.Add(memberViewModels.profile);
            db.SaveChanges();


            return RedirectToAction("Index","Home");
        }
    }
}