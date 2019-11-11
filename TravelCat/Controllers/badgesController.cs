using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TravelCat.Models;

namespace TravelCat.Controllers
{
    public class badgesController : Controller
    {
        private dbTravelCat db = new dbTravelCat();

        // GET: badges
        public ActionResult Index()
        {
            return View(db.badge.ToList());
        }

        

        // GET: badges/Create
        public ActionResult Create()
        {
            return View();
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(badge badge, HttpPostedFileBase badge_photo)
        {          

            if (ModelState.IsValid)
            {
                string fileName = "";
                if (badge_photo != null)
                {
                    if (badge_photo.ContentLength > 0)
                    {
                        fileName = System.IO.Path.GetFileName(badge_photo.FileName);      //取得檔案的檔名(主檔名+副檔名)
                        badge_photo.SaveAs(Server.MapPath("~/images/badges/" + fileName));      //將檔案存到該資料夾
                    }
                    }
                badge.badge_photo = fileName;

                db.badge.Add(badge);
                db.SaveChanges();
                return RedirectToAction("Index");
            }           
           
            return View(badge);
        }
        
        // GET: badges/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            badge badge = db.badge.Find(id);
            if (badge == null)
            {
                return HttpNotFound();
            }
            return View(badge);
        }

       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id,badge badge, HttpPostedFileBase badge_photo, String oldImg)
        {
            
            db.Entry(badge).State = EntityState.Modified;
            
            string fileName = "";
            if (badge_photo != null)
            {
                if (badge_photo.ContentLength > 0)
                {
                    System.IO.File.Delete(Server.MapPath("~/images/badges/" + oldImg));
                    fileName = System.IO.Path.GetFileName(badge_photo.FileName);      //取得檔案的檔名(主檔名+副檔名)
                    badge_photo.SaveAs(Server.MapPath("~/images/badges/" + fileName));      //將檔案存到該資料夾
                }
            }
            else
            {
                fileName = oldImg;
            }
            var tp1 = db.badge.Where(m => m.badge_id == id).FirstOrDefault();

            tp1.badge_photo = fileName;
            tp1.badge_title = badge.badge_title;
            db.SaveChanges();

            return RedirectToAction("Index");
        }

        // GET: badges/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            badge badge = db.badge.Find(id);
            if (badge == null)
            {
                return HttpNotFound();
            }
            return View(badge);
        }

        // POST: badges/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            badge badge = db.badge.Find(id);
            db.badge.Remove(badge);
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
