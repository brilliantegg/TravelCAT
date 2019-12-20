using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TravelCat.Models;
using X.PagedList;
using X.PagedList.Mvc;

namespace TravelCat.Controllers
{
    public class commentsController : Controller
    {
        private dbTravelCat db = new dbTravelCat();

        // GET: comments
        public ActionResult Index(string id=null,int page=1)
        {
            ViewBag.id = id;

            var comments = db.comment.Include(c => c.member).OrderBy(m => m.comment_id).ToList();
            int pagesize = 10;
            int pagecurrent = page < 1 ? 1 : page;
            var pagedlist = comments.ToPagedList(pagecurrent, pagesize);

            if (!String.IsNullOrEmpty(id))
            {
                var search = db.comment.Where(m => m.comment_id.ToString().Contains(id));
                return View(search.OrderBy(m => m.comment_id).ToPagedList(page, pagesize));
            }
            else
            {
                return View("Index", pagedlist);
            }

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
        public ActionResult Edit( comment comment)
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
