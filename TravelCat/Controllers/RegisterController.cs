using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Windows.Forms;
using TravelCat.Models;
using TravelCat.ViewModels;

namespace TravelCat.Controllers
{

    public class RegisterController : Controller
    {
        dbTravelCat db = new dbTravelCat();

        // GET: Register
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]

        public ActionResult Index(member model, HttpPostedFileBase photo)
        {

            string mem_id = db.Database.SqlQuery<string>("Select dbo.GetmemberId()").FirstOrDefault();
            model.member_id = mem_id;
            model.member_profile.member_id = mem_id;
            model.member_profile.member_score = 0;
            model.member_profile.create_time = DateTime.Now;
            model.member_profile.emailConfirmed = false;

            byte[] password = System.Text.Encoding.UTF8.GetBytes(model.member_password);
            byte[] hash = new System.Security.Cryptography.SHA256Managed().ComputeHash(password);
            string hashpassword = Convert.ToBase64String(hash);
            model.member_password = hashpassword;


            string fileName = "";
            if (photo != null)
            {
                if (photo.ContentLength > 0)
                {
                    string t = photo.FileName;
                    fileName = mem_id + "_" + DateTime.Now.ToString().Replace("/", "").Replace(":", "").Replace(" ", "") + Path.GetExtension(t);
                    photo.SaveAs(Server.MapPath("~/images/member/" + fileName));
                    model.member_profile.profile_photo = fileName;
                }
            }
            
            var callbackUrl = Url.Action("Confirm", "Register", new { account = model.member_account, id = mem_id }, protocol: Request.Url.Scheme);
            if (ModelState.IsValid)
            {
                GmailSender gs = new GmailSender();
                gs.account = "travelcat.service@gmail.com";
                gs.password = "lqleyzcbmrmttloe";
                gs.sender = "旅途貓 <travelcat.service@gmail.com>";
                gs.receiver = $"{model.member_profile.email}";
                gs.subject = "旅途貓驗證";
                gs.messageBody = "恭喜註冊成功<br><a href=" + callbackUrl + ">請點此連結</a>";
                gs.IsHtml = true;
                gs.Send();

                db.member.Add(model);
                db.member_profile.Add(model.member_profile);
                db.SaveChanges();
                return RedirectToAction("Index", "Home");
            }
            return View(model);
        }
        public ActionResult Confirm(string account, string id)
        {
            var check = db.member.Where(m => m.member_id == id && m.member_account == account).FirstOrDefault();
            if (check != null)
            {
                DialogResult ans = MessageBox.Show("註冊已完成", "信箱已確認", MessageBoxButtons.OK, MessageBoxIcon.Question);
                if (ans == DialogResult.OK)
                {
                    check.member_profile.emailConfirmed = true;
                    db.SaveChanges();
                    return RedirectToAction("Index", "Home");
                }
                return View("重新整理");
            }
            else
            {                
                DialogResult ans = MessageBox.Show("請先註冊會員!", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Question);
                return RedirectToAction("Index", "Register");
            }

        }

    }
}