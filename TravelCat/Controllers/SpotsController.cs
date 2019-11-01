using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TravelCat.Models;
using TravelCat.ViewModels;
using PagedList;

namespace TravelCat.Controllers
{
    public class SpotController : Controller
    {
        // GET: Spot
        dbTravelCat db = new dbTravelCat();
        int pageSize = 10;

        public ActionResult Index(int? page)
        {
            int pageNumber = (page ?? 1);
            var data = db.spot.OrderBy(m => m.spot_id).ToPagedList(pageNumber, pageSize);
                       
            return View(data);                       
        }
        //public ActionResult contentQuery(string id)
        //{
        //    var search = from a in db.spots
        //                    select a;
        //    if (!String.IsNullOrEmpty(id))
        //    {
        //        search = search.Where(s => s.spot_id.Contains(id) || s.spot_title.Contains(id)
        //        || s.city.Contains(id) || s.district.Contains(id));
        //    }
        //    return View(search);
        //}

   

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(spot spot, HttpPostedFileBase tourism_photo)
        {
            string act_id = db.Database.SqlQuery<string>("Select dbo.GetspotId()").FirstOrDefault();
            spot.spot_id= act_id;

            string fileName = "";
            if (tourism_photo != null)
            {
                if (tourism_photo.ContentLength > 0)
                {
                    
                    fileName = System.IO.Path.GetFileName(tourism_photo.FileName);
                    tourism_photo.SaveAs(Server.MapPath("~/images/spot/" + fileName));
                }
            }

            tourism_photo tp = new tourism_photo();
            tp.tourism_photo1 = fileName;
            tp.tourism_id = spot.spot_id;

            spot.update_date = DateTime.Now;

            db.spot.Add(spot);
            db.tourism_photo.Add(tp);
            db.SaveChanges();


            return RedirectToAction("Index");
        }

        public ActionResult Delete(string Id)
        {
            var product = db.spot.Where(m => m.spot_id == Id).FirstOrDefault();
            db.spot.Remove(product);
            db.SaveChanges();

            var photos = db.tourism_photo.Where(m => m.tourism_id == Id).FirstOrDefault();
            string fileName = photos.tourism_photo1;
            if (fileName != "")
            {
                System.IO.File.Delete(Server.MapPath("~/images/spot/" + fileName));
            }
            db.tourism_photo.Remove(photos);
            db.SaveChanges();

            return RedirectToAction("Index");
        }
        public ActionResult Edit(string id)
        {
            SpotPhotoViewModel model = new SpotPhotoViewModel()
            {
                spot = db.spot.Where(m => m.spot_id == id).FirstOrDefault(),
                spot_photos = db.tourism_photo.Where(m => m.tourism_id == id).FirstOrDefault()
            };
            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(string id,SpotPhotoViewModel spotPhotoViewModel, HttpPostedFileBase tourism_photo, String oldImg)
        {

            //string update_time = DateTime.Now.ToShortDateString() + DateTime.Now.TimeOfDay.ToString();
            spotPhotoViewModel.spot.update_date = DateTime.Now;

            db.Entry(spotPhotoViewModel.spot).State = EntityState.Modified;
            db.SaveChanges();

            string fileName = "";
            if (tourism_photo != null)
            {
                if (tourism_photo.ContentLength > 0)
                {
                    System.IO.File.Delete(Server.MapPath("~/images/spot/" + oldImg));
                    fileName = System.IO.Path.GetFileName(tourism_photo.FileName);      //取得檔案的檔名(主檔名+副檔名)
                    tourism_photo.SaveAs(Server.MapPath("~/images/spot/" + fileName));      //將檔案存到該資料夾
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