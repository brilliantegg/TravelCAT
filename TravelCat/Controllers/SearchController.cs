using X.PagedList;
using X.PagedList.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TravelCat.Models;
using TravelCat.ViewModels;

namespace TravelCat.Controllers
{
    public class SearchController : Controller
    {
        dbTravelCat db = new dbTravelCat();
        //int pageSize = 15;

        //GET: Search
        public ActionResult Index(string q = null, int page = 1)
        {
            ViewBag.q = q;
            Session["pg"] = page;
            SearchViewModel model = new SearchViewModel();

            if (!String.IsNullOrEmpty(q))
            {
                model = new SearchViewModel
                {
                    activity = db.activity.Where(s => s.activity_intro.Contains(q) || s.activity_title.Contains(q) || s.city.Contains(q) || s.district.Contains(q)).ToList(),
                    hotel = db.hotel.Where(s => s.hotel_intro.Contains(q) || s.hotel_title.Contains(q) || s.city.Contains(q) || s.district.Contains(q)).ToList(),
                    restaurant = db.restaurant.Where(s => s.restaurant_intro.Contains(q) || s.restaurant_title.Contains(q) || s.city.Contains(q) || s.district.Contains(q)).ToList(),
                    spot = db.spot.Where(s => s.spot_intro.Contains(q) || s.spot_title.Contains(q) || s.city.Contains(q) || s.district.Contains(q)).ToList()
                };
            }
            else
            {
                model = new SearchViewModel
                {
                    activity = db.activity.OrderBy(s => s.activity_id).ToList(),
                    hotel = db.hotel.OrderBy(s => s.hotel_id).ToList(),
                    restaurant = db.restaurant.OrderBy(s => s.restaurant_id).ToList(),
                    spot = db.spot.OrderBy(s => s.spot_id).ToList()
                };
            }
            return View(model);
            //return View(model.activity.OrderBy(m => m.activity_id).ToPagedList(page, pageSize));
        }
        [ChildActionOnly]
        public ActionResult _Activity(string type = null, string q = null, string Sortby = null, string city = null, string comment_rating = null, string[] travel_partner = null, string travel_month = null, string comment_stay_total = null)
        {
            ViewBag.q = q;
            //Session["pg"] = page;
            SearchViewModel model = new SearchViewModel
            {
                activity = db.activity.OrderBy(s => s.activity_title).ToList(),
                hotel = db.hotel.OrderBy(s => s.hotel_title).ToList(),
                restaurant = db.restaurant.OrderBy(s => s.restaurant_title).ToList(),
                spot = db.spot.OrderBy(s => s.spot_title).ToList(),
                result_ratings = null
            };
            if (!String.IsNullOrEmpty(q))
            {
                model.activity = db.activity.Where(s => s.activity_intro.Contains(q) || s.activity_title.Contains(q) || s.city.Contains(q) || s.district.Contains(q)).ToList();
                model.restaurant = db.restaurant.Where(s => s.restaurant_intro.Contains(q) || s.restaurant_title.Contains(q) || s.city.Contains(q) || s.district.Contains(q)).ToList();
                model.hotel = db.hotel.Where(s => s.hotel_intro.Contains(q) || s.hotel_title.Contains(q) || s.city.Contains(q) || s.district.Contains(q)).ToList();
                model.spot = db.spot.Where(s => s.spot_intro.Contains(q) || s.spot_title.Contains(q) || s.city.Contains(q) || s.district.Contains(q)).ToList();
            }
            //找地區
            search_city(city, model);
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
            var hotel_null_result = (from a in db.hotel
                                     join b in db.comment on a.hotel_id equals b.tourism_id into x
                                     from b in x.DefaultIfEmpty()
                                     where b.tourism_id == null
                                     select new result_rating { id = a.hotel_id, rating = b.tourism_id }).ToList();
            var restaurant_null_result = (from a in db.restaurant
                                          join b in db.comment on a.restaurant_id equals b.tourism_id into x
                                          from b in x.DefaultIfEmpty()
                                          where b.tourism_id == null
                                          select new result_rating { id = a.restaurant_id, rating = b.tourism_id }).ToList();
            var spot_null_result = (from a in db.spot
                                    join b in db.comment on a.spot_id equals b.tourism_id into x
                                    from b in x.DefaultIfEmpty()
                                    where b.tourism_id == null
                                    select new result_rating { id = a.spot_id, rating = b.tourism_id }).ToList();

            List<result_rating> result = rating_result.Union(activity_null_result.Union(restaurant_null_result.Union(hotel_null_result.Union(spot_null_result)))).OrderByDescending(s => s.rating).ToList();

            model.result_ratings = model.activity.Join(result, a => a.activity_id, b => b.id,
                                                                    (a, b) => new result_rating
                                                                    {
                                                                        id = a.activity_id,
                                                                        title = a.activity_title,
                                                                        intro = a.activity_intro,
                                                                        rating = b.rating
                                                                    }).Concat(model.hotel.Join(result, a => a.hotel_id, b => b.id,
                                                                    (a, b) => new result_rating
                                                                    {
                                                                        id = a.hotel_id,
                                                                        title = a.hotel_title,
                                                                        intro = a.hotel_intro,
                                                                        rating = b.rating
                                                                    }).Concat(model.restaurant.Join(result, a => a.restaurant_id, b => b.id,
                                                                    (a, b) => new result_rating
                                                                    {
                                                                        id = a.restaurant_id,
                                                                        title = a.restaurant_title,
                                                                        intro = a.restaurant_intro,
                                                                        rating = b.rating
                                                                    }).Concat(model.spot.Join(result, a => a.spot_id, b => b.id,
                                                                    (a, b) => new result_rating
                                                                    {
                                                                        id = a.spot_id,
                                                                        title = a.spot_title,
                                                                        intro = a.spot_intro,
                                                                        rating = b.rating
                                                                    })))).ToList();
            //找評分
            if (!String.IsNullOrEmpty(comment_rating))
            {
                ViewBag.rating = comment_rating;
                double rating = int.Parse(comment_rating); //查詢條件轉型
                for (int i = 0; i < model.result_ratings.Count; i++)
                {
                    if (String.IsNullOrEmpty(model.result_ratings[i].rating))
                    {
                        model.result_ratings.RemoveAt(i);
                        i = i - 1;
                    }
                    else
                    {
                        double rating_avg = Double.Parse(model.result_ratings[i].rating);
                        if (rating_avg < rating)
                        {
                            model.result_ratings.RemoveAt(i);
                            i = i - 1;
                        }
                    }
                }
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
                //有評論分數的
                //select tourism_id,avg(comment_rating) as aver
                //from activity as a
                //join comment as c on a.activity_id = c.tourism_id
                //group by tourism_id
                //order by aver desc               
                var activity_rating_result = (from a in db.activity
                                     join b in db.comment on a.activity_id equals b.tourism_id
                                     group b by b.tourism_id into g
                                     orderby g.Average(s => s.comment_rating)
                                     select new result_rating { id = g.Key, rating = g.Average(s => s.comment_rating).ToString() }).ToList();
                var hotel_rating_result = (from a in db.hotel
                                     join b in db.comment on a.hotel_id equals b.tourism_id
                                     group b by b.tourism_id into g
                                     orderby g.Average(s => s.comment_rating)
                                     select new result_rating { id = g.Key, rating = g.Average(s => s.comment_rating).ToString() }).ToList();
                var restaurant_rating_result = (from a in db.restaurant
                                     join b in db.comment on a.restaurant_id equals b.tourism_id
                                     group b by b.tourism_id into g
                                     orderby g.Average(s => s.comment_rating)
                                     select new result_rating { id = g.Key, rating = g.Average(s => s.comment_rating).ToString() }).ToList();
                var spot_rating_result = (from a in db.spot
                                     join b in db.comment on a.spot_id equals b.tourism_id
                                     group b by b.tourism_id into g
                                     orderby g.Average(s => s.comment_rating)
                                     select new result_rating { id = g.Key, rating = g.Average(s => s.comment_rating).ToString() }).ToList();

                //沒有評論分數的
                //select a.activity_id,tourism_id
                //from activity as a left join comment as c
                //on a.activity_id = c.tourism_id
                //where c.tourism_id is null
                var null_result = (from a in db.activity
                                   join b in db.comment on a.activity_id equals b.tourism_id into x
                                   from b in x.DefaultIfEmpty()
                                   where b.tourism_id == null
                                   select new result_rating { id = a.activity_id, rating = b.tourism_id }).ToList();
                var hotel_null_result = (from a in db.hotel
                                   join b in db.comment on a.hotel_id equals b.tourism_id into x
                                   from b in x.DefaultIfEmpty()
                                   where b.tourism_id == null
                                   select new result_rating { id = a.hotel_id, rating = b.tourism_id }).ToList();
                var restaurant_null_result = (from a in db.restaurant
                                              join b in db.comment on a.restaurant_id equals b.tourism_id into x
                                   from b in x.DefaultIfEmpty()
                                   where b.tourism_id == null
                                   select new result_rating { id = a.restaurant_id, rating = b.tourism_id }).ToList();
                var spot_null_result = (from a in db.spot
                                        join b in db.comment on a.spot_id equals b.tourism_id into x
                                   from b in x.DefaultIfEmpty()
                                   where b.tourism_id == null
                                   select new result_rating { id = a.spot_id, rating = b.tourism_id }).ToList();

                //List<result_rating> result = rating_result.Union(null_result).OrderByDescending(s => s.rating).ToList();

                //model.result_ratings = model.activity.Join(result, a => a.activity_id, b => b.id,
                //                                                        (a, b) => new result_rating
                //                                                        {
                //                                                            id = a.activity_id,
                //                                                            title = a.activity_title,
                //                                                            intro = a.activity_intro,
                //                                                            rating = b.rating
                //                                                        }).OrderBy(r => r.rating).ToList();

                return View(model);
                if (Sortby == "htol")
                {
                    model.result_ratings = model.result_ratings.OrderByDescending(s => s.rating).ToList();
                    return View(model);
                }
                else if (Sortby == "ltoh")
                {
                    model.result_ratings = model.result_ratings.OrderBy(s => s.rating).ToList();
                    return View(model);
                }
            }
            return View(model);

        }
        //排序
        //關鍵字搜尋
        //地點
        private SearchViewModel search_city(string city, SearchViewModel model)
        {
            if (!String.IsNullOrEmpty(city))
            {
                ViewBag.city = city;
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
        private SearchViewModel search_partner(string[] travel_partner, SearchViewModel model)
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
                        for (int x = 0; x < travel_partner.Length; x++)
                        {
                            int result = model.comment.Where(s => s.travel_partner == travel_partner[x]).Count();
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














        //[ChildActionOnly]
        //public ActionResult _Hotel(string q = null, int page = 1)
        //{
        //    ViewBag.q = q;
        //    Session["pg"] = page;
        //    if (!String.IsNullOrEmpty(q))
        //    {

        //        var hotel = db.hotel.Where(s => s.hotel_intro.Contains(q) || s.hotel_title.Contains(q) || s.city.Contains(q) || s.district.Contains(q)).ToList();

        //        return View(hotel.OrderBy(m => m.hotel_id).ToPagedList(page, pageSize));
        //    }
        //    else
        //    {
        //        return View();
        //    }
        //}

        //[ChildActionOnly]
        //public ActionResult _Restaurant(string q = null, int page = 1)
        //{
        //    ViewBag.q = q;
        //    Session["pg"] = page;
        //    if (!String.IsNullOrEmpty(q))
        //    {
        //        var restaurant = db.restaurant.Where(s => s.restaurant_intro.Contains(q) || s.restaurant_title.Contains(q) || s.city.Contains(q) || s.district.Contains(q)).ToList();

        //        return View(restaurant.OrderBy(m => m.restaurant_id).ToPagedList(page, pageSize));
        //    }
        //    else
        //    {
        //        return View();
        //    }
        //}

        //[ChildActionOnly]
        //public ActionResult _Spot(string q = null, int page = 1)
        //{
        //    ViewBag.q = q;
        //    Session["pg"] = page;
        //    if (!String.IsNullOrEmpty(q))
        //    {

        //        var spot = db.spot.Where(s => s.spot_intro.Contains(q) || s.spot_title.Contains(q) || s.city.Contains(q) || s.district.Contains(q)).ToList();

        //        return View(spot.OrderBy(m => m.spot_id).ToPagedList(page, pageSize));
        //    }
        //    else
        //    {
        //        return View();
        //    }
        //}













        //網軍來惹
        public ActionResult generate_comment()
        {
            List<spot> spots = db.spot.OrderBy(m => Guid.NewGuid()).ToList();
            //string id = "";
            string[] title = { "巴澤航空12/13首航 起降時間皆紅眼","好想吃杏子豬排","組員叫我當水軍","日本高中生訪總統府 總統現身問喝珍奶沒",
                                        "單親媽背幼兒賣地瓜 努力賺錢爭取女兒","芳子好變態","花蓮女盜採砂石累犯 徒手搬逾600公斤","水氣與氣溫配合 氣象專家：高山初雪機率高"};
            string[] content = { "氣象專家吳德榮說，由於水氣與氣溫的配合，今天下午起到明天3000公尺以上高山，發生「初雪」的機率高。中央氣象局指出，今天持續受到東北季風及南方雲系影響，全台各地都容易下雨，尤其是北部及東北部降雨時間較長，感受上較為濕冷，有局部大雨或豪雨發生機率。國立中央大學大氣科學系兼任副教授吳德榮也在「三立準氣象．老大洩天機」專欄表示，最新歐洲中期預報中心（ECMWF）模式模擬顯示，環境「垂直風切」持續將颱風北冕的水氣往台灣輸送。他表示，今、明（5、6日）兩天厚實雲層籠罩全台，除了迎風面的北台灣及中南部山區有較多的降雨，冬季很少降雨的中南部平地，也有降雨。吳德榮說，明天下午起另一波更強冷空氣開始南下，氣溫逐漸下降，各地將更為「濕冷」；應注意防雨及保暖。另外，吳德榮指出，今天下午起至明天3000公尺以上高山，由於水氣與氣溫的配合，發生「初雪」的機率高。吳德榮也說，最新模式模擬顯示，週六（7日）乾冷空氣持續南下，清晨仍濕冷、白天漸轉乾冷；週六晚、週日清晨氣溫降至最低，預期將達到「大陸冷氣團」的定義，空曠平地因晴朗、輻射冷卻加成，最低氣溫將降至攝氏10、11度，要注意保暖。吳德榮表示，週日白天起至下週二（10日）冷空氣逐漸減弱，天氣晴朗穩定，週日仍偏冷，週日晚、下週一晨氣溫仍很低；下週一、二氣溫明顯回升、日夜溫差大，下週二晚起另一東北季風影響，迎風面天氣轉變。",
                                                "基隆一名單親媽媽離婚後獨自扶養2名兒子，女兒則在寄養家庭，為了照顧1歲半小兒子，最近在中船路擺攤賣地瓜，收入雖不多，但她說，「我要努力賺錢，讓女兒能回到身邊」。育有3名子女的34歲林姓女子，去年4月離婚後，就讀小學六年級的大兒子和1歲半小兒子由她照顧，女兒則在寄養家庭。林女一直想自食其力，無奈小兒子頭蓋骨癒合較慢，恐有發育遲緩問題，龐大的生活壓力及小孩醫療支出，加上求職碰壁，只能尋求外界協助。林女迫於無奈有時必須領取物資生活，曾在排隊時有人對她訕笑，一度讓她灰心喪志，在臉書（Facebook）網友鼓勵下，10月間向人安基金會基隆平安站報名「給魚給竿，拉人一把」烤地瓜謀生方案，她說，「為了孩子，我要努力工作，不僅要證明自己能謀生，更想爭取讓女兒回到身邊」。",
                                                "印尼巴澤航空將開航台灣，根據巴澤航空向民航局申請的首航時間是12月13日，在台灣起飛及降落的時間都是深夜紅眼時段。民航局說，巴澤航空是一般傳統航空公司，並非低成本航空，目前申請12月13日起，每天一班往返桃園國際機場與印尼首都雅加達。根據巴澤航空向民航局提報的資料，使用737-900ER飛機飛航，可載運180人，包括12個商務艙及168個經濟艙座位，下午4時30分從雅加達起飛，晚上11時到桃園國際機場，地面停留約1小時，深夜12時再從桃園機場起飛，清晨4時30分回到雅加達。巴澤航空是印尼獅子航空集團旗下的航空公司，獅子航空集團下的馬印航空及泰獅航都已開航台灣。",
                                                "日本共同社報導，與蔡總統握手的16歲學生加藤大和對媒體說：「總統突然來了，我真的嚇了一跳。第一次到台灣，這成了最好的回憶。」日本時事社報導，私立松山城南高中2年級學生60人、老師5人今天在台北的總統府與蔡總統一起拍紀念照、握手，大約10分鐘時間，展開了一場台日交流。在交流之際，起源於台灣、目前在日本大受歡迎的珍珠奶茶話題也出現。這群高中生到台灣進行為期4天的訪問，除了參觀總統府之外，也與台灣的高中學校進行交流，還體驗製作小籠包。報導說，近年來，由於台灣治安良好，民調也顯示台日關係友好，台灣成為日本高中很喜歡前往進行修學（教育）旅行的地方。報導引用日本文部科學省（相當於教育部）最新調查顯示，2017年度赴台灣修學旅行的日本高中多達332所，排名第一，赴美者有208校，排名第二）。另外，根據日本全國修學旅行研究協會的統計資料，2016年度日本海外修學旅行高中842所，學生總數14萬5944人，其中赴台灣的高中有262所，人數4萬1878人，約占總數1/4。2017年度日本海外修學旅行高中895所，學生總數15萬6413人。其中赴台灣修學旅行高中325所，人數5萬3940人，占整體34%排名首位。總統府提供的資料則顯示，日本學校（含高中、大學）參訪總統府的學校及人數逐年倍增，2017年一共有7團133人，2018年16團578人，2019年至11月為止已達24團1499人。",
                                                "一名有盜採砂石前科的程姓女子，昨天在花蓮溪口北岸徒手搬運石頭被海巡人員查獲，程嫌供稱要填補牆壁，經秤重約搬運620公斤，訊後依竊盜罪移送花蓮地檢署偵辦。東部分署第一二岸巡隊今天表示，司法小隊昨天下午在花蓮溪口北岸發現一輛形跡可疑的黑色箱型車，疑似盜採砂石，並通報第九巡防區指揮部支援。經盤查發現程嫌車上裝有粉扁石，盜採量約620公斤隨即進行筆錄。第一二岸巡隊表示，程嫌為台東人，現居花蓮，民國103年也曾因盜採砂石遭該隊查獲，昨天下午獨自在岸邊徒手撿拾，約1小時就撿拾620公斤的粉扁石，訊時供稱要填補家中牆壁使用。第一二岸巡隊表示，岸際砂石為國有財產，未經許可不得任意撿拾，將持續取締不法，若民眾發現任何不法情事，可透過海巡署「118」服務專線通報。",
                                                "日本愛媛縣松山市城南高校學生赴台灣教育旅行，今天進入總統府參觀，導覽人員正持總統蔡英文人形立牌解說，蔡總統突然現身，問大家「喝珍珠奶茶了沒？」引起學生雀躍驚呼。日本放送協會（NHK）報導，愛媛縣松山市的松山城南高校學生約60人正在台灣進行教育旅行，行程之一就是造訪位於台北市的總統府。今天在總統府正門玄關大廳，接待人員手持蔡總統的人形立牌做解說之際，蔡總統突然出現在大廳。蔡總統一出現就用日語「大家好」打招呼，之後用中文說：「我是總統蔡英文，這是我的辦公室。」後來蔡總統問：「這次有來吃珍珠奶茶嗎？」報導說，這座總統府有百年歷史，在日本統治台灣時代建造，也是日本學生教育旅行的代表性參觀景點之一。總統府的部分區域開放給一般民眾參觀，但蔡總統本人現身訪客參觀區實屬罕見，所以蔡總統一現身，這群高中生不禁發出驚訝聲。見到蔡總統本人的日本高中生有人說：「沒想到總統本人會出現」，有人說：「以為就是一般參觀，就快結束了，沒想到會見到（總統）」。有女高中生覺得蔡總統看起來很親切，另有女高中生很高興的說：「蔡總統的手握起來好溫暖喔，我不能洗手。」"};
            string[] date = { "2010-07-26", "2016-01-07", "2013-03-08", "2015-11-30", "2018-08-23", "2019-12-05" };
            string[] photo = { "01_ash.png", "02_candy_crush.jpg", "03_goodboy.jpg", "G4wXXRk.gif", "12_goodguy.png", "01_sleep.jpg" };
            string[] memberID = { "M000001", "M000002", "M000003", "M000004", "M000005" };
            string[] partner = { "蜜月", "伴侶", "朋友", "商務", "家庭" };

            Random random = new Random();

            for (int i = 0; i < 100; i++)
            {
                comment cmt = new comment();
                DateTime parsedDate = DateTime.Parse(date[random.Next(0, 6)]);

                cmt.tourism_id = spots[i].spot_id;
                cmt.comment_title = title[random.Next(0, 8)];
                cmt.comment_content = content[random.Next(0, 6)];
                cmt.comment_date = parsedDate;
                cmt.comment_photo = photo[random.Next(0, 6)];
                cmt.comment_stay_total = random.Next(1, 5);
                cmt.comment_rating = Convert.ToInt16(random.Next(1, 6));
                cmt.travel_month = random.Next(1, 13).ToString();
                cmt.comment_status = true;
                cmt.member_id = memberID[random.Next(0, 5)];
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