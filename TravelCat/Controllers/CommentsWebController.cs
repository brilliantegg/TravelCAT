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
                activity = db.activity.Where(m => m.activity_id == tourismId).FirstOrDefault(),
                comment = db.comment.Where(m => m.tourism_id == tourismId).ToList(),
                message = db.message.Where(m => m.tourism_id == tourismId).ToList(),
                comment_emoji_details = db.comment_emoji_details.ToList(),
                message_emoji_details = db.message_emoji_details.ToList(),
                member_profile = db.member_profile.ToList(),
                member = db.member.ToList(),
            };
            ViewBag.tourismId = tourismId;

            return PartialView(model);
        }
        public PartialViewResult _CommentsFromMember(string memId)
        {
            message.msg_time = DateTime.Now;
            db.message.Add(message);
            db.SaveChanges();
            destinationsViewModel model = new destinationsViewModel()
            {

                activity = db.activity.Where(m => m.activity_id == tourismId).FirstOrDefault(),
                comment = db.comment.Where(m => m.tourism_id == tourismId).OrderByDescending(m => m.comment_date).ToList(),
                message = db.message.Where(m => m.tourism_id == tourismId).OrderByDescending(m => m.msg_time).ToList(),
                comment_emoji_details = db.comment_emoji_details.ToList(),
                message_emoji_details = db.message_emoji_details.ToList(),
                member_profile = db.member_profile.ToList(),
                member = db.member.ToList(),
            };
            ViewBag.tourismId = tourismId;
            return PartialView("_CommentsForDestination", model);
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
        public PartialViewResult _CreateMsg(string tourismID)
        {
            comment newComment = new comment();
            newComment.tourism_id = tourismID;
            ViewBag.tourismID = tourismID;
            return PartialView("_CreateComment");
        }
        [HttpPost]
        public PartialViewResult _CreateMsg(string tourismID,message message)
        {
            comment newComment = new comment();
            newComment.tourism_id = tourismID;
            ViewBag.tourismID = tourismID;
            return PartialView("_CreateComment");
        }
        //[HttpPost]
        //public PartialViewResult _CommentsForDestination(string tourismID, comment comment)
        //{
        //    db.comment.Add(comment);
        //    db.SaveChanges();

        //    var comments = db.comment.Where(m => m.tourism_id == tourismID);

        //    comment newComment = new comment();
        //    newComment.tourism_id = tourismID;
        //    ViewBag.tourismID = tourismID;
        //    return PartialView("_CreateComment", comments.ToList());
        //}
    }
}