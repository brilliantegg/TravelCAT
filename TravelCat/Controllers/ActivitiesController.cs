using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TravelCat.Models;
using TravelCat.ViewModels;
using PagedList;

namespace TravelCat.Controllers
{
    public class ActivitiesController : Controller
    {        
        dbTravelCat db = new dbTravelCat();
        int pageSize = 10;
        
        // GET: Activities
        public ActionResult Index(int? page)
        {
            int pageNumber = (page ?? 1);
            var data = db.activities.OrderBy(m => m.activity_id).ToPagedList(pageNumber, pageSize);


            return View(data);
        }
        //public ActionResult contentQuery(string id)
        //{
        //    var search = from a in db.activities
        //                 select a;
        //    if (!String.IsNullOrEmpty(id))
        //    {
        //        search = search.Where(s => s.activity_id.Contains(id) || s.activity_title.Contains(id)
        //        || s.city.Contains(id) || s.district.Contains(id));
        //    }
        //    return View(search);

        //}                   

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]          //預設就是httpget，這裡是要讓他新增資料到資料庫
        public ActionResult Create(activity activity, HttpPostedFileBase tourism_photo)       //多載(overloading/overloading)
        {
            string act_id = db.Database.SqlQuery<string>("Select dbo.GetactivityId()").FirstOrDefault();
            activity.activity_id = act_id;
            
            //處理圖檔上傳
            string fileName = "";
            if (tourism_photo != null)
            {
                if (tourism_photo.ContentLength > 0)
                {
                    fileName = System.IO.Path.GetFileName(tourism_photo.FileName);      //取得檔案的檔名(主檔名+副檔名)
                    tourism_photo.SaveAs(Server.MapPath("~/images/activity/" + fileName));      //將檔案存到該資料夾
                }
            }
            //end
            tourism_photo tp = new tourism_photo();
            tp.tourism_photo1 = fileName;
            tp.tourism_id = activity.activity_id;

            db.activities.Add(activity);
            db.SaveChanges();
            db.tourism_photo.Add(tp);
            try
            {
                db.SaveChanges();
            }
            catch (DbEntityValidationException ex)
            {
                var entityError = ex.EntityValidationErrors.SelectMany(x => x.ValidationErrors).Select(x => x.ErrorMessage);
                var getFullMessage = string.Join("; ", entityError);
                var exceptionMessage = string.Concat(ex.Message, "errors are: ", getFullMessage);
                //NLog
                return View(exceptionMessage.ToString());
                
            }

            return RedirectToAction("Index");
        }

        public ActionResult Delete(string Id)
        {
            var product = db.activities.Where(m => m.activity_id == Id).FirstOrDefault();        //找到id等於傳進來的id值的資料
            db.activities.Remove(product);
            db.SaveChanges();
            //如果要刪檔案
            var photos = db.tourism_photo.Where(m => m.tourism_id == Id).FirstOrDefault();
            string fileName = photos.tourism_photo1;
            if (fileName != "")
            {
                System.IO.File.Delete(Server.MapPath("~/images/activity/" + fileName));
            }
            db.tourism_photo.Remove(photos);
            db.SaveChanges();

            return RedirectToAction("Index");
        }
        public ActionResult Edit(string id)
        {
            ActivityPhotoViewModel model = new ActivityPhotoViewModel() { 
            activity= db.activities.Where(m => m.activity_id == id).FirstOrDefault(),
            activity_photos = db.tourism_photo.Where(m => m.tourism_id == id).FirstOrDefault()
            };
            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(string id, ActivityPhotoViewModel activityPhotoViewModel, HttpPostedFileBase tourism_photo, String oldImg)
        {
            //activityPhotoViewModel.activity.activity_id = id;


            db.Entry(activityPhotoViewModel.activity).State = EntityState.Modified;
            db.SaveChanges();
            
            string fileName = "";
            if (tourism_photo != null)
            {
                if (tourism_photo.ContentLength > 0)
                {
                    System.IO.File.Delete(Server.MapPath("~/images/activity/" + oldImg));
                    fileName = System.IO.Path.GetFileName(tourism_photo.FileName);      //取得檔案的檔名(主檔名+副檔名)
                    tourism_photo.SaveAs(Server.MapPath("~/images/activity/" + fileName));      //將檔案存到該資料夾
                }
            }
            else
            {
                fileName = oldImg;
            }
            var tp1 = db.tourism_photo.Where(m => m.tourism_id == id).FirstOrDefault();
            
            tp1.tourism_photo1 = fileName;
            db.SaveChanges();
            
            return RedirectToAction("Index");
        }

    }
}