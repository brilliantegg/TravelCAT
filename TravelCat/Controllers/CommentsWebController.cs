using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TravelCat.Models;
using TravelCat.ViewModels;

namespace TravelCat.Controllers
{
    public class CommentsWebController : Controller
    {
        private dbTravelCat db = new dbTravelCat();
        // GET: CommentsWeb

        public PartialViewResult _CommentsForDestination(string tourismId)
        {
            
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
            ViewBag.tourismId = tourismId;
            return PartialView(model);

        }
        [HttpPost]
        public PartialViewResult _CommentsForDestination(string tourismId, string[] comment_rating = null, string[] travel_partner = null, string travel_month = null)
        {
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
            //if (comment_rating != null)
            //{

            //    for (int i = 0;i<model.comment.Count; i++)
            //    {
            //        for (int j = 0; j < comment_rating.Length; j++)

            //        {

            //            if (model.comment[i].comment_rating.ToString() != comment_rating[j])
            //            {
            //                model.comment.RemoveAt(i);
            //                i = i - 1;
            //            }
            //        }
            //    }
            //}
            foreach (var comment in origin.ToList())
            {
                comments.Add(comment);
            }
            if (comment_rating != null)
            {

                foreach (var comment in comments.ToList())
                {
                    foreach (var item in comment_rating)
                    {
                        if (comment.comment_rating.ToString() != item)
                        {
                            comments.Remove(comment);
                        }
                    }
                }
            }
            if (travel_partner != null)
            {

                foreach (var comment in comments.ToList())
                {
                    foreach (var item in travel_partner)
                    {
                        if (comment.travel_partner != item)
                        {
                            comments.Remove(comment);
                        }
                    }
                }
            }
            //if (travel_month != null)
            //{

            //    foreach (var comment in comments)
            //    {
            //        foreach (var item in travel_month)
            //        {
            //            if (comment.travel_month != item)
            //            {
            //                comments.Remove(comment);
            //            }
            //        }
            //    }
            //}
            model.comment = comments;
            ViewBag.tourismId = tourismId;

            return PartialView(model);

        }
        public PartialViewResult _CommentsFromMember(string memId)
        {
            destinationsViewModel model = new destinationsViewModel()
            {

                comment = db.comment.Where(m => m.member_id == memId).ToList(),
                message = db.message.Where(m => m.member_id == memId).ToList(),
                comment_emoji_details = db.comment_emoji_details.ToList(),
                message_emoji_details = db.message_emoji_details.ToList(),
                member_profile = db.member_profile.Where(m => m.member_id == memId).ToList(),
                member = db.member.Where(m => m.member_id == memId).ToList(),
            };
            return PartialView(model);
        }
        public PartialViewResult _CommentsForFollwers(string memId)
        {
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
                comment = comments.OrderByDescending(m => m.comment_date).ToList(),
                comment_emoji_details = db.comment_emoji_details.ToList(),
                member_profile = db.member_profile.Where(m => m.member_id == memId).ToList(),
                member = members,
            };
            IEnumerable<member> followers = from mem in db.member
                                            join fol in db.follow_list on mem.member_id equals fol.member_id
                                            where fol.member_id == memId
                                            select mem;

            IEnumerable<comment> comm = from mem in followers
                                       join com in db.comment on mem.member_id equals com.member_id
                                       select com;
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