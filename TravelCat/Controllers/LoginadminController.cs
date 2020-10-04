using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Configuration;
using System.Web.Security;
using TravelCat.Models;
using System.Net;
using System.Data.Entity;

namespace TravelCat.Controllers
{
    
    public class LoginadminController : Controller
    {
        SqlConnection Conn = new SqlConnection(@"data source=(LocalDB)\MSSQLLocalDB;attachdbfilename=|DataDirectory|\travel_cat_v1.mdf;integrated security=True;MultipleActiveResultSets=True;App=EntityFramework&quot;");
        dbTravelCat db = new dbTravelCat();

        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(string id, string pwd)
        {
            string sql = "select * from admin where admin_account=@admin_account and admin_password=@admin_password";//連線到資料庫
            SqlCommand cmd = new SqlCommand(sql, Conn);//寫進資料庫

            byte[] password = System.Text.Encoding.UTF8.GetBytes(pwd);
            byte[] hash = new System.Security.Cryptography.SHA256Managed().ComputeHash(password);
            string hashpassword = Convert.ToBase64String(hash);
            pwd = hashpassword;

            cmd.Parameters.AddWithValue("@admin_account", id);
            cmd.Parameters.AddWithValue("@admin_password", pwd);
            SqlDataReader rd;

            Conn.Open();
            rd = cmd.ExecuteReader();

            if (rd.Read())
            {
               
                Session["id"] = rd["admin_account"].ToString();

                Conn.Close();

                return RedirectToAction("Home", "Admin");
            }
            
            Conn.Close();
            ViewBag.LoginErr = "帳號或密碼有誤";
            return View();
        }

        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Index", "Loginadmin");
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
            var acc = db.admin.Where(m => m.admin_account == account).FirstOrDefault();

            if (acc != null)
            {

                if (acc.admin_account == account && acc.admin_email == email)
                {

                    var callbackUrl = Url.Action("ResetPwd", "Loginadmin", new { id = acc.admin_id }, protocol: Request.Url.Scheme);

                    GmailSender gs = new GmailSender();
                    gs.account = "travelcat.service@gmail.com";
                    gs.password = "lqleyzcbmrmttloe";
                    gs.sender = "旅途貓 <travelcat.service@gmail.com>";
                    gs.receiver = $"{acc.admin_email}";
                    gs.subject = "忘記密碼";
                    gs.messageBody = "如果要重置您的密碼<br><br><a href=" + callbackUrl + ">請點此連結</a><br><br>如果您沒有要重置密碼，請忽略此消息。";
                    gs.IsHtml = true;
                    gs.Send();

                    return RedirectToAction("ForgetView", "Loginadmin", new { account = acc.admin_account });

                }

            }

            ViewBag.PwdErr = "您輸入的帳號或信箱錯誤";
            return View();
        }

        //忘記密碼完後的頁面 
        public ActionResult ForgetView(string account)
        {
            var acc = db.admin.Where(m => m.admin_account == account).FirstOrDefault();
            string memderacc = acc.admin_account;
            string email1 = acc.admin_email;
            ViewBag.account = memderacc;
            ViewBag.accemail = email1;

            return View();
        }
        //重置密碼
        public ActionResult ResetPwd(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            admin admin = db.admin.Find(id);

            if (admin == null)
            {
                return HttpNotFound();
            }
            return View(admin);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ResetPwd(int id, string newpassword)
        {
            admin admin = db.admin.Find(id);

            byte[] password = System.Text.Encoding.UTF8.GetBytes(newpassword);
            byte[] hash = new System.Security.Cryptography.SHA256Managed().ComputeHash(password);
            string hashpassword = Convert.ToBase64String(hash);

            admin.admin_password = hashpassword;

            if (ModelState.IsValid)
            {
                db.Entry(admin).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", "Loginadmin");
            }

            return View();
        }

    }
}