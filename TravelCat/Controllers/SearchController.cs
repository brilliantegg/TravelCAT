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
            //int? count = (int?)TempData["counta"]+ (int?)TempData["counth"] + (int?)TempData["counts"] + (int?)TempData["countr"];
            //ViewBag.count = count;
            return View();
        }
        [ChildActionOnly]
        public ActionResult _Activity(int page = 1, string q = null, string Sortby = null, string city = null, string comment_rating = null, string travel_partner = null, string travel_month = null, string comment_stay_total = null)
        {
            ViewBag.q = q;
            //Session["pg"] = page;
            SearchViewModel model = new SearchViewModel()
            {
                comment = db.comment.ToList(),
                collections_detail = db.collections_detail.ToList(),
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
            if (!String.IsNullOrEmpty(city))
            {
                string type = "activity";
                search_city(type, city, model);
            }
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
            if (!String.IsNullOrEmpty(comment_stay_total))
            {
                search_stay(comment_stay_total, model);
            }
            //旅伴
            if (!String.IsNullOrEmpty(travel_partner))
            {
                search_partner(travel_partner, model);
            }
            //搜尋月份
            if (!String.IsNullOrEmpty(travel_month))
            {
                search_month(travel_month, model);
            }
            //依照分數排序
            if (!String.IsNullOrEmpty(Sortby))
            {
                search_sort(Sortby, model);
            }

            //ViewBag.counta = model.result_ratings.Count;
            //TempData["counta"] = model.result_ratings.Count();
            model.show_ratings = model.result_ratings.ToPagedList(page, pageSize);
            return View(model);
        }
        [ChildActionOnly]
        public ActionResult _Hotel(int page = 1, string q = null, string Sortby = null, string city = null, string comment_rating = null, string travel_partner = null, string travel_month = null, string comment_stay_total = null)
        {
            ViewBag.q = q;
            SearchViewModel model = new SearchViewModel()
            {
                comment = db.comment.ToList(),
                collections_detail = db.collections_detail.ToList(),
                hotel = db.hotel.Take(10).ToList(),
                result_ratings = null
            };

            if (!String.IsNullOrEmpty(q))
            {
                model.hotel = db.hotel.Where(s => s.hotel_intro.Contains(q) || s.hotel_title.Contains(q) || s.city.Contains(q) || s.district.Contains(q)).ToList();
            }
            //找地區
            if (!String.IsNullOrEmpty(city))
            {
                string type = "hotel";
                search_city(type, city, model);
            }

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
            if (!String.IsNullOrEmpty(comment_stay_total))
            {
                search_stay(comment_stay_total, model);
            }
            //旅伴
            if (!String.IsNullOrEmpty(travel_partner))
            {
                search_partner(travel_partner, model);
            }
            //搜尋月份
            if (!String.IsNullOrEmpty(travel_month))
            {
                search_month(travel_month, model);
            }
            //依照分數排序
            if (!String.IsNullOrEmpty(Sortby))
            {
                search_sort(Sortby, model);
            }


            model.show_ratings = model.result_ratings.ToPagedList(page, pageSize);
            return View(model);

        }
        [ChildActionOnly]
        public ActionResult _Restaurant(int page = 1, string q = null, string Sortby = null, string city = null, string comment_rating = null, string travel_partner = null, string travel_month = null, string comment_stay_total = null)
        {
            ViewBag.q = q;
            SearchViewModel model = new SearchViewModel()
            {
                comment = db.comment.ToList(),
                collections_detail = db.collections_detail.ToList(),
                restaurant = db.restaurant.Take(10).ToList(),
                result_ratings = null
            };

            if (!String.IsNullOrEmpty(q))
            {
                model.restaurant = db.restaurant.Where(s => s.restaurant_intro.Contains(q) || s.restaurant_title.Contains(q) || s.city.Contains(q) || s.district.Contains(q)).ToList();
            }
            //找地區
            if (!String.IsNullOrEmpty(city))
            {
                string type = "restaurant";
                search_city(type, city, model);
            }
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
            if (!String.IsNullOrEmpty(comment_stay_total))
            {
                search_stay(comment_stay_total, model);
            }
            //旅伴
            if (!String.IsNullOrEmpty(travel_partner))
            {
                search_partner(travel_partner, model);
            }
            //搜尋月份
            if (!String.IsNullOrEmpty(travel_month))
            {
                search_month(travel_month, model);
            }
            //依照分數排序
            if (!String.IsNullOrEmpty(Sortby))
            {
                search_sort(Sortby, model);
            }


            model.show_ratings = model.result_ratings.ToPagedList(page, pageSize);
            return View(model);
        }
        [ChildActionOnly]
        public ActionResult _Spot(int page = 1, string q = null, string Sortby = null, string city = null, string comment_rating = null, string travel_partner = null, string travel_month = null, string comment_stay_total = null)
        {
            ViewBag.q = q;
            SearchViewModel model = new SearchViewModel()
            {
                comment = db.comment.ToList(),
                collections_detail = db.collections_detail.ToList(),
                spot = db.spot.Take(10).ToList(),
                result_ratings = null
            };

            if (!String.IsNullOrEmpty(q))
            {
                model.spot = db.spot.Where(s => s.spot_intro.Contains(q) || s.spot_title.Contains(q) || s.city.Contains(q) || s.district.Contains(q)).ToList();
            }
            //找地區
            if (!String.IsNullOrEmpty(city))
            {
                string type = "spot";
                search_city(type, city, model);
            }
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
            if (!String.IsNullOrEmpty(comment_stay_total))
            {
                search_stay(comment_stay_total, model);
            }
            //旅伴
            if (!String.IsNullOrEmpty(travel_partner))
            {
                search_partner(travel_partner, model);
            }
            //搜尋月份
            if (!String.IsNullOrEmpty(travel_month))
            {
                search_month(travel_month, model);
            }
            //依照分數排序
            if (!String.IsNullOrEmpty(Sortby))
            {
                search_sort(Sortby, model);
            }


            model.show_ratings = model.result_ratings.ToPagedList(page, pageSize);
            return View(model);
        }

        private SearchViewModel search_sort(string Sortby, SearchViewModel model)
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

            return model;
        }
        private SearchViewModel search_partner(string travel_partner, SearchViewModel model)
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


            return model;
        }
        private SearchViewModel search_stay(string comment_stay_total, SearchViewModel model)
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
            return model;
        }


        //水軍遊樂場
        public ActionResult generate_comment()
        {
            List<comment> comments = db.comment.Where(s => s.tourism_id.Contains("H")).Take(1260).OrderBy(m => Guid.NewGuid()).ToList();
            //string id = "";
            string[] title = { "大說謊家","下禮拜要報告好緊張","錢多事少離家近 數錢數到手抽筋","熱氣球飛行家",
                                        "去年聖誕節","Google goes offline after fibre cables cut","布魯克林孤兒","遲到騎車還下雨 有夠難過","Robert Downey Jr launches YouTube doc featuring AI","Airbnb is not an estate agent, EU court rules","Inside the lives of Orthodox Jewish women","Why did an entire civilisation vanish?","鋒迴路轉","賽道狂人","82年生的金智英"};
            string[] content = { "Crews are battling over 100 fires amid a heatwave which has produced the nation's two hottest days on record.<br>Two volunteer firefighters died in a road accident on Thursday while deployed to a huge blaze near Sydney.<br>Mr Morrison said he would end his leave early.His absence this week has drawn condemnation and protests.",
                                               "德國時間周一（12月10日）晚上時份，一家生產巧克力的工廠，因巧克力罐洩漏，導致大量液態巧克力流出，覆蓋了大約10平方米（108平方英尺）的地面，令西部城鎮韋爾市的一條道路封閉。<br>由於巧克力迅速在地面凝固，25名消防員需要使用鐵鏟，熱水和噴槍清理現場，巧克力工廠的員工亦有協助清理。<br>當地消防部門說：「儘管發生了如此讓人沮喪的事情，這個聖誕節還是不能沒有巧克力。」<br>此家工廠之前保證，他們的工廠於周三恢復正常運營。",
                                               "多方分析指出，近年來大量歐盟和非歐盟移民湧入，是造成英國民間脫歐情緒高漲的主要原因之一。移民改革的第一步就是脫歐第一日起終止歐盟國家居民自由在英國居住與工作的權利。<br>約翰遜在競選期間就多次承諾，希望在英國實行澳大利亞式的移民打分制度，也就是說依照移民申請人的實力、能力等各類條件打分，然後按年度指標有秩序地吸收夠條件的人士移民。<br>新移民法提案還專門提出加快對英國老齡化社會所迫切需要的醫生、護士等醫療服務人員的簽證處理時間。",
                                               "除上述一些主要施政要點外，英國政府也希望能在新議會期間通過有關環保和氣候變化，社會養老，選舉制度改革等許多方面的新法。<br>觀察人士指出，由於執政保守黨目前在英國議會中佔有80席多數，因此可以基本確保通過任何立法提案。約翰遜首相政府也可以說是擁有自1987年撒切爾（戴卓爾）夫人首相政府以來的最強勢保守黨政府。<br>對英國政府來說，未來國內政治最大不確定因素應是由蘇格蘭民族黨不斷希望推動的獨立公投，而國際上則有脫歐之後與歐盟關係，以及如何在美中對抗加劇的今日，擺好英國立場的抉擇。",
                                               "Temperatures have exceeded 45C in several states this week, fanning bushfires in South Australia and NSW.<br>On Thursday, NSW Premier Gladys Berejiklian announced a seven-day state of emergency due to forecasts of worsening conditions. The intense heat is expected to persist into Saturday, forecasters warn.<br>The nation may have its all-time temperature record broken again after it was smashed twice this week. It hit a high of 41.9C (107.4F) on Wednesday.<br>Of the firefighters' deaths, the NSW Rural Fire Service said: 'This is an absolutely devastating event in what has already been an incredibly difficult day and fire season.'<br>",
                                               "教育部國民及學前教育署今天組成專案小組到長榮中學瞭解狀況，晚間得出初步結論。國教署告訴媒體，校方表示將從2個途徑籌措財源，包括持續與銀行接洽借款；另外，基督教長老總會預計於25日派人到校，校方會努力向其爭取資金挹注。<br>長榮中學於今年8月傳出未與教師協議即刪減學術研究費，引起教師抗爭，教育部也3度發文要求改善。教育部表示，校方於明天下午3時與工會代表進行團體協議，將關注後續結果。<br>教育部表示，期望長榮中學的董事會、行政團隊與教師作理性溝通，啟動協商，共同面對學生減少的情勢，規劃學校轉型，共謀校務創新發展途徑。",
                                               "故事主角是一個不曾相信過任何人的職業騙徒：羅伊寇特尼(伊恩麥克連 飾)，他一生中無所不騙，心狠手辣毫不留情，而且不留下任何痕跡。某日他在網路上認識一名有錢的寡婦：貝蒂麥雷許(海倫米蘭 飾)。兩人見面後相談甚歡，毫無疑問地，貝蒂很快就被羅伊幽默風趣的談吐與溫柔貼心的態度所吸引，但出乎羅伊意料的是貝蒂竟是他這一生中見過最聰明、優雅、風趣而且有品味的女性；貝蒂對羅伊敞開心胸毫不隱瞞，大方地打開大門邀請他進入她家與她的人生中，此時的羅伊開始驚訝地發現自己竟然是真心喜歡貝蒂，他一生中未曾有過如此心動的感覺，讓原本按照計畫應該發生的騙局，竟在最後的轉瞬間峰迴路轉，面對這個一生一次能讓自己動心的女人，羅伊將會做出什麼樣的選擇？",
                                               "真人實事改編，19世紀科學家詹姆斯葛萊舍（艾迪瑞德曼 飾）與熱氣球駕駛員艾蜜莉雷恩（費莉絲蒂瓊斯 飾）挑戰「飛」出人類極限高度。為了實驗熱氣球可飛行高度，科學家詹姆斯帶著飛行員艾蜜莉一同實驗，而在飛行途中發生一連串的狀況與事件，隨著高度越高，空氣越來越稀薄，兩人該如何面對接下來的挑戰？",
                                               "五歲的星星（陳品嫙 飾）有個跟別人不一樣的媽媽小青（姚愛寗 飾），愛笑的小青最喜歡跟星星一起玩耍，相依為命的兩人總在一聲聲的「謝謝」與「對不起」中尋求大家的認同與諒解。小青為了生活，帶著星星到市場打工，卻無意惹禍上身，開啟了一連串的麻煩事，甚至丟了工作，還引起新聞媒體的注意，爆出小青不堪的過往……。<br>在社會輿論的壓力下，社會局決定安排星星到寄養家庭，然而這個表面上《最好的安排》，竟成了小青母女倆《最壞的決定》。面對現實的壓迫與社會的歧視，小青決心帶著星星離開，但他們究竟該何去何從呢？<br>今年冬天最動人的溫馨電影《為你存在的每一天》，由資深監製曾禎執導，新生代演員群姚愛寗、黃遠、于樂誠及天才童星陳品嫙主演，本片大膽碰觸國片少見的社會弱勢議題，以及主演演技加持，近日已榮獲洛杉磯菲斯蒂喬斯國際影展六項大獎、義大利奧尼羅電影獎最佳劇情片、倫敦獨立電影獎兩項大獎以及入圍美國獨立電影獎最佳亞洲劇情片。《為你存在的每一天》將於12月13日全台閃耀上映，更多電影資訊請上官方粉絲團查詢。",
                                               "一事無成的凱特（艾蜜莉亞克拉克 飾）在倫敦渾渾噩噩地過日子，她做出一連串的糟糕決定導致的厄運總是伴隨著鞋子上發出的鈴聲如影隨形地跟著她，因為她只能打扮成聖誕老公公的小精靈，在一間全年無休的聖誕飾品店當店員。於是當心地善良的大帥哥湯姆（亨利高汀 飾）闖入凱特的人生，並且開始幫助她克服她人生中的許多障礙與難關，這一切都似乎美好得有點不真實。當倫敦在聖誕佳節期間轉變成一座童話般的夢幻王國，這兩個看似八竿子打不著的人應該不會結成良緣，但有時候你得順其自然、聆聽內心的聲音…而且一定要擁有信念。<br>《去年聖誕節》一片中有許多喬治麥可的經典名曲，當然包括與電影同名，詞意苦中帶甜的經典聖誕歌曲。這部電影中也有一些這位曾經榮獲多項葛萊美獎的傳奇歌手從未發表過的全新歌曲。喬治麥可在輝煌的演藝生涯中，總共賣出1億1千5百萬張專輯，並且擁有10首冠軍單曲。",
                                               "萊諾艾斯洛（艾德華諾頓 飾），一名患有妥瑞症的孤獨私家偵探，他的良師和唯一朋友法蘭克敏納（布魯斯威利 飾）遭到謀殺，他冒著生命危險設法破案。他手上只有寥寥無幾的線索，不過他滿懷衝勁，解開了層層掩飾的重大祕密，而那攸關著能否讓紐約市維持和諧的命運。在一連串的神祕事件中，他從哈林區醉生夢死的爵士俱樂部，來到布魯克林區邊緣化的貧民窟，最後來到紐約權力經紀人的陣地，萊諾必須對付流氓、腐敗和全市最危險的人，為的只是向他的朋友致敬，並拯救可能會讓他獲得救贖的女人。"};
            string[] date = { "2019-08-07", "2019-01-05", "2016-05-20", "2019-12-13", "2018-08-23", "2019-12-20", "2019-09-26" };
            string[] photo = { "01_ash.png", "02_candy_crush.jpg", "03_goodboy.jpg", "G4wXXRk.gif", "12_goodguy.png", "01_sleep.jpg", "01_sleeves.jpg", "05_gaming.jpg", "05_report.jpg", "06_gf_zero.jpg", "10_crow.jpg", "11_math.jpg", "11_spiderman.jpg", "13_ad.jpg", "16_useful.jpg", "19_hell.jpg", "20_bean.jpg" };
            //string[] memberID = { "M000001", "M000002", "M000003", "M000004", "M000005" };
            string[] partner = { "蜜月", "伴侶", "朋友", "商務", "家庭" };

            Random random = new Random();

            for (int i = 0; i < 400; i++)
            {
                comment cmt = new comment();
                DateTime parsedDate = DateTime.Parse(date[random.Next(0, 7)]);

                cmt.tourism_id = comments[i].tourism_id;
                cmt.comment_title = title[random.Next(0, 15)];
                cmt.comment_content = content[random.Next(0, 11)];
                cmt.comment_date = parsedDate;
                cmt.comment_photo = photo[random.Next(0, 17)];
                cmt.comment_stay_total = random.Next(1, 5);
                cmt.comment_rating = Convert.ToInt16(random.Next(3, 6));
                cmt.travel_month = random.Next(1, 13).ToString();
                cmt.comment_status = true;
                cmt.member_id = db.member.ToList()[random.Next(0, 30)].member_id;
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