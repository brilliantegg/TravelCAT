using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
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
            return View();
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
        public ActionResult Create(string tourismID)
        {
            ViewBag.tourismID = tourismID;


            return View();
        }

        // POST: web_comments/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(comment comment, HttpPostedFileBase comment_photo)
        {
            //處理圖檔上傳
            string fileName = "";
            string rename_filename = "";
            if (comment_photo != null)
            {
                if (comment_photo.ContentLength > 0)
                {

                    fileName = System.IO.Path.GetFileName(comment_photo.FileName);      //取得檔案的檔名(主檔名+副檔名)
                    rename_filename = comment.comment_id + "_" + DateTime.Now.ToString().Replace("/", "").Replace(":", "").Replace(" ", "") + Path.GetExtension(fileName);

                }
            }
            //end
            if (ModelState.IsValid)
            {
                comment_photo.SaveAs(Server.MapPath("~/images/comment/" + rename_filename));      //將檔案存到該資料夾
                comment.comment_date = DateTime.Now;
                comment.comment_photo = rename_filename;
                db.comment.Add(comment);
                db.SaveChanges();
                return RedirectToRoute(new { controller = "web_activities", action = "Details", id = comment.tourism_id });

            }

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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(comment comment, HttpPostedFileBase comment_photo, string oldImg)
        {
            db.Entry(comment).State = EntityState.Modified;
            db.SaveChanges();
            String id = comment.tourism_id;
            //處理圖檔上傳
            string fileName = "";
            string rename_filename = "";
            
            if (comment_photo != null)
            {
                if (comment_photo.ContentLength > 0)
                {
                    System.IO.File.Delete(Server.MapPath("~/images/comment/" + oldImg));
                    fileName = System.IO.Path.GetFileName(comment_photo.FileName);      //取得檔案的檔名(主檔名+副檔名)
                    rename_filename = comment.comment_id + "_" + DateTime.Now.ToString().Replace("/", "").Replace(":", "").Replace(" ", "") + Path.GetExtension(fileName);
                    comment_photo.SaveAs(Server.MapPath("~/images/comment/" + rename_filename));      //將檔案存到該資料夾
                    comment.comment_photo = rename_filename;
                }
            }
            else
            {
                comment.comment_photo = oldImg;
            }
            //end                      
            if (ModelState.IsValid)
            {               
                db.SaveChanges();
                return RedirectToAction("Details", "web_activities", new { id = id });
            }

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

            List<comment_emoji_details> emojis = db.comment_emoji_details.Where(m => m.comment_id == id).ToList();

            List<message> messages = db.message.Where(m => m.comment_id == id).ToList();

            List<message_emoji_details> mEmojis = db.message_emoji_details.Where(m => m.message.msg_id == id).ToList();


            mEmojis.ForEach(m => db.message_emoji_details.Remove(m));
            db.SaveChanges();
            messages.ForEach(m => db.message.Remove(m));
            db.SaveChanges();
            emojis.ForEach(m => db.comment_emoji_details.Remove(m));
            db.SaveChanges();
            db.comment.Remove(comment);
            db.SaveChanges();

            if (comment == null)
            {
                return HttpNotFound();
            }
            //for(int i = 0; i< emojis.Count; i++)
            //{
            //    db.comment_emoji_details.Remove(emojis[i]);
            //    db.SaveChanges();
            //}            


            return RedirectToRoute(new { controller = "web_activities", action = "Details", id = comment.tourism_id });

        }



        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        //_ partial view
        [ChildActionOnly]
        public ActionResult _PhotoGallery(int number = 0)
        {
            List<comment> comment;
            if (number == 0)
            {
                comment = db.comment.OrderByDescending(p => p.comment_date).ThenByDescending(p => p.comment_id).ToList();

            }
            else
            {
                comment = db.comment.OrderByDescending(p => p.comment_date).ThenByDescending(p => p.comment_id).Take(number).ToList();
            }
            return PartialView(comment);
        }

    }
}
