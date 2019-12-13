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
        //新增收藏
        [HttpPost]
        public ActionResult Postcollections_detail(string member_id, string tourism_id,int collection_type_id=1)
        {
            string id = tourism_id.Substring(0, 1);
            string controller;
            switch (id)
            {
                case "A":
                    controller = "web_activities";
                    break;
                case "H":
                    controller = "WebHotels";
                    break;
                case "R":
                    controller = "WebRestaurants";
                    break;
                case "S":
                    controller = "WebSpots";
                    break;
                default:
                    controller = "web_activities";
                    break;
            }
            collections_detail collect = new collections_detail();
            collect.member_id = member_id;
            collect.tourism_id = tourism_id;
            collect.privacy = true;
            collect.collection_type_id = collection_type_id;


            if (ModelState.IsValid)
            {
                db.collections_detail.Add(collect);
                db.SaveChanges();
                return RedirectToRoute(new { controller = controller, action = "Details", id = tourism_id });

            }

            return RedirectToRoute(new { controller = controller, action = "Details", id = tourism_id });
        }
        //刪除收藏
        [HttpPost]
        public ActionResult Deletecollections_detail(string tourism_id,int? collection_id,string member_id)
        {
            if (collection_id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            collections_detail collections_detail = db.collections_detail.Where(m => m.collection_id == collection_id && m.member_id == member_id).FirstOrDefault();
            db.collections_detail.Remove(collections_detail);
            db.SaveChanges();

            string id = tourism_id.Substring(0, 1);
            string controller;
            switch (id)
            {
                case "A":
                    controller = "web_activities";
                    break;
                case "H":
                    controller = "WebHotels";
                    break;
                case "R":
                    controller = "WebRestaurants";
                    break;
                case "S":
                    controller = "WebSpots";
                    break;
                default:
                    controller = "web_activities";
                    break;
            }


            return RedirectToRoute(new { controller = controller, action = "Details", id = tourism_id });
        }
        //新增留言
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult createMessage(message message)
        {
            
            string id = message.tourism_id.Substring(0, 1);
            string controller;
            switch (id)
            {
                case "A":
                    controller = "web_activities";
                    break;
                case "H":
                    controller = "WebHotels";
                    break;
                case "R":
                    controller = "WebRestaurants";
                    break;
                case "S":
                    controller = "WebSpots";
                    break;
                default:
                    controller = "web_activities";
                    break;
            }
            message.msg_time = DateTime.Now;
            if (ModelState.IsValid)
            {
                db.message.Add(message);
                db.SaveChanges();

                return RedirectToRoute(new { controller = controller, action = "Details", id = message.tourism_id });

            }

            return RedirectToRoute(new { controller = controller, action = "Details", id = message.tourism_id });
        }

        // GET 新增評論
        public ActionResult Create(string tourismID)
        {
            ViewBag.tourismID = tourismID;


            return View();
        }

        // POST 新增評論

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(comment comment, HttpPostedFileBase photo)
        {
            string id = comment.tourism_id.Substring(0, 1);
            string controller;
            switch (id)
            {
                case "A":
                    controller = "web_activities";
                    break;
                case "H":
                    controller = "WebHotels";
                    break;
                case "R":
                    controller = "WebRestaurants";
                    break;
                case "S":
                    controller = "WebSpots";
                    break;
                default:
                    controller = "web_activities";
                    break;
            }
            //處理圖檔上傳
            string fileName = "";
            string rename_filename = "";
            if (photo != null)
            {
                if (photo.ContentLength > 0)
                {

                    fileName = System.IO.Path.GetFileName(photo.FileName);      //取得檔案的檔名(主檔名+副檔名)
                    rename_filename = comment.comment_id + "_" + DateTime.Now.ToString().Replace("/", "").Replace(":", "").Replace(" ", "") + Path.GetExtension(fileName);

                }
            }
            //end
            if (ModelState.IsValid)
            {
                photo.SaveAs(Server.MapPath("~/images/comment/" + rename_filename));      //將檔案存到該資料夾
                comment.comment_date = DateTime.Now;
                comment.comment_photo = rename_filename;
                db.comment.Add(comment);
                db.SaveChanges();
                //Response.Cache.SetCacheability(HttpCacheability.NoCache);
                //Response.Cache.AppendCacheExtension("no-store, must-revalidate");
                //Response.Write("<script type='text/javascript'>window.location.reload(history.go(-2)); </script>");
                return RedirectToRoute(new { controller = controller, action = "Details", id = comment.tourism_id });

            }
            ViewBag.tourismID = comment.tourism_id;
            return View(comment);
        }

        // GET 編輯評論
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

        // POST: 編輯評論

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(comment comment, HttpPostedFileBase comment_photo, string oldImg)
        {
            string id = comment.tourism_id.Substring(0, 1);
            string controller;
            switch (id)
            {
                case "A":
                    controller = "web_activities";
                    break;
                case "H":
                    controller = "WebHotels";
                    break;
                case "R":
                    controller = "WebRestaurants";
                    break;
                case "S":
                    controller = "WebSpots";
                    break;
                default:
                    controller = "web_activities";
                    break;
            }
            db.Entry(comment).State = EntityState.Modified;
            db.SaveChanges();

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


                return RedirectToRoute(new { controller = controller, action = "Details", id = comment.tourism_id });
            }

            return View(comment);
        }

        // 刪除評論
        public ActionResult Delete(long? id)
        {
            string tourism_id = db.comment.Where(m => m.comment_id == id).FirstOrDefault().tourism_id;
            string firstChar = tourism_id.Substring(0, 1);
            string controller;
            switch (firstChar)
            {
                case "A":
                    controller = "web_activities";
                    break;
                case "H":
                    controller = "WebHotels";
                    break;
                case "R":
                    controller = "WebRestaurants";
                    break;
                case "S":
                    controller = "WebSpots";
                    break;
                default:
                    controller = "web_activities";
                    break;
            }
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
            return RedirectToRoute(new { controller = controller, action = "Details", id = comment.tourism_id });

        }




    }
}
