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
    public class issuesController : Controller
    {
        private dbTravelCat db = new dbTravelCat();

        // GET: issues
        public ActionResult Index()
        {
            var issues = db.issue.Include(i => i.admin).Include(i => i.issue_type).Include(i => i.member);
            //return View(issues.ToList());
            return View();
        }

        //問題PartialView
        [ChildActionOnly]
        public PartialViewResult _Problem(int type_id, int page = 1, string Problem_id = null)
        {
            var issue = db.issue.Where(m => m.issue_id == type_id).OrderBy(m => m.issue_status).ThenByDescending(m => m.resolve_date).ToList();

            int pagesize = 10;
            int pagecurrent = page < 1 ? 1 : page;
            var pagedlist = issue.ToPagedList(pagecurrent, pagesize);

            ViewBag.issueId = type_id;

            if (!String.IsNullOrEmpty(Problem_id))
            {
                var search = db.issue.Where(m => m.issue_content.Contains(Problem_id) || m.problem_id.Contains(Problem_id) || m.member_id.Contains(Problem_id));
                return PartialView(search.OrderBy(m => m.report_date).ToPagedList(pagecurrent, pagesize));
            }
            else
            {
                return PartialView("_Problem", pagedlist);
            }

        }

        // GET: issues/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            issue issue = new issue();
            issue = db.issue.Where(m => m.id == id).FirstOrDefault();

            //issue issue. = db.issues.Find(id);
            if (issue == null)
            {
                return HttpNotFound();
            }
            ViewBag.admin_id = new SelectList(db.admin, "admin_id", "admin_account", issue.admin_id);
            ViewBag.issue_id = new SelectList(db.issue_type, "issue_id", "issue_name", issue.issue_id);
            ViewBag.member_id = new SelectList(db.member, "member_id", "member_account", issue.member_id);
            return View(issue);
        }

        // POST: issues/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,member_id,admin_id,issue_id,report_date,issue_content,issue_result,issue_status,resolve_date,problem_id")] issue issue)
        {
            string email = db.member_profile.Where(m => m.member_id == issue.member_id).FirstOrDefault().email;
            string result = issue.issue_result;
            string content = issue.issue_content;

            int id = issue.issue_id;
            string controller;
            switch (id)
            {
                case 1:
                    controller = "issues";
                    break;
                case 2:
                    controller = "members";
                    break;
                case 3:
                    controller = "comments";
                    break;
                case 4:
                    controller = "messages";
                    break;
                default:
                    controller = "issues";
                    break;
            }

            if (issue.issue_status == true)
            {
                GmailSender gs = new GmailSender();
                gs.account = "travelcat.service@gmail.com";
                gs.password = "lqleyzcbmrmttloe";
                gs.sender = "旅途貓 <travelcat.service@gmail.com>";
                gs.receiver = $"{email}";
                gs.subject = "系統問題回覆";
                gs.messageBody = "<div><h3>關於您的問題:</h3><p>" + content + "</p><br></div>" + "<div><h3>以下是本網站針對此問題做出的回覆:</h3><p>" + result+ "</p></div><br><footer>感謝您寶貴的建議，全體人員在此感謝。</footer>";
                gs.IsHtml = true;
                gs.Send();
            }
            issue.resolve_date = DateTime.Now;
            if (ModelState.IsValid)
            {
                db.Entry(issue).State = EntityState.Modified;
                db.SaveChanges();

                return RedirectToRoute(new { controller = controller, action = "Index", tab = 2 });
            }

            ViewBag.admin_id = new SelectList(db.admin, "admin_id", "admin_account", issue.admin_id);

            return View(issue);
        }



    }
}
