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
    [Authorize(Roles = "Confirmed,Unconfirmed")]
    public class web_Member_IndexController : Controller
    {
        dbTravelCat db = new dbTravelCat();

        // GET: web_Member_Index
        public ActionResult Index(string id)
        {
            MemberIndexViewModels model = new MemberIndexViewModels()
            {
                member = db.member.Find(id),
                member_profile = db.member_profile.Find(id),

            };
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
        public ActionResult EditMemberProfile(string id, string newacc, string newname, bool newgender,DateTime newbirthday, string newnickname, string newnation, string newcity, string newaddress_detail, string newphone, HttpPostedFileBase photo)
        {
            member member = db.member.Find(id);

            if (ModelState.IsValid)
            {
                member.member_account = newacc;
                member.member_profile.member_name = newname;
                member.member_profile.gender = newgender;
                member.member_profile.birthday = newbirthday;
                member.member_profile.nickname = newnickname;
                member.member_profile.nation = newnation;
                member.member_profile.address_detail = newaddress_detail;
                member.member_profile.phone = newphone;

                string fileName = "";
                if (photo != null)
                {
                    if (photo.ContentLength > 0)
                    {
                        string t = photo.FileName;
                        fileName = member + "_" + DateTime.Now.ToString().Replace("/", "").Replace(":", "").Replace(" ", "") + Path.GetExtension(t);
                        photo.SaveAs(Server.MapPath("~/images/member/" + fileName));
                        member.member_profile.profile_photo = fileName;
                    }
                }

                db.Entry(member).State = EntityState.Modified;
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
        public ActionResult Editpassword(string id, string oldpassword, string newpassword)
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
                gs.subject = "旅途貓驗證";
                gs.messageBody = "恭喜註冊成功<br><a href=" + callbackUrl + ">請點此連結</a>";
                gs.IsHtml = true;
                gs.Send();

                db.Entry(member).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("ConfirmView", "web_Member_Index", new { id = member.member_id });

            }
            return View(member);
        }
        //更改完後的頁面
        public ActionResult ConfirmView(string id)
        {
            member meb = db.member.Find(id);
            string memderacc = meb.member_account;
            ViewBag.account = memderacc;

            return View(meb);
        }
        //驗證信箱
        public ActionResult Confirm(string account)
        {
            var check = db.member_profile.Where(m => m.member_id == account).FirstOrDefault();

            if (check != null)
            {
                DialogResult ans = MessageBox.Show("註冊已完成", "信箱已確認", MessageBoxButtons.OK, MessageBoxIcon.Question);
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
    }
}