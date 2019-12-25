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
using X.PagedList;

namespace TravelCat.Controllers
{
    public class membersController : Controller
    {
        private dbTravelCat db = new dbTravelCat();

        // GET: members1
        public ActionResult Index(string id = null, int page = 1,int tab =1)
        {
            ViewBag.id = id;
            ViewBag.tab = tab;
            var member = db.member.OrderBy(m => m.member_id).ToList();
            int pagesize = 10;
            int pagecurrent = page < 1 ? 1 : page;
            var pagedlist = member.ToPagedList(pagecurrent, pagesize);

            if (!String.IsNullOrEmpty(id))
            {
                var search = db.member.Where(m => m.member_id == id).ToList();
                return View(search.ToPagedList(page, pagesize));
            }
            else
            {
                return View("Index", pagedlist);
            }
        }

        // GET: members1/Edit/5
        public ActionResult Edit(string id)
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

        // POST: members1/Edit/5
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "member_id,member_account,member_password,member_status")] member member)
        {
            string email = db.member_profile.Where(m => m.member_id == member.member_id).FirstOrDefault().email;

            if (member.member_status == true)
            {
                GmailSender gs = new GmailSender();
                gs.account = "travelcat.service@gmail.com";
                gs.password = "lqleyzcbmrmttloe";
                gs.sender = "旅途貓 <travelcat.service@gmail.com>";
                gs.receiver = $"{email}";
                gs.subject = "會員通知";
                gs.messageBody = "<h3>親愛的會員您好:</h3><br /><p>因您已違反本網站規定，本站將取消您的會員，如有任何疑問請與本站客服人員聯絡。</p>";
                gs.IsHtml = true;
                gs.Send();
            }

            if (ModelState.IsValid)
            {
                db.Entry(member).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(member);
        }


    }
}
