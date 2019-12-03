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
    public class commentsController : Controller
    {
        private dbTravelCat db = new dbTravelCat();

        // GET: comments
        public ActionResult Index()
        {
            var comments = db.comment.Include(c => c.member);
            return View(comments.ToList());
        }

        // GET: comments/Edit/5
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
            ViewBag.member_id = new SelectList(db.member, "member_id", "member_account", comment.member_id);
            return View(comment);
        }

        // POST: comments/Edit/5
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
            
            return View(comment);
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
