using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TravelCat.Models;
using TravelCat.ViewModels;
using X.PagedList;
using System.IO;

namespace TravelCat.Controllers
{
    [LoginCheck]
    public class RestaurantsController : Controller
    {
        dbTravelCat db = new dbTravelCat();
        int pageSize = 10;

        // GET: Hotels
        public ActionResult Index(string id = null, int page = 1)
        {
            ViewBag.id = id;
            Session["pg"]=page;

            if (!String.IsNullOrEmpty(id))
            {
                var search = db.restaurant.Where(s => s.restaurant_id.Contains(id) || s.restaurant_title.Contains(id)
               || s.city.Contains(id) || s.district.Contains(id));
                return View(search.OrderBy(m => m.restaurant_id).ToPagedList(page, pageSize));
            }
            else
            {
                var data = db.restaurant.OrderBy(m => m.restaurant_id);
                return View(data.ToPagedList(page, pageSize));
            }
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(restaurant restaurant, HttpPostedFileBase[] tourism_photo)
        {
            string act_id = db.Database.SqlQuery<string>("Select dbo.GetrestId()").FirstOrDefault();
            restaurant.restaurant_id = act_id;

            string fileName = "";
            for (int i = 0; i < tourism_photo.Length; i++)
            {
                HttpPostedFileBase f = tourism_photo[i];
                if (f != null)
                {
                    if (f.ContentLength > 0)
                    {
                        string t = tourism_photo[i].FileName;
                        fileName = restaurant.restaurant_id + "_"+DateTime.Now.ToString().Replace("/", "").Replace(":", "").Replace(" ", "") + (i + 1).ToString() + Path.GetExtension(t);
                        f.SaveAs(Server.MapPath("~/images/restaurant/" + fileName));
                        tourism_photo tp = new tourism_photo();
                        tp.tourism_photo1 = fileName;
                        tp.tourism_id = restaurant.restaurant_id;
                        db.tourism_photo.Add(tp);
                    }
                }
            }
            db.restaurant.Add(restaurant);
            db.SaveChanges();


            return RedirectToAction("Index",new { page=Session["pg"] });
        }

        public ActionResult Delete(string Id)
        {
            var product = db.restaurant.Where(m => m.restaurant_id == Id).FirstOrDefault();
            db.restaurant.Remove(product);
            db.SaveChanges();

            var photos = db.tourism_photo.Where(m => m.tourism_id == Id).ToList();
            for (int i = 0; i < photos.Count; i++)
            {
                string fileName = photos[i].tourism_photo1;
                if (fileName != "")
                {
                    System.IO.File.Delete(Server.MapPath("~/images/restaurant/" + fileName));
                }
                db.tourism_photo.Remove(photos[i]);
                db.SaveChanges();
            }

            return RedirectToAction("Index",new { page=Session["pg"] });
        }
        public ActionResult Edit(string id)
        {
            RestaurantPhotoViewModel model = new RestaurantPhotoViewModel()
            {
                restaurant = db.restaurant.Where(m => m.restaurant_id == id).FirstOrDefault(),
                restaurant_photos = db.tourism_photo.Where(m => m.tourism_id == id).ToList()
            };
            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(string id, RestaurantPhotoViewModel restaurantPhotoViewModel, HttpPostedFileBase[] tourism_photo, String oldImg)
        {

            db.Entry(restaurantPhotoViewModel.restaurant).State = EntityState.Modified;
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
                        fileName = restaurantPhotoViewModel.restaurant.restaurant_id + "_" + DateTime.Now.ToString().Replace("/", "").Replace(":", "").Replace(" ", "") + (i + 1).ToString() + Path.GetExtension(t);
                        if (i < tp1.Count)  //如果原有紀錄
                        {
                            //刪掉原檔案
                            System.IO.File.Delete(Server.MapPath("~/images/restaurant/" + tp1[i].tourism_photo1));
                            tourism_photo[i].SaveAs(Server.MapPath("~/images/restaurant/" + fileName));      //將檔案存到該資料夾
                            //改掉資料庫檔案
                            tp1[i].tourism_photo1 = fileName;
                        }
                        else //如果原本沒有
                        {
                            tourism_photo tp = new tourism_photo();
                            tp.tourism_photo1 = fileName;
                            tourism_photo[i].SaveAs(Server.MapPath("~/images/restaurant/" + fileName));
                            tp.tourism_id = restaurantPhotoViewModel.restaurant.restaurant_id;
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