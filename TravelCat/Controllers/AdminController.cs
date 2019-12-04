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

        public ActionResult Home()
        {
            return View();
        }

        public ActionResult Index()
        {
            return View(db.admin.ToList());
        }


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

        public ActionResult Create()
        {
            return View();
        }
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
        public ActionResult Confirm(string account)
        {
            var check = db.admin.Where(m => m.admin_account == account).FirstOrDefault();
            if (check != null)
            {
                DialogResult ans = MessageBox.Show("註冊已完成", "信箱已確認", MessageBoxButtons.OK, MessageBoxIcon.Question);
                if (ans == DialogResult.OK)
                {
                    check.emailConfirmed = true;
                    db.SaveChanges();
                    return RedirectToAction("Index", "Admin");
                }
                return View("重新整理");
            }
            else
            {
                DialogResult ans = MessageBox.Show("請先註冊會員!", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Question);
                return RedirectToAction("Create", "Admin");
            }

        }


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
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "admin_id,admin_account,admin_password,admin_email,emailConfirmed")] admin admin)
        {
            if (ModelState.IsValid)
            {

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


                db.Entry(admin).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");



            }
            return View(admin);
        }

        //修改密碼
        public ActionResult Editpwd(int? id)
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
        public ActionResult Editpwd(int id,string oldpassword, string newpassword)
        {
            admin admin = db.admin.Find(id);

            byte[] password1 = System.Text.Encoding.UTF8.GetBytes(oldpassword);
            byte[] hash1 = new System.Security.Cryptography.SHA256Managed().ComputeHash(password1);
            string hashpassword1 = Convert.ToBase64String(hash1);

            ViewBag.Err = "原密碼有誤";
            if (hashpassword1 == admin.admin_password )
            {
                if (ModelState.IsValid)
                {
                    byte[] password = System.Text.Encoding.UTF8.GetBytes(newpassword);
                    byte[] hash = new System.Security.Cryptography.SHA256Managed().ComputeHash(password);
                    string hashpassword = Convert.ToBase64String(hash);

                    admin.admin_password = hashpassword;

                    db.Entry(admin).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            return View();
        }


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
