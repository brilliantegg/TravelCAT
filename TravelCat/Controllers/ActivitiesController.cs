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
using System.IO;

namespace TravelCat.Controllers
{
    public class ActivitiesController : Controller
    {        
        dbTravelCat db = new dbTravelCat();
        int pageSize = 10;
        
        // GET: Activities
        public ActionResult Index(string id=null,int page=1)
        {
            ViewBag.id = id;
            Session["pg"]=page;
            
            if (!String.IsNullOrEmpty(id))
            {
                var search = db.activity.Where(s => s.activity_id.Contains(id) || s.activity_title.Contains(id)
               || s.city.Contains(id) || s.district.Contains(id));
                return View(search.OrderBy(m=>m.activity_id).ToPagedList(page, pageSize));
            }
            else { 
            var data = db.activity.OrderBy(m => m.activity_id);
            return View(data.ToPagedList(page, pageSize));
            }   
        }     

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]          //預設就是httpget，這裡是要讓他新增資料到資料庫
        public ActionResult Create(activity activity, HttpPostedFileBase[] tourism_photo)       //多載(overloading/overloading)
        {
            string act_id = db.Database.SqlQuery<string>("Select dbo.GetactivityId()").FirstOrDefault();
            activity.activity_id = act_id;
            
             //圖片
            string fileName = "";
            for (int i = 0; i < tourism_photo.Length; i++)
            {
                HttpPostedFileBase f = tourism_photo[i];
                if (f != null)
                {
                    if (f.ContentLength > 0)
                    {
                        string t = tourism_photo[i].FileName;
                        fileName = activity.activity_id + "_" +DateTime.Now.ToString().Replace("/", "").Replace(":", "").Replace(" ", "") + (i + 1).ToString() + Path.GetExtension(t);
                        f.SaveAs(Server.MapPath("~/images/activity/" + fileName));
                        tourism_photo tp = new tourism_photo();
                        tp.tourism_photo1 = fileName;
                        tp.tourism_id = activity.activity_id;
                        db.tourism_photo.Add(tp);
                    }
                }
            }
            db.activity.Add(activity);
            db.SaveChanges();
            //try
            //{
            //    db.SaveChanges();
           // }
            //catch (DbEntityValidationException ex)
            //{
              //  var entityError = ex.EntityValidationErrors.SelectMany(x => x.ValidationErrors).Select(x => x.ErrorMessage);
                //var getFullMessage = string.Join("; ", entityError);
                //var exceptionMessage = string.Concat(ex.Message, "errors are: ", getFullMessage);
                //NLog
                //return View(exceptionMessage.ToString());
                
            //}

            return RedirectToAction("Index",new { page=Session["pg"] });
        }

        public ActionResult Delete(string Id)
        {
            var product = db.activity.Where(m => m.activity_id == Id).FirstOrDefault();        //找到id等於傳進來的id值的資料
            db.activity.Remove(product);
            db.SaveChanges();

            var photos = db.tourism_photo.Where(m => m.tourism_id == Id).ToList();
            for (int i = 0; i < photos.Count; i++)
            {
                string fileName = photos[i].tourism_photo1;
                if (fileName != "")
                {
                    System.IO.File.Delete(Server.MapPath("~/images/activity/" + fileName));
                }
                db.tourism_photo.Remove(photos[i]);
                db.SaveChanges();
            }
            return RedirectToAction("Index",new { page=Session["pg"] });
        }
        public ActionResult Edit(string id)
        {
            ActivityPhotoViewModel model = new ActivityPhotoViewModel() { 
            activity= db.activity.Where(m => m.activity_id == id).FirstOrDefault(),
            activity_photos = db.tourism_photo.Where(m => m.tourism_id == id).ToList()
            };
            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(string id, ActivityPhotoViewModel activityPhotoViewModel, HttpPostedFileBase[] tourism_photo, String oldImg)
        {
            
            db.Entry(activityPhotoViewModel.activity).State = EntityState.Modified;
            db.SaveChanges();            

            string fileName = "";
            var tp1 = db.tourism_photo.Where(m => m.tourism_id == id).ToList();

            for (int i = 0; i < 3; i++)
            {
                if (tourism_photo[i] != null)
                {
                    //如果有新增檔案到input
                    if (tourism_photo[i].ContentLength > 0)
                    {
                        //改名
                        string t=tourism_photo[i].FileName;
                        fileName = activityPhotoViewModel.activity.activity_id + "_" + DateTime.Now.ToString().Replace("/", "").Replace(":", "").Replace(" ", "") + (i + 1).ToString() + Path.GetExtension(t);
                        if (i < tp1.Count)  //如果原有紀錄
                        {
                            //刪掉原檔案
                            System.IO.File.Delete(Server.MapPath("~/images/activity/" + tp1[i].tourism_photo1));
                            tourism_photo[i].SaveAs(Server.MapPath("~/images/activity/" + fileName));      //將檔案存到該資料夾
                            //改掉資料庫檔案
                            tp1[i].tourism_photo1 = fileName;
                        }
                        else //如果原本沒有
                        {
                            tourism_photo tp = new tourism_photo();
                            tp.tourism_photo1 = fileName;
                            tourism_photo[i].SaveAs(Server.MapPath("~/images/activity/" + fileName));
                            tp.tourism_id = activityPhotoViewModel.activity.activity_id;
                            db.tourism_photo.Add(tp);
                        }
                    }
                }
            }
            db.SaveChanges();
            
            return RedirectToAction("Index",new { page=Session["pg"] });
        }

    }
}