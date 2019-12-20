using X.PagedList;
using X.PagedList.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TravelCat.Models;
using TravelCat.ViewModels;
using System.Web.Script.Serialization;

namespace TravelCat.Controllers
{
    public class SearchController : Controller
    {
        dbTravelCat db = new dbTravelCat();
        int pageSize = 10;

        //GET: Search
        public ActionResult Index(int page = 1, string q = null, string Sortby = null, string city = null, string comment_rating = null, string travel_partner = null, string travel_month = null, string comment_stay_total = null)
        {
            ViewBag.q = q;
            ViewBag.city = city;
            ViewBag.month = travel_month;
            ViewBag.partner = travel_partner;
            ViewBag.rating = comment_rating;
            ViewBag.stay = comment_stay_total;
            ViewBag.sort = Sortby;

            return View();
        }
        [ChildActionOnly]
        public ActionResult _Activity(int page = 1, string q = null, string Sortby = null, string city = null, string comment_rating = null, string travel_partner = null, string travel_month = null, string comment_stay_total = null)
        {
            ViewBag.q = q;
            //Session["pg"] = page;
            SearchViewModel model = new SearchViewModel()
            {
                activity = db.activity.Take(10).ToList(),
                //hotel = db.hotel.ToList(),
                //restaurant = db.restaurant.ToList(),
                //spot = db.spot.ToList(),
                result_ratings = null
            };

            if (!String.IsNullOrEmpty(q))
            {
                model.activity = db.activity.Where(s => s.activity_intro.Contains(q) || s.activity_title.Contains(q) || s.city.Contains(q) || s.district.Contains(q)).ToList();
                //model.restaurant = db.restaurant.Where(s => s.restaurant_intro.Contains(q) || s.restaurant_title.Contains(q) || s.city.Contains(q) || s.district.Contains(q)).ToList();
                //model.hotel = db.hotel.Where(s => s.hotel_intro.Contains(q) || s.hotel_title.Contains(q) || s.city.Contains(q) || s.district.Contains(q)).ToList();
                //model.spot = db.spot.Where(s => s.spot_intro.Contains(q) || s.spot_title.Contains(q) || s.city.Contains(q) || s.district.Contains(q)).ToList();
            }
            //找地區
            string type = "activity";
            search_city(type, city, model);
            //有評論分數的
            //單個表的篩選寫法 要加上join
            //select tourism_id,avg(comment_rating) as aver
            //from activity as a
            //join comment as c on a.activity_id = c.tourism_id
            //group by tourism_id
            //order by aver desc       
            //擴充
            //var activity_rating_result = (from a in db.activity
            //                     join b in db.comment on a.activity_id equals b.tourism_id
            //                     group b by b.tourism_id into g
            //                     orderby g.Average(s => s.comment_rating)
            //                     select new result_rating { id = g.Key, rating = g.Average(s => s.comment_rating).ToString() }).ToList();
            //但是改成多個表一起 不用join
            //select tourism_id, AVG(comment_rating) from comment group by tourism_id
            var rating_result = (from b in db.comment
                                 group b by b.tourism_id into g
                                 orderby g.Average(s => s.comment_rating)
                                 select new result_rating { id = g.Key, rating = g.Average(s => s.comment_rating).ToString() }).ToList();

            //沒有評論分數的
            //select a.activity_id,tourism_id
            //from activity as a left join comment as c
            //on a.activity_id = c.tourism_id
            //where c.tourism_id is null
            var activity_null_result = (from a in db.activity
                                        join b in db.comment on a.activity_id equals b.tourism_id into x
                                        from b in x.DefaultIfEmpty()
                                        where b.tourism_id == null
                                        select new result_rating { id = a.activity_id, rating = b.tourism_id }).ToList();
            //var hotel_null_result = (from a in db.hotel
            //                         join b in db.comment on a.hotel_id equals b.tourism_id into x
            //                         from b in x.DefaultIfEmpty()
            //                         where b.tourism_id == null
            //                         select new result_rating { id = a.hotel_id, rating = b.tourism_id }).ToList();
            //var restaurant_null_result = (from a in db.restaurant
            //                              join b in db.comment on a.restaurant_id equals b.tourism_id into x
            //                              from b in x.DefaultIfEmpty()
            //                              where b.tourism_id == null
            //                              select new result_rating { id = a.restaurant_id, rating = b.tourism_id }).ToList();
            //var spot_null_result = (from a in db.spot
            //                        join b in db.comment on a.spot_id equals b.tourism_id into x
            //                        from b in x.DefaultIfEmpty()
            //                        where b.tourism_id == null
            //                        select new result_rating { id = a.spot_id, rating = b.tourism_id }).ToList();

            List<result_rating> result = rating_result.Union(activity_null_result).OrderByDescending(s => s.rating).ToList();
          

            model.result_ratings = model.activity.Join(result, a => a.activity_id, b => b.id,
                                                                    (a, b) => new result_rating
                                                                    {
                                                                        id = a.activity_id,
                                                                        title = a.activity_title,
                                                                        intro = a.activity_intro,
                                                                        rating = b.rating
                                                                    }).ToList();
            //找評分
            if (!String.IsNullOrEmpty(comment_rating))
            {
                ViewBag.rating = comment_rating;
                double rating = Double.Parse(comment_rating); //查詢條件轉型
                model.result_ratings = model.result_ratings.Where(x => !String.IsNullOrEmpty(x.rating) && Double.Parse(x.rating) >= rating).ToList();
                //for (int i = 0; i < model.result_ratings.Count; i++)
                //{
                //    if (String.IsNullOrEmpty(model.result_ratings[i].rating))
                //    {
                //        model.result_ratings.RemoveAt(i);
                //        i = i - 1;
                //    }
                //    else
                //    {
                //        double rating_avg = Double.Parse(model.result_ratings[i].rating);
                //        if (rating_avg < rating)
                //        {
                //            model.result_ratings.RemoveAt(i);
                //            i = i - 1;
                //        }
                //    }
                //}
            }
            //找平均時間
            search_stay(comment_stay_total, model);
            //旅伴
            search_partner(travel_partner, model);
            //搜尋月份
            search_month(travel_month, model);
            //依照分數排序
            if (!String.IsNullOrEmpty(Sortby))
            {
                ViewBag.sort = Sortby;
                if (Sortby == "htol")
                {
                    model.result_ratings = model.result_ratings.OrderByDescending(s => s.rating).ToList();
                }
                else if (Sortby == "ltoh")
                {
                    model.result_ratings = model.result_ratings.OrderBy(s => s.rating).ToList();
                }
            }

            ViewBag.counta = model.result_ratings.Count;
            model.show_ratings = model.result_ratings.ToPagedList(page, pageSize);
            return View(model);
        }
        [ChildActionOnly]
        public ActionResult _Hotel(int page = 1, string q = null, string Sortby = null, string city = null, string comment_rating = null, string travel_partner = null, string travel_month = null, string comment_stay_total = null)
        {
            ViewBag.q = q;
            SearchViewModel model = new SearchViewModel()
            {
                hotel = db.hotel.Take(10).ToList(),
                result_ratings = null
            };

            if (!String.IsNullOrEmpty(q))
            {
                model.hotel = db.hotel.Where(s => s.hotel_intro.Contains(q) || s.hotel_title.Contains(q) || s.city.Contains(q) || s.district.Contains(q)).ToList();
            }
            //找地區
            string type = "hotel";
            search_city(type, city, model);

            //有評論分數的
            var rating_result = (from b in db.comment
                                 group b by b.tourism_id into g
                                 orderby g.Average(s => s.comment_rating)
                                 select new result_rating { id = g.Key, rating = g.Average(s => s.comment_rating).ToString() }).ToList();
            //沒有評論分數的
            var hotel_null_result = (from a in db.hotel
                                     join b in db.comment on a.hotel_id equals b.tourism_id into x
                                     from b in x.DefaultIfEmpty()
                                     where b.tourism_id == null
                                     select new result_rating { id = a.hotel_id, rating = b.tourism_id }).ToList();
            List<result_rating> result = rating_result.Union(hotel_null_result).OrderByDescending(s => s.rating).ToList();

            model.result_ratings = model.hotel.Join(result, a => a.hotel_id, b => b.id,
                                                                    (a, b) => new result_rating
                                                                    {
                                                                        id = a.hotel_id,
                                                                        title = a.hotel_title,
                                                                        intro = a.hotel_intro,
                                                                        rating = b.rating
                                                                    }).ToList();
            //找評分
            if (!String.IsNullOrEmpty(comment_rating))
            {
                ViewBag.rating = comment_rating;
                double rating = Double.Parse(comment_rating); //查詢條件轉型
                model.result_ratings = model.result_ratings.Where(x => !String.IsNullOrEmpty(x.rating) && Double.Parse(x.rating) >= rating).ToList();
            }

            //找平均時間
            search_stay(comment_stay_total, model);
            //旅伴
            search_partner(travel_partner, model);
            //搜尋月份
            search_month(travel_month, model);
            //依照分數排序
            if (!String.IsNullOrEmpty(Sortby))
            {
                if (Sortby == "htol")
                {
                    model.result_ratings = model.result_ratings.OrderByDescending(s => s.rating).ToList();
                }
                else if (Sortby == "ltoh")
                {
                    model.result_ratings = model.result_ratings.OrderBy(s => s.rating).ToList();
                }
            }
            ViewBag.counth = model.result_ratings.Count;
            model.show_ratings = model.result_ratings.ToPagedList(page, pageSize);
            return View(model);

        }
        [ChildActionOnly]
        public ActionResult _Restaurant(int page = 1, string q = null, string Sortby = null, string city = null, string comment_rating = null, string travel_partner = null, string travel_month = null, string comment_stay_total = null)
        {
            ViewBag.q = q;
            SearchViewModel model = new SearchViewModel()
            {
                restaurant = db.restaurant.Take(10).ToList(),
                result_ratings = null
            };

            if (!String.IsNullOrEmpty(q))
            {
                model.restaurant = db.restaurant.Where(s => s.restaurant_intro.Contains(q) || s.restaurant_title.Contains(q) || s.city.Contains(q) || s.district.Contains(q)).ToList();
            }
            //找地區
            string type = "restaurant";
            search_city(type, city, model);
            //有評論分數的
            var rating_result = (from b in db.comment
                                 group b by b.tourism_id into g
                                 orderby g.Average(s => s.comment_rating)
                                 select new result_rating { id = g.Key, rating = g.Average(s => s.comment_rating).ToString() }).ToList();

            //沒有評論分數的
            var restaurant_null_result = (from a in db.restaurant
                                          join b in db.comment on a.restaurant_id equals b.tourism_id into x
                                          from b in x.DefaultIfEmpty()
                                          where b.tourism_id == null
                                          select new result_rating { id = a.restaurant_id, rating = b.tourism_id }).ToList();

            List<result_rating> result = rating_result.Union(restaurant_null_result).OrderByDescending(s => s.id).ToList();

            model.result_ratings = model.restaurant.Join(result, a => a.restaurant_id, b => b.id,
                                                                    (a, b) => new result_rating
                                                                    {
                                                                        id = a.restaurant_id,
                                                                        title = a.restaurant_title,
                                                                        intro = a.restaurant_intro,
                                                                        rating = b.rating
                                                                    }).ToList();
            //找評分
            if (!String.IsNullOrEmpty(comment_rating))
            {
                ViewBag.rating = comment_rating;
                double rating = Double.Parse(comment_rating); //查詢條件轉型
                model.result_ratings = model.result_ratings.Where(x => !String.IsNullOrEmpty(x.rating) && Double.Parse(x.rating) >= rating).ToList();
            }
            //找平均時間
            search_stay(comment_stay_total, model);
            //旅伴
            search_partner(travel_partner, model);
            //搜尋月份
            search_month(travel_month, model);
            //依照分數排序
            if (!String.IsNullOrEmpty(Sortby))
            {
                ViewBag.sort = Sortby;
                if (Sortby == "htol")
                {
                    model.result_ratings = model.result_ratings.OrderByDescending(s => s.rating).ToList();
                }
                else if (Sortby == "ltoh")
                {
                    model.result_ratings = model.result_ratings.OrderBy(s => s.rating).ToList();
                }
            }
            ViewBag.countr = model.result_ratings.Count;
            ViewBag.count = model.result_ratings.Count;
            model.show_ratings = model.result_ratings.ToPagedList(page, pageSize);
            return View(model);
        }
        [ChildActionOnly]
        public ActionResult _Spot(int page = 1, string q = null, string Sortby = null, string city = null, string comment_rating = null, string travel_partner = null, string travel_month = null, string comment_stay_total = null)
        {
            ViewBag.q = q;
            SearchViewModel model = new SearchViewModel()
            {
                spot = db.spot.Take(10).ToList(),
                result_ratings = null
            };

            if (!String.IsNullOrEmpty(q))
            {
                model.spot = db.spot.Where(s => s.spot_intro.Contains(q) || s.spot_title.Contains(q) || s.city.Contains(q) || s.district.Contains(q)).ToList();
            }
            //找地區
            string type = "spot";
            search_city(type, city, model);
            //有評論分數的
            var rating_result = (from b in db.comment
                                 group b by b.tourism_id into g
                                 orderby g.Average(s => s.comment_rating)
                                 select new result_rating { id = g.Key, rating = g.Average(s => s.comment_rating).ToString() }).ToList();
            //沒有評論分數的
            var spot_null_result = (from a in db.spot
                                    join b in db.comment on a.spot_id equals b.tourism_id into x
                                    from b in x.DefaultIfEmpty()
                                    where b.tourism_id == null
                                    select new result_rating { id = a.spot_id, rating = b.tourism_id }).ToList();

            List<result_rating> result = rating_result.Union(spot_null_result).OrderByDescending(s => s.id).ToList();

            model.result_ratings = model.spot.Join(result, a => a.spot_id, b => b.id,
                                                                    (a, b) => new result_rating
                                                                    {
                                                                        id = a.spot_id,
                                                                        title = a.spot_title,
                                                                        intro = a.spot_intro,
                                                                        rating = b.rating
                                                                    }).ToList();
            //找評分
            if (!String.IsNullOrEmpty(comment_rating))
            {
                ViewBag.rating = comment_rating;
                double rating = Double.Parse(comment_rating); //查詢條件轉型
                model.result_ratings = model.result_ratings.Where(x => !String.IsNullOrEmpty(x.rating) && Double.Parse(x.rating) >= rating).ToList();
            }
            //找平均時間
            search_stay(comment_stay_total, model);
            //旅伴
            search_partner(travel_partner, model);
            //搜尋月份
            search_month(travel_month, model);
            //依照分數排序
            search_sort(Sortby, model);

            ViewBag.counts = model.result_ratings.Count;
            ViewBag.count = model.result_ratings.Count;
            model.show_ratings = model.result_ratings.ToPagedList(page, pageSize);
            return View(model);
        }

        private SearchViewModel search_sort(string Sortby, SearchViewModel model)
        {
            if (!String.IsNullOrEmpty(Sortby))
            {
                ViewBag.sort = Sortby;
                if (Sortby == "htol")
                {
                    model.result_ratings = model.result_ratings.OrderByDescending(s => s.rating).ToList();
                }
                else if (Sortby == "ltoh")
                {
                    model.result_ratings = model.result_ratings.OrderBy(s => s.rating).ToList();
                }
            }
            return model;
        }
        private SearchViewModel search_city(string type, string city, SearchViewModel model)
        {
            if (!String.IsNullOrEmpty(city))
            {
                ViewBag.city = city;
                switch (type)
                {
                    case "activity":
                        for (int i = 0; i < model.activity.Count; i++)
                        {
                            if (String.IsNullOrEmpty(model.activity[i].city) && String.IsNullOrEmpty(model.activity[i].district))
                            {
                                model.activity.RemoveAt(i);
                                i -= 1;
                            }
                            else if (!model.activity[i].city.Contains(city) && !model.activity[i].district.Contains(city))
                            {
                                model.activity.RemoveAt(i);
                                i -= 1;
                            }
                        }
                        break;
                    case "hotel":
                        for (int i = 0; i < model.hotel.Count; i++)
                        {
                            if (String.IsNullOrEmpty(model.hotel[i].city) && String.IsNullOrEmpty(model.hotel[i].district))
                            {
                                model.hotel.RemoveAt(i);
                                i -= 1;
                            }
                            else if (!model.hotel[i].city.Contains(city) && !model.hotel[i].district.Contains(city))
                            {
                                model.hotel.RemoveAt(i);
                                i -= 1;
                            }
                        }
                        break;
                    case "restaurant":
                        for (int i = 0; i < model.restaurant.Count; i++)
                        {
                            if (String.IsNullOrEmpty(model.restaurant[i].city) && String.IsNullOrEmpty(model.restaurant[i].district))
                            {
                                model.restaurant.RemoveAt(i);
                                i -= 1;
                            }
                            else if (!model.restaurant[i].city.Contains(city) && !model.restaurant[i].district.Contains(city))
                            {
                                model.restaurant.RemoveAt(i);
                                i -= 1;
                            }
                        }
                        break;
                    case "spot":
                        for (int i = 0; i < model.spot.Count; i++)
                        {
                            if (String.IsNullOrEmpty(model.spot[i].city) && String.IsNullOrEmpty(model.spot[i].district))
                            {
                                model.spot.RemoveAt(i);
                                i -= 1;
                            }
                            if (!model.spot[i].city.Contains(city))
                            {
                                model.spot.RemoveAt(i);
                                i -= 1;
                            }
                        }
                        break;
                }
            }

            return model;
        }
        private SearchViewModel search_month(string travel_month, SearchViewModel model)
        {
            if (travel_month != null)
            {
                ViewBag.month = travel_month;
                string id;
                string[] month = new string[3];
                switch (travel_month)
                {
                    case "3至5月":
                        month[0] = "3";
                        month[1] = "4";
                        month[2] = "5";
                        break;
                    case "6至8月":
                        month[0] = "6";
                        month[1] = "7";
                        month[2] = "8";
                        break;
                    case "9至11月":
                        month[0] = "9";
                        month[1] = "10";
                        month[2] = "11";
                        break;
                    case "12至2月":
                        month[0] = "12";
                        month[1] = "1";
                        month[2] = "2";
                        break;
                }
                for (int i = 0; i < model.result_ratings.Count; i++)
                {
                    bool is_exist = false;
                    id = model.result_ratings[i].id;
                    model.comment = db.comment.Where(s => s.tourism_id == id).ToList();
                    for (int j = 0; j < model.comment.Count; j++)   //某個活動內的某個留言
                    {
                        for (int x = 0; x < month.Length; x++) //找這個留言內有沒有我們要的月份
                        {
                            int result = model.comment.Where(s => s.travel_month == month[x]).Count();
                            if (result != 0)
                            {
                                is_exist = true;  //只要有一項就代表活動符合資格
                            }
                        }
                    }
                    if (!is_exist)
                    {
                        model.result_ratings.RemoveAt(i);     //不符合資格的移除
                        i = i - 1;
                    }
                }
            }
            return model;
        }
        private SearchViewModel search_partner(string travel_partner, SearchViewModel model)
        {
            if (travel_partner != null)
            {
                ViewBag.partner = travel_partner;
                string id;
                for (int i = 0; i < model.result_ratings.Count; i++)
                {
                    bool is_exist = false;
                    id = model.result_ratings[i].id;
                    model.comment = db.comment.Where(s => s.tourism_id == id).ToList();
                    for (int j = 0; j < model.comment.Count; j++)   //找對於單個活動的所有評論內是否有含搜尋選項
                    {
                        int result = model.comment.Where(s => s.travel_partner == travel_partner).Count();
                        if (result != 0)
                        {
                            is_exist = true;  //只要有一項就代表活動符合資格
                        }
                    }
                    if (!is_exist)
                    {
                        model.result_ratings.RemoveAt(i);     //不符合資格的移除
                        i = i - 1;
                    }
                }
            }

            return model;
        }
        private SearchViewModel search_stay(string comment_stay_total, SearchViewModel model)
        {
            if (!String.IsNullOrEmpty(comment_stay_total))
            {
                string id;
                ViewBag.stay = comment_stay_total;
                for (int i = 0; i < model.result_ratings.Count; i++)
                {
                    id = model.result_ratings[i].id;
                    int cmt_count = db.comment.Where(s => s.tourism_id == id).Count();
                    if (cmt_count == 0)
                    {
                        model.result_ratings.RemoveAt(i);
                        i = i - 1;
                    }
                    else
                    {
                        //找每個活動的平均
                        double time_avg = db.comment.Where(s => s.tourism_id == id).Average(r => r.comment_rating);
                        switch (comment_stay_total)
                        {
                            case "三到四小時":
                                if (time_avg > 4 || time_avg < 3)
                                {
                                    model.result_ratings.RemoveAt(i);
                                    i = i - 1;
                                }
                                break;
                            case "一到兩小時":
                                if (time_avg < 1 || time_avg > 2)
                                {
                                    model.result_ratings.RemoveAt(i);
                                    i = i - 1;
                                }
                                break;
                            case "兩到三小時":
                                if (time_avg < 2 || time_avg > 3)
                                {
                                    model.result_ratings.RemoveAt(i);
                                    i = i - 1;
                                }
                                break;
                            case "四小時以上":
                                if (time_avg < 4)
                                {
                                    model.result_ratings.RemoveAt(i);
                                    i = i - 1;
                                }
                                break;
                        }
                    }
                }
            }
            return model;
        }


        //水軍遊樂場
        public ActionResult generate_comment()
        {
            //List<spot> spots = db.spot.OrderBy(m => Guid.NewGuid()).ToList();
            //List<activity> activitys = db.activity.OrderBy(m => Guid.NewGuid()).ToList();
            //List<hotel> hotels = db.hotel.OrderBy(m => Guid.NewGuid()).ToList();
            List<restaurant> restaurants = db.restaurant.OrderBy(m => Guid.NewGuid()).ToList();
            //string id = "";
            string[] title = { "東京旅遊觀落櫻","吳敦義最愛的旅遊景點","爬山看風景","最美的風景","台中文昌廟","全家一日遊","超健康行程","招喚師峽谷","吃喝玩樂"};
            string[] content = {"人 人 人 人 人 人 人 人 人人人人人 人 人 人 人 人人人 人 人 八 八 人人人人人 人人人人人 人 人 人 人 人 人 人人人人人 人 人 人 人 人 人 人 人 人 嘛",
            "手銬的鬆緊度剛剛好，偵訊室相當的舒適，問筆錄態度良好，賓至如歸，如果有機會，還想在被貴所逮捕一次","東西精緻，也吃得出食材有用心，調味配料上再修正一點，不要吃到最後會有點膩就完美了","在審計368附近，想找一家來吃個下午茶；尋得這間日式甜甜圈店，造訪時來客數不少，點餐還排了小隊。看看整體氣氛跟小甜甜圈飾品，感覺很對味。內用低消一杯飲料，飲料最便宜有50元可選，還算有誠意。","帶前任去過，任何藥都治不了他的白痴行徑","跟前任去過，他只配喝廁所的水","帶前任去過，他真應該買一本最厚的書，然後狂打他後腦杓，活該"};
            string[] date = { "2010-07-26", "2016-01-07", "2013-03-08", "2015-11-30", "2018-08-23", "2019-12-05", "2019-12-05", "2019-01-05", "2019-02-05", "2019-03-03", "2019-04-07", "2019-05-19", "2019-06-03", "2019-07-22", "2019-08-13", "2019-09-25", "2019-10-15", "2019-11-07", "2019-12-05" };
            string[] photo = { "comment (1).jpg", "comment (2).jpg", "comment (3).jpg", "comment (4).jpg", "comment (5).jpg", "comment (6).jpg", "comment (7).jpg", "comment (8).jpg", "comment (9).jpg", "comment (10).jpg", "comment (11).jpg", "comment (12).jpg", "comment (13).jpg", "comment (14).jpg", "comment (15).jpg", "comment (16).jpg", "comment (17).jpg", "comment (18).jpg", "comment (19).jpg", "comment (20).jpg", "comment (21).jpg", "comment (22).jpg", "comment (23).jpg", "comment (24).jpg", "comment (25).jpg", "comment (26).jpg", "comment (27).jpg", "comment (28).jpg", "comment (29).jpg", "comment (30).jpg", "comment (31).jpg", "comment (32).jpg", "comment (33).jpg", "comment (34).jpg", "comment (35).jpg", "comment (36).jpg", "comment (37).jpg", "comment (38).jpg", "comment (39).jpg", "comment (40).jpg", "comment (41).jpg", "comment (42).jpg", "comment (43).jpg",  "comment (44).jpg"};
            string[] memberID = { "M000001", "M000002", "M000003", "M000004", "M000005", "M000006", "M000007", "M000008", "M000009", "M000010", "M000011", "M000012", "M000013", "M000014", "M000015", "M000016", "M000017", "M000018", "M000019", "M000020", "M000021", "M000022", "M000023", "M000024", "M000025", "M000026", "M000027", "M000028", "M000029" };
            string[] partner = { "蜜月", "伴侶", "朋友", "商務", "家庭" };

            Random random = new Random();

            for (int i = 0; i < 2000; i++)
            {
                comment cmt = new comment();
                DateTime parsedDate = DateTime.Parse(date[random.Next(0, 18)]);

                //cmt.tourism_id = spots[i].spot_id;
                //cmt.tourism_id = activitys[i].activity_id;
                //cmt.tourism_id = hotels[i].hotel_id;
                cmt.tourism_id = restaurants[i].restaurant_id;
                cmt.comment_title = title[random.Next(0, 9)];
                cmt.comment_content = content[random.Next(0, 6)];
                cmt.comment_date = parsedDate;
                cmt.comment_photo = photo[random.Next(0, 44)];
                cmt.comment_stay_total = random.Next(1, 5);
                cmt.comment_rating = Convert.ToInt16(random.Next(1, 6));
                cmt.travel_month = random.Next(1, 13).ToString();
                cmt.comment_status = true;
                cmt.member_id = memberID[random.Next(0, 29)];
                cmt.travel_partner = partner[random.Next(0, 5)];

                if (ModelState.IsValid)
                {
                    db.comment.Add(cmt);
                    db.SaveChanges();
                }
            }



            return View();
        }
    }
}