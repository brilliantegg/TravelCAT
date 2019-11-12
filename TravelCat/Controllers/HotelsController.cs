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
    public class HotelsController : Controller
    {
        dbTravelCat db = new dbTravelCat();
        int pageSize = 10;

        // GET: Hotels
        public ActionResult Index(string id = null, int page = 1)
        {
            ViewBag.id = id;
            Session["pg"]=page;
            //int pageNumber = (page ?? 1);

            if (!String.IsNullOrEmpty(id))
            {
                var search = db.hotel.Where(s => s.hotel_id.Contains(id) || s.hotel_title.Contains(id)
               || s.city.Contains(id) || s.district.Contains(id));
                return View(search.OrderBy(m => m.hotel_id).ToPagedList(page, pageSize));
            }
            else
            {
                var data = db.hotel.OrderBy(m => m.hotel_id);
                return View(data.ToPagedList(page, pageSize));
            }
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]    
        public ActionResult Create(hotel hotel, HttpPostedFileBase[] tourism_photo)     
        {
            string h_id = db.Database.SqlQuery<string>("Select dbo.GethotelId()").FirstOrDefault();
            hotel.hotel_id = h_id;

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
                        fileName = hotel.hotel_id + "_" +DateTime.Now.ToString().Replace("/", "").Replace(":", "").Replace(" ", "") + (i + 1).ToString() + Path.GetExtension(t);
                        f.SaveAs(Server.MapPath("~/images/hotel/" + fileName));
                        tourism_photo tp = new tourism_photo();
                        tp.tourism_photo1 = fileName;
                        tp.tourism_id = hotel.hotel_id;
                        db.tourism_photo.Add(tp);
                    }
                }
            }
            db.hotel.Add(hotel);
            db.SaveChanges();


            return RedirectToAction("Index",new { page=Session["pg"] });
        }

        public ActionResult Delete(string Id)
        {
            var product = db.hotel.Where(m => m.hotel_id == Id).FirstOrDefault();
            db.hotel.Remove(product);
            db.SaveChanges();

            var photos = db.tourism_photo.Where(m => m.tourism_id == Id).ToList();
            for (int i = 0; i < photos.Count; i++)
            {
                string fileName = photos[i].tourism_photo1;
                if (fileName != "")
                {
                    System.IO.File.Delete(Server.MapPath("~/images/hotel/" + fileName));
                }
                db.tourism_photo.Remove(photos[i]);
                db.SaveChanges();
            }
            return RedirectToAction("Index",new { page=Session["pg"] });
        }
        public ActionResult Edit(string id)
        {
            HotelPhotoViewModel model = new HotelPhotoViewModel()
            {
                hotel = db.hotel.Where(m => m.hotel_id == id).FirstOrDefault(),
                hotel_photos = db.tourism_photo.Where(m => m.tourism_id == id).ToList()
            };
            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(string id, HotelPhotoViewModel hotelPhotoViewModel, HttpPostedFileBase[] tourism_photo, String oldImg)
        {

            db.Entry(hotelPhotoViewModel.hotel).State = EntityState.Modified;
            db.SaveChanges();

            //圖片
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
                        string t = tourism_photo[i].FileName;
                        fileName = hotelPhotoViewModel.hotel.hotel_id + "_" + DateTime.Now.ToString().Replace("/", "").Replace(":", "").Replace(" ", "") + (i + 1).ToString() + Path.GetExtension(t);
                        if (i < tp1.Count)  //如果原有紀錄
                        {
                            //刪掉原檔案
                            System.IO.File.Delete(Server.MapPath("~/images/hotel/" + tp1[i].tourism_photo1));
                            tourism_photo[i].SaveAs(Server.MapPath("~/images/hotel/" + fileName));      //將檔案存到該資料夾
                            //改掉資料庫檔案
                            tp1[i].tourism_photo1 = fileName;
                        }
                        else //如果原本沒有
                        {
                            tourism_photo tp = new tourism_photo();
                            tp.tourism_photo1 = fileName;
                            tourism_photo[i].SaveAs(Server.MapPath("~/images/hotel/" + fileName));
                            tp.tourism_id = hotelPhotoViewModel.hotel.hotel_id;
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