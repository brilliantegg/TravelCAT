using System;
using System.Collections.Generic;
using System.Data.Entity;
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


        //修改密碼
        public ActionResult Editpassword(string id)
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
        [ValidateAntiForgeryToken]
        public ActionResult Editpassword(string id, string oldpassword, string newpassword)
        {
            member member = db.member.Find(id);
            
            byte[] password1 = System.Text.Encoding.UTF8.GetBytes(oldpassword);
            byte[] hash1 = new System.Security.Cryptography.SHA256Managed().ComputeHash(password1);
            string hashpassword1 = Convert.ToBase64String(hash1);

            ViewBag.Err = "原密碼有誤";

            if (hashpassword1 == member.member_password)
            {
                if (ModelState.IsValid)
                {
                    byte[] password = System.Text.Encoding.UTF8.GetBytes(newpassword);
                    byte[] hash = new System.Security.Cryptography.SHA256Managed().ComputeHash(password);
                    string hashpassword = Convert.ToBase64String(hash);

                    member.member_password = hashpassword;

                    db.Entry(member).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index", "web_Member_Index", new { id = member.member_id });
                }
            }
            return View(member);
        }
    }
}