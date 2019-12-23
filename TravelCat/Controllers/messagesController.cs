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

namespace TravelCat.Controllers
{
    public class messagesController : Controller
    {
        private dbTravelCat db = new dbTravelCat();

        // GET: messages
        public ActionResult Index(string id = null, int page = 1)
        {
            ViewBag.id = id;

            var messages = db.message.Include(m => m.comment).Include(m => m.member).ToList();
            int pagesize = 10;
            int pagecurrent = page < 1 ? 1 : page;
            var pagedlist = messages.ToPagedList(pagecurrent, pagesize);

            if (!String.IsNullOrEmpty(id))
            {
                var search = db.message.Where(m => m.msg_id.ToString()==id).ToList();
                return View(search.ToPagedList(page, pagesize));
            }
            else
            {
                return View("Index", pagedlist);
            }

        }

        // GET: messages/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            message message = db.message.Find(id);
            if (message == null)
            {
                return HttpNotFound();
            }
            return View(message);
        }

        // GET: messages/Create
        public ActionResult Create()
        {
            ViewBag.comment_id = new SelectList(db.comment, "comment_id", "tourism_id");
            ViewBag.member_id = new SelectList(db.member, "member_id", "member_account");
            return View();
        }

        // POST: messages/Create
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "msg_id,msg_time,msg_content,comment_id,member_id")] message message)
        {
            if (ModelState.IsValid)
            {
                db.message.Add(message);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.comment_id = new SelectList(db.comment, "comment_id", "tourism_id", message.comment_id);
            ViewBag.member_id = new SelectList(db.member, "member_id", "member_account", message.member_id);
            return View(message);
        }

        // GET: messages/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            message message = db.message.Find(id);
            if (message == null)
            {
                return HttpNotFound();
            }
            ViewBag.comment_id = new SelectList(db.comment, "comment_id", "tourism_id", message.comment_id);
            ViewBag.member_id = new SelectList(db.member, "member_id", "member_account", message.member_id);
            return View(message);
        }

        // POST: messages/Edit/5
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "msg_id,msg_time,msg_content,comment_id,member_id,msg_status,tourism_id")] message message)
        {
            if (ModelState.IsValid)
            {
                db.Entry(message).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.comment_id = new SelectList(db.comment, "comment_id", "tourism_id", message.comment_id);
            ViewBag.member_id = new SelectList(db.member, "member_id", "member_account", message.member_id);
            return View(message);
        }

        // GET: messages/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            message message = db.message.Find(id);
            if (message == null)
            {
                return HttpNotFound();
            }
            return View(message);
        }

        // POST: messages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            message message = db.message.Find(id);
            db.message.Remove(message);
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
