using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
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
            byte[] pwd = System.Text.Encoding.UTF8.GetBytes(password);
            byte[] hash = new System.Security.Cryptography.SHA256Managed().ComputeHash(pwd);
            string hashpassword = Convert.ToBase64String(hash);
            password = hashpassword;

            if (new UserManager().IsValid(username, password))
            {
                var ident = new ClaimsIdentity(
                  new[] { 
              // adding following 2 claim just for supporting default antiforgery provider
              new Claim(ClaimTypes.NameIdentifier, username),
              new Claim("http://schemas.microsoft.com/accesscontrolservice/2010/07/claims/identityprovider", "ASP.NET Identity", "http://www.w3.org/2001/XMLSchema#string"),

              new Claim(ClaimTypes.Name,username),

              //// optionally you could add roles if any
              //new Claim(ClaimTypes.Role, "UnConfirmedUser"),
              //new Claim(ClaimTypes.Role, "BlockedUser"),

                  },
                  DefaultAuthenticationTypes.ApplicationCookie);

                HttpContext.GetOwinContext().Authentication.SignIn(
                   new AuthenticationProperties { IsPersistent = false }, ident);
                return RedirectToAction("test"); // auth succeed 
            }
            // invalid username or password
            ModelState.AddModelError("", "invalid username or password");
            return View();
        }
        public ActionResult LogOut() 
        {
            var ctx = Request.GetOwinContext();
            var authManager = ctx.Authentication;

            authManager.SignOut("ApplicationCookie");
            return RedirectToAction("Index", "Home");
        }
        [Authorize]
        public ActionResult test()
        {
            ViewBag.Data = DateTime.Now.ToString();
            return View();
        }









    }
}