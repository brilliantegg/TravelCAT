using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TravelCat.Models;
using TravelCat.ViewModels;
using X.PagedList;
using X.PagedList.Mvc;

namespace TravelCat.Controllers
{
    public class CommentsWebController : Controller
    {
        private dbTravelCat db = new dbTravelCat();
        int pageSize = 10;
        

        public PartialViewResult _CommentsForDestination(string tourismId, int page = 1)
        {
            int currentPage = page < 1 ? 1 : page;
            destinationsViewModel model = new destinationsViewModel()
            {                
                comment = db.comment.Where(m => m.tourism_id == tourismId).ToList(),
                message = db.message.Where(m => m.tourism_id == tourismId).ToList(),
                comment_emoji_details = db.comment_emoji_details.Where(m => m.tourism_id == tourismId).ToList(),
                message_emoji_details = db.message_emoji_details.Where(m => m.tourism_id == tourismId).ToList(),
                member_profile = db.member_profile.ToList(),
                member = db.member.ToList(),
            };
            string firstChar = tourismId.Substring(0, 1);            
            switch (firstChar)
            {
                case "A":
                    model.activity = db.activity.Where(m => m.activity_id == tourismId).FirstOrDefault();
                    break;
                case "H":
                    model.hotel = db.hotel.Where(m => m.hotel_id == tourismId).FirstOrDefault();
                    break;
                case "R":
                    model.restaurant = db.restaurant.Where(m => m.restaurant_id == tourismId).FirstOrDefault();
                    break;
                case "S":
                    model.spot = db.spot.Where(m => m.spot_id == tourismId).FirstOrDefault();
                    break;
                default:                    
                    break;
            }
            model.comment_page = model.comment.ToPagedList(currentPage, pageSize);
            ViewBag.tourismId = tourismId;
            return PartialView(model);

        }
        //篩選評論
        [HttpPost]
        public PartialViewResult _CommentsForDestination(string tourismId, string[] comment_rating = null, string[] travel_partner = null, string travel_month = null, int page = 1)
        {
            int currentPage = page < 1 ? 1 : page;
            List<comment> origin = db.comment.Where(m => m.tourism_id == tourismId).ToList();

            destinationsViewModel model = new destinationsViewModel()
            {
                activity = db.activity.Where(m => m.activity_id == tourismId).FirstOrDefault(),
                comment = db.comment.Where(m => m.tourism_id == tourismId && m.comment_status == true).ToList(),
                message = db.message.Where(m => m.tourism_id == tourismId).ToList(),
                comment_emoji_details = db.comment_emoji_details.ToList(),
                message_emoji_details = db.message_emoji_details.ToList(),
                member_profile = db.member_profile.ToList(),
                member = db.member.ToList(),
            };
            List<comment> comments = new List<comment>();
            
            foreach (var comment in origin.ToList())
            {
                comments.Add(comment);
            }
            if (comment_rating != null)
            {
                foreach (var comment in comments.ToList())
                {
                    if (!comment_rating.Contains(comment.comment_rating.ToString()))
                        comments.Remove(comment);
                }
            }
            if (travel_partner != null)
            {

                foreach (var comment in comments.ToList())
                {

                    if (!travel_partner.Contains(comment.travel_partner))
                        comments.Remove(comment);

                }
            }
            if (travel_month != null)
            {
                string[] month = new string[3];
                switch (travel_month[0].ToString())
                {
                    case "3to5":
                        month[0] = "3";
                        month[1] = "4";
                        month[2] = "5";
                        break;
                    case "6to8":
                        month[0] = "6";
                        month[1] = "7";
                        month[2] = "8";
                        break;
                    case "9to11":
                        month[0] = "9";
                        month[1] = "10";
                        month[2] = "11";
                        break;
                    case "12to2":
                        month[0] = "12";
                        month[1] = "1";
                        month[2] = "2";
                        break;
                }
                foreach (var comment in comments)
                {
                    if (!month.Contains(comment.travel_month))
                        comments.Remove(comment);
                }
            }
            model.comment = comments;
            ViewBag.tourismId = tourismId;
            model.comment_page = model.comment.ToPagedList(currentPage, pageSize);
            return PartialView(model);

        }
        //個人評論
        public PartialViewResult _CommentsFromMember(string memId, int page = 1)
        {
            int currentPage = page < 1 ? 1 : page;
            destinationsViewModel model = new destinationsViewModel()
            {            
                comment = db.comment.Where(m => m.member_id == memId).OrderByDescending(m=>m.comment_date).ToList(),
                message = db.message.Where(m => m.member_id == memId).ToList(),
                comment_emoji_details = db.comment_emoji_details.ToList(),
                message_emoji_details = db.message_emoji_details.ToList(),
                member_profile = db.member_profile.Where(m => m.member_id == memId).ToList(),
                member = db.member.Where(m => m.member_id == memId).ToList(),
                hotel_list = db.hotel.ToList(),
                activity_list = db.activity.ToList(),
                spot_list = db.spot.ToList(),
                restaurant_list = db.restaurant.ToList(),

            };
            model.comment_page = model.comment.ToPagedList(currentPage, pageSize);
            return PartialView(model);
        }
        public PartialViewResult _CommentsForFollwers(string memId, int page = 1)
        {
            int currentPage = page < 1 ? 1 : page;
            List<member> member = db.member.ToList();
            List<comment> comment = db.comment.ToList();
            List<follow_list> follw = db.follow_list.Where(m => m.member_id == memId).ToList();
            List<member> members = new List<member>();
            List<comment> comments = new List<comment>();

            foreach (var fol in follw)
            {
                foreach (var mem in member)
                {
                    if (fol.followed_id == mem.member_id)
                    {
                        members.Add(mem);

                    }
                }
            }
            foreach (var mem in members)
            {
                foreach (var com in comment)
                {
                    if (mem.member_id == com.member_id)
                    {
                        comments.Add(com);

                    }
                }
            }
            FollowerViewModels model = new FollowerViewModels()
            {
                hotel_list = db.hotel.ToList(),
                activity_list = db.activity.ToList(),
                spot_list = db.spot.ToList(),
                restaurant_list = db.restaurant.ToList(),
                comment = comments.OrderByDescending(m => m.comment_date).ToList(),
                comment_emoji_details = db.comment_emoji_details.ToList(),
                member_profile = db.member_profile.Where(m => m.member_id == memId).ToList(),
                member = members,
            };
            model.comment_page = model.comment.ToPagedList(currentPage, pageSize);
            return PartialView(model);
        }
        public int getMsgEmojiNum(int msg_id, string tourismId)
        {
            int num = db.message_emoji_details.Where(m => m.msg_id == msg_id && m.tourism_id == tourismId).Count();
            return num;
        }
        public int getCommentEmojiNum(int emojiId, int commentId, string tourismId)
        {
            int num = db.comment_emoji_details.Where(m => m.emoji_id == emojiId && m.comment_id == commentId && m.tourism_id == tourismId).Count();
            return num;
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult createMessage(message message)
        {
            message.msg_time = DateTime.Now;
            if (ModelState.IsValid)
            {
                db.message.Add(message);
                db.SaveChanges();
                return RedirectToRoute(new { controller = "web_activities", action = "Details", id = message.tourism_id });

            }

            return RedirectToRoute(new { controller = "web_activities", action = "Details", id = message.tourism_id });
        }

        public PartialViewResult _CreateMsg(string tourismID)
        {
            comment newComment = new comment();
            newComment.tourism_id = tourismID;
            ViewBag.tourismID = tourismID;
            return PartialView("_CreateComment");
        }
        [HttpPost]
        public PartialViewResult _CreateMsg(string tourismID, message message)
        {
            comment newComment = new comment();
            newComment.tourism_id = tourismID;
            ViewBag.tourismID = tourismID;
            return PartialView("_CreateComment");
        }
        

    }
}