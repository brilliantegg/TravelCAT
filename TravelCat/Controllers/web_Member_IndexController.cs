using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Windows.Forms;
using TravelCat.Models;
using TravelCat.ViewModels;

namespace TravelCat.Controllers
{
    //[Authorize(Roles = "Confirmed,Unconfirmed")]
    public class web_Member_IndexController : Controller
    {
        dbTravelCat db = new dbTravelCat();

        // GET: web_Member_Index
        public ActionResult Index(string id = "M000003")
        {
            var score = db.Database.SqlQuery<string>("Select dbo.GetactivityId()").ToList();
            MemberIndexViewModels model = new MemberIndexViewModels()
            {
                member = db.member.Find(id),
                member_profile = db.member_profile.Find(id),
                comment = db.comment.OrderByDescending(m => m.comment_id).ToList(),
                follow = db.follow_list.Where(m => m.followed_id == id).ToList(),
                followed = db.follow_list.Where(m => m.member_id == id).ToList(),
                follow_list = db.follow_list.ToList(),
                collections_detail = db.collections_detail.Where(m => m.member_id == id).ToList(),
                activity = db.activity.ToList(),
                hotel = db.hotel.ToList(),
                restaurant = db.restaurant.ToList(),
                spot = db.spot.ToList(),

            };

            var follow_score = (from a in db.follow_list
                           group a by a.member_id into b
                           orderby b.Count() descending
                           select new { id = b.Key, score = b.Count()*5 }).ToList();


            var comment_score = (from a in db.comment
                           group a by a.member_id into b
                           orderby b.Count() descending
                           select new { id = b.Key, score = b.Count()*5 }).ToList();


            var totoal_score = (follow_score).Union(comment_score).GroupBy(m=>m.id).Select(m=>new { id=m.Key,total=m.Sum(x=>x.score) }).OrderByDescending(m=>m.total).Take(3).ToList();

            //var result5 = (from a in result4
            //               group a by a.id into b
            //               orderby b.Count() descending
            //               select new { id = b.Key, count = b.Count() }).ToList();

            //score final_result = new score();
            //for (int i = 0; i < result5.Count; i++)
            //{
            //    score final_result = new score();
            //    string memid = result5[i].id;
            //    int score_result = 0;
            //    for (int j = 0; j < result5[i].count; j++)
            //    {
            //        score_result += result4.Where(s => s.id == id).ToList()[j].count;
            //    }

            //    final_result.memID = memid;
            //    final_result.mem_score = score_result;
            //    model.scores.Add(final_result);
            //}

            ViewBag.memberId = id;
            ViewBag.member_profile = db.member_profile.ToList();
            ViewBag.score = totoal_score;

            //result2.GetType().GetProperty("id").GetValue(result2);

            return View(model);
        }
        public ActionResult EditMemberProfile(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            member member = db.member.Find(id);
            if (member == null)
            {
                return HttpNotFound();
            }
            return View(member);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditMemberProfile(string id, string newnickname, string newnation, string newcity, string newaddress_detail, string newphone, HttpPostedFileBase photo)
        {
            member member = db.member.Find(id);

            member.member_profile.nickname = newnickname;
            member.member_profile.nation = newnation;
            member.member_profile.city = newcity;
            member.member_profile.address_detail = newaddress_detail;
            member.member_profile.phone = newphone;

            string fileName = "";
            if (photo != null)
            {
                if (photo.ContentLength > 0)
                {
                    string t = photo.FileName;
                    fileName = member.member_profile.member_id + "_" + DateTime.Now.ToString().Replace("/", "").Replace(":", "").Replace(" ", "") + Path.GetExtension(t);
                    System.IO.File.Delete(Server.MapPath("~/images/member/" + member.member_profile.profile_photo));
                    photo.SaveAs(Server.MapPath("~/images/member/" + fileName));
                    member.member_profile.profile_photo = fileName;
                }
            }
            if (ModelState.IsValid)
            {
                db.Entry(member.member_profile).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", "web_Member_Index", new { id = member.member_id });
            }

            return View(member);
        }

        //修改密碼
        public ActionResult Editpassword(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            member member = db.member.Find(id);

            if (member == null)
            {
                return HttpNotFound();
            }
            return View(member);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Editpassword(string id,string oldpassword, string newpassword)
        {
            member member = db.member.Find(id);


            byte[] password1 = System.Text.Encoding.UTF8.GetBytes(oldpassword);
            byte[] hash1 = new System.Security.Cryptography.SHA256Managed().ComputeHash(password1);
            string hashpassword1 = Convert.ToBase64String(hash1);

            ViewBag.Err = "原密碼有誤";

            if (hashpassword1 == member.member_password)
            {
                if (ModelState.IsValid)
                {
                    byte[] password = System.Text.Encoding.UTF8.GetBytes(newpassword);
                    byte[] hash = new System.Security.Cryptography.SHA256Managed().ComputeHash(password);
                    string hashpassword = Convert.ToBase64String(hash);

                    member.member_password = hashpassword;

                    db.Entry(member).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index", "web_Member_Index", new { id = member.member_id });
                }
            }
            return View(member);
        }

        //修改信箱
        public ActionResult Editemail(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            member_profile member = db.member_profile.Find(id);

            if (member == null)
            {
                return HttpNotFound();
            }
            return View(member);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Editemail(string id, string newemail)
        {
            member_profile member = db.member_profile.Find(id);

            if (ModelState.IsValid)
            {
                member.email = newemail;
                member.emailConfirmed = false;

                var callbackUrl = Url.Action("Confirm", "web_Member_Index", new { account = member.member_id }, protocol: Request.Url.Scheme);

                GmailSender gs = new GmailSender();
                gs.account = "travelcat.service@gmail.com";
                gs.password = "lqleyzcbmrmttloe";
                gs.sender = "旅途貓 <travelcat.service@gmail.com>";
                gs.receiver = $"{member.email}";
                gs.subject = "更改信箱";
                gs.messageBody = "更改完成，點選連結驗證您的信箱<br><a href=" + callbackUrl + ">請點此連結</a>";
                gs.IsHtml = true;
                gs.Send();

                db.Entry(member).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", "web_Member_Index", new { id = member.member_id });

            }
            return View(member);
        }
      
        public ActionResult Confirm(string account)
        {
            var check = db.member_profile.Where(m => m.member_id == account).FirstOrDefault();

            if (check != null)
            {
                DialogResult ans = MessageBox.Show("更改信箱完成", "信箱已確認", MessageBoxButtons.OK, MessageBoxIcon.Question);
                if (ans == DialogResult.OK)
                {
                    check.emailConfirmed = true;
                    db.SaveChanges();
                    return RedirectToAction("Index", "Home");
                }
                return View("重新整理");
            }
            else
            {
                DialogResult ans = MessageBox.Show("驗證有誤!", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Question);
                return View();
            }
        }
        [HttpPost]
        public string getfollowed(string member_id, string followed_id)
        {
            string response = "追蹤成功";
            follow_list follower = new follow_list();
            follower.follow_date = DateTime.Now;
            follower.member_id = member_id;
            follower.followed_id = followed_id;
            db.follow_list.Add(follower);
            db.SaveChanges();
            return response;
        }
        [HttpPost]
        public string Unfollowed(string member_id, string followed_id)
        {
            string response = "取消追蹤";
            follow_list follower = db.follow_list.Where(m=>m.member_id== member_id&& m.followed_id== followed_id).FirstOrDefault();
            db.follow_list.Remove(follower);
            db.SaveChanges();
            return response;
        }
        public ActionResult Collections(string id= "M000003")
        {
            ViewBag.id = id;
            CollectionViewModels model = new CollectionViewModels()
            {
                member = db.member.Find(id),
                member_profile = db.member_profile.Find(id),
                collections_detail = db.collections_detail.Where(m => m.member_id == id).ToList(),
                activity = db.activity.ToList(),
                hotel = db.hotel.ToList(),
                restaurant = db.restaurant.ToList(),
                spot = db.spot.ToList(),

            };
            
            var data = new[] { new { title = "", longitude = "", latitude = "" } }.ToList();
            data.RemoveAt(0);
            var member = db.member.Find(id);
            

            foreach (var c in member.collections_detail)
            {
                string firstChar = c.tourism_id.Substring(0, 1);
                switch (firstChar)
                {
                    case "A":
                        var activity = db.activity.Where(a => a.activity_id == c.tourism_id).FirstOrDefault();
                        data.Add(new
                        {
                            title = activity.activity_title.Replace(" ", ""),
                            longitude = activity.longitude,
                            latitude = activity.latitude
                        });
                        break;
                    case "H":
                        var hotel = db.hotel.Where(a => a.hotel_id == c.tourism_id).FirstOrDefault();
                        data.Add(new
                        {
                            title = hotel.hotel_title.Replace(" ", ""),
                            longitude = hotel.longitude,
                            latitude = hotel.latitude
                        });
                        break;
                    case "R":
                        var restaurant = db.restaurant.Where(a => a.restaurant_id == c.tourism_id).FirstOrDefault();
                        data.Add(new
                        {
                            title = restaurant.restaurant_title.Replace(" ", ""),
                            longitude = restaurant.longitude,
                            latitude = restaurant.latitude
                        });
                        break;
                    case "S":
                        var spot = db.spot.Where(a => a.spot_id == c.tourism_id).FirstOrDefault();
                        data.Add(new
                        {
                            title = spot.spot_title.Replace(" ", ""),
                            longitude = spot.longitude,
                            latitude = spot.latitude
                        });
                        break;
                    default:
                        break;
                }                          
                
            }
            
            string json = JsonConvert.SerializeObject(data);
            ViewBag.data = json;

            var collect = model.collections_detail.Where(m => m.collection_type_id.ToString() == "1").ToList();
            ViewBag.collect = collect;
            var wannaGo = model.collections_detail.Where(m => m.collection_type_id.ToString() == "2").ToList();
            ViewBag.wannaGo = wannaGo;
            var hadBeen = model.collections_detail.Where(m => m.collection_type_id.ToString() == "3").ToList();
            ViewBag.hadBeen = hadBeen;
            var mustGo = model.collections_detail.Where(m => m.collection_type_id.ToString() == "4").ToList();
            ViewBag.mustGo = mustGo;
            //model.a_c = (from a in db.activity
            //                        join c in model.collections_detail on a.activity_id equals c.tourism_id into x
            //                        from b in x
            //                        select new a_c { id = a.activity_id }).ToList();
            //ViewBag.collect_activity = collect_activity;


            return View(model);

        }
    }
}