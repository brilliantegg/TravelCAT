using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using TravelCat.Models;
using TravelCat.ViewModels;

namespace TravelCat.Controllers
{
    public class HomeController : Controller
    {
        dbTravelCat db = new dbTravelCat();


        // GET: Home
        public ActionResult Index(string account)
        {
            //抓會員帳號
            if (Request.IsAuthenticated)
            {
                string user = User.Identity.GetUserName();
                member member = db.member.Where(m => m.member_account == user).FirstOrDefault();

                //HttpCookie cookie = new HttpCookie("memID");
                //cookie.Value = member.member_id.ToString();
                //cookie.Expires = DateTime.Now.AddMinutes(120);
                //Response.Cookies.Add(cookie);
                Session["memberID"] = member.member_id;
            }
            WebIndexViewModel model = new WebIndexViewModel()
            {
                activity = db.activity.OrderBy(m => Guid.NewGuid()).ToList(),
                hotel = db.hotel.OrderBy(m => Guid.NewGuid()).ToList(),
                restaurant = db.restaurant.OrderBy(m => Guid.NewGuid()).ToList(),
                spot = db.spot.OrderBy(m => Guid.NewGuid()).ToList(),
                member = db.member.Where(m => m.member_account == account).FirstOrDefault(),
                comment = db.comment.OrderBy(m => m.comment_date).ToList()
            };

            return View(model);
        }

        class UserManager
        {
            public bool IsValid(string username, string password)
            {
                using (var db = new dbTravelCat()) // use your DbConext
                {
                    // for the sake of simplicity I use plain text passwords
                    // in real world hashing and salting techniques must be implemented   
                    return db.member.Any(u => u.member_account == username
                        && u.member_password == password);
                }
            }
        }
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(string username, string password)
        {
            byte[] password1 = System.Text.Encoding.UTF8.GetBytes(password);
            byte[] hash = new System.Security.Cryptography.SHA256Managed().ComputeHash(password1);
            string hashpassword = Convert.ToBase64String(hash);
            password = hashpassword;
            if (new UserManager().IsValid(username, password))
            {
                var mem = db.member.Where(m => m.member_account == username).FirstOrDefault();
                string role = "";
                if (mem.member_profile.emailConfirmed == true)
                {
                    if (mem.member_status == false)
                    {
                        role = "Confirmed";
                    }
                    else
                    {
                        role = "Blocked";
                    }
                }
                else
                {
                    role = "UnConfirmed";
                }

                var ident = new ClaimsIdentity(
                  new[] { 
              // adding following 2 claim just for supporting default antiforgery provider
              new Claim(ClaimTypes.NameIdentifier, username),
              new Claim("http://schemas.microsoft.com/accesscontrolservice/2010/07/claims/identityprovider", "ASP.NET Identity", "http://www.w3.org/2001/XMLSchema#string"),

              new Claim(ClaimTypes.Name,username),
              new Claim("memID",mem.member_id,ClaimValueTypes.String),
               //// optionally you could add roles if any
              new Claim(ClaimTypes.Role, role)
                  },
                  DefaultAuthenticationTypes.ApplicationCookie);

                HttpContext.GetOwinContext().Authentication.SignIn(
                   new AuthenticationProperties { IsPersistent = false }, ident);
                return RedirectToAction("test"); // auth succeed 
            }

            ViewBag.LoginErr = "帳號或密碼錯誤";
            return View();
        }
        public ActionResult LogOut() 
        {
            var ctx = Request.GetOwinContext();
            var authManager = ctx.Authentication;
            //Response.Cookies["memID"].Expires = DateTime.Now.AddDays(-1);
            Session.Clear();

            authManager.SignOut("ApplicationCookie");
            return RedirectToAction("Index", "Home");
        }
        [Authorize]
        public ActionResult test()
        {
            string user = User.Identity.GetUserName();
            var member = db.member.Where(m => m.member_account == user).FirstOrDefault();
            Session["memberID"] = member.member_id.ToString();
            ViewBag.Data = DateTime.Now.ToString();
            return View();
        }

        //忘記密碼
        public ActionResult ForgetPwd()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ForgetPwd(string account, string email)
        {
            var acc = db.member.Where(m => m.member_account == account).FirstOrDefault();

            if (acc != null)
            {

                if (acc.member_account == account && acc.member_profile.email == email)
                {

                    var callbackUrl = Url.Action("ResetPwd", "Home", new { id = acc.member_id }, protocol: Request.Url.Scheme);

                    GmailSender gs = new GmailSender();
                    gs.account = "travelcat.service@gmail.com";
                    gs.password = "lqleyzcbmrmttloe";
                    gs.sender = "旅途貓 <travelcat.service@gmail.com>";
                    gs.receiver = $"{acc.member_profile.email}";
                    gs.subject = "忘記密碼";
                    gs.messageBody = "如果要重置您的密碼<br><br><a href=" + callbackUrl + ">請點此連結</a><br><br>如果您沒有要重置密碼，請忽略此消息。";
                    gs.IsHtml = true;
                    gs.Send();

                    return RedirectToAction("ForgetView", "Home", new { account = acc.member_account });

                }

            }

            ViewBag.PwdErr = "您輸入的帳號或信箱錯誤";
            return View();
        }
        //忘記密碼完後的頁面 
        public ActionResult ForgetView(string account)
        {
            var acc = db.member.Where(m => m.member_account == account).FirstOrDefault();
            string memderacc = acc.member_account;
            string email1 = acc.member_profile.email;
            ViewBag.account = memderacc;
            ViewBag.accemail = email1;

            return View(acc);
        }

        //重置密碼
        public ActionResult ResetPwd(string id)
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
        public ActionResult ResetPwd(string id, string newpassword)
        {
            member member = db.member.Find(id);

            byte[] password = System.Text.Encoding.UTF8.GetBytes(newpassword);
            byte[] hash = new System.Security.Cryptography.SHA256Managed().ComputeHash(password);
            string hashpassword = Convert.ToBase64String(hash);

            member.member_password = hashpassword;

            if (ModelState.IsValid)
            {
                db.Entry(member).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Login", "Home");
            }

            return View();
        }

    }
}