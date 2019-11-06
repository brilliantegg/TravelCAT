using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TravelCat.Models;

namespace TravelCat.Controllers
{
    public class web_commentsController : Controller
    {
        private dbTravelCat db = new dbTravelCat();

        // GET: web_comments
        public ActionResult Index()
        {
            var comment = db.comment.Include(c => c.member);
            return View(comment.ToList());
        }

        // GET: web_comments/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            comment comment = db.comment.Find(id);
            if (comment == null)
            {
                return HttpNotFound();
            }
            return View(comment);
        }

        // GET: web_comments/Create
        public ActionResult Create()
        {
            ViewBag.member_id = new SelectList(db.member, "member_id", "member_account");
            return View();
        }

        // POST: web_comments/Create
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "comment_id,tourism_id,comment_title,comment_content,comment_date,comment_photo,comment_stay_total,travel_partner,comment_rating,travel_month,comment_status,member_id")] comment comment)
        {
            string[] travel_Partner = { "伴侶旅行", "家庭 (含年幼孩童)", "家庭 (含青少年)", "朋友", "商務", "單獨旅行", "其他生物" };

            ViewBag.travelPartner = travel_Partner;
            if (ModelState.IsValid)
            {
                db.comment.Add(comment);
                db.SaveChanges();
                return RedirectToRoute(new { controller = "web_activities", action = "Details", id = comment.tourism_id });

            }

            ViewBag.member_id = new SelectList(db.member, "member_id", "member_account", comment.member_id);
            return View(comment);
        }

        // GET: web_comments/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            comment comment = db.comment.Find(id);
            if (comment == null)
            {
                return HttpNotFound();
            }

            return View(comment);
        }

        // POST: web_comments/Edit/5
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "comment_id,tourism_id,comment_title,comment_content,comment_date,comment_photo,comment_stay_total,travel_partner,comment_rating,travel_month,comment_status,member_id")] comment comment)
        {
            if (ModelState.IsValid)
            {
                db.Entry(comment).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.member_id = new SelectList(db.member, "member_id", "member_account", comment.member_id);
            return View(comment);
        }

        // GET: web_comments/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            comment comment = db.comment.Find(id);
            if (comment == null)
            {
                return HttpNotFound();
            }
            return View(comment);
        }

        // POST: web_comments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            comment comment = db.comment.Find(id);
            db.comment.Remove(comment);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
