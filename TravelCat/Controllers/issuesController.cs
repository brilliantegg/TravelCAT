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
    public class issuesController : Controller
    {
        private dbTravelCat db = new dbTravelCat();

        // GET: issues
        public ActionResult Index()
        {

            var issues = db.issues.Include(i => i.admin).Include(i => i.issue_type).Include(i => i.member);
            return View(issues.ToList());
        }

        // GET: issues/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            issue issue = new issue();
            issue = db.issues.Where(m=>m.id==id).FirstOrDefault();

            //issue issue. = db.issues.Find(id);
            if (issue == null)
            {
                return HttpNotFound();
            }
            ViewBag.admin_id = new SelectList(db.admins, "admin_id", "admin_account", issue.admin_id);
            ViewBag.issue_id = new SelectList(db.issue_type, "issue_id", "issue_name", issue.issue_id);
            ViewBag.member_id = new SelectList(db.members, "member_id", "member_account", issue.member_id);
            return View(issue);
        }

        // POST: issues/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,member_id,admin_id,issue_id,report_date,issue_content,issue_result,issue_status,resolve_date")] issue issue)
        {
            if (ModelState.IsValid)
            {
                db.Entry(issue).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(issue);
        }


        
    }
}
