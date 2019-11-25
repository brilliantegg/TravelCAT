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
    //[Authorize]
    public class AdminController : Controller
    {
        private dbTravelCat db = new dbTravelCat();

        // GET: Admin

        public ActionResult Home()
        {
            return View();
        }

        public ActionResult Index()
        {
            return View(db.admin.ToList());
        }

        // GET: Admin/Details/5

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            admin admin = db.admin.Find(id);
            if (admin == null)
            {
                return HttpNotFound();
            }
            return View(admin);
        }

        // GET: Admin/Create

        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/Create
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "admin_id,admin_account,admin_password,admin_email,emailConfirmed")] admin admin)
        {
            
            if (ModelState.IsValid)
            {
                byte[] password = System.Text.Encoding.UTF8.GetBytes(admin.admin_password);
                byte[] hash = new System.Security.Cryptography.SHA256Managed().ComputeHash(password);
                string hashpassword = Convert.ToBase64String(hash);
                admin.admin_password = hashpassword;
                admin.emailConfirmed = false;


                var callbackUrl = Url.Action("Confirm", "Admin", new { account = admin.admin_account }, protocol: Request.Url.Scheme);
                GmailSender gs = new GmailSender();
                gs.account = "travelcat.service@gmail.com";
                gs.password = "lqleyzcbmrmttloe";
                gs.sender = "旅途貓 <travelcat.service@gmail.com>";
                gs.receiver = $"{admin.admin_email}";
                gs.subject = "旅途貓驗證";
                gs.messageBody = "恭喜註冊成功<br><a href=" + callbackUrl + ">請點此連結</a>";
                gs.IsHtml = true;
                gs.Send();

                db.admin.Add(admin);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(admin);
        }

        // GET: Admin/Edit/5

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            admin admin = db.admin.Find(id);
            if (admin == null)
            {
                return HttpNotFound();
            }
            return View(admin);
        }

        // POST: Admin/Edit/5
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "admin_id,admin_account,admin_password,admin_email,emailConfirmed")] admin admin)
        {
            if (ModelState.IsValid)
            {
                byte[] password = System.Text.Encoding.UTF8.GetBytes(admin.admin_password);
                byte[] hash = new System.Security.Cryptography.SHA256Managed().ComputeHash(password);
                string hashpassword = Convert.ToBase64String(hash);
                admin.admin_password = hashpassword;

                GmailSender gs = new GmailSender();
                gs.account = "travelcat.service@gmail.com";
                gs.password = "lqleyzcbmrmttloe";
                gs.sender = "旅途貓 <travelcat.service@gmail.com>";
                gs.receiver = $"{admin.admin_email}";
                gs.subject = "旅途貓驗證";
                gs.messageBody = "恭喜驗證成功";
                gs.IsHtml = true;
                gs.Send();
             

                db.Entry(admin).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");



            }
            return View(admin);
        }

        //修改密碼
        public ActionResult Edit1(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            admin admin = db.admin.Find(id);
            if (admin == null)
            {
                return HttpNotFound();
            }
            return View(admin);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit1(string oldpassword,[Bind(Include = "admin_id,admin_account,admin_password,admin_email,emailConfirmed")] admin admin)
        {
            byte[] password = System.Text.Encoding.UTF8.GetBytes(admin.admin_password);
            byte[] hash = new System.Security.Cryptography.SHA256Managed().ComputeHash(password);
            string hashpassword = Convert.ToBase64String(hash);
            admin.admin_password = hashpassword;
            oldpassword = admin.admin_password;


            if (ModelState.IsValid)
            {
                byte[] password1 = System.Text.Encoding.UTF8.GetBytes(admin.admin_password);
                byte[] hash1 = new System.Security.Cryptography.SHA256Managed().ComputeHash(password1);
                string hashpassword1 = Convert.ToBase64String(hash1);
                admin.admin_password = hashpassword1;


                db.Entry(admin).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View();
        }

        // GET: Admin/Delete/5

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            admin admin = db.admin.Find(id);
            if (admin == null)
            {
                return HttpNotFound();
            }

            return View(admin);
        }

        //POST: Admin/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            admin admin = db.admin.Find(id);
            db.admin.Remove(admin);
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
