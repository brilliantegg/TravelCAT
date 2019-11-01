using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Configuration;

namespace TravelCat.Controllers
{
    public class LoginadminController : Controller
    {

        SqlConnection Conn = new SqlConnection("data source=MCSDD10823; initial catalog = traval_cat_v1; integrated security = True; multipleactiveresultsets=True;application name = EntityFramework & quot");

        // GET: Loginadmin

      
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]      
        public ActionResult Index(string id,string pwd)
        {
            string sql = "select * from admin where admin_account=@admin_account and admin_password=@admin_password";
            SqlCommand cmd = new SqlCommand(sql, Conn);
            cmd.Parameters.AddWithValue("@admin_account", id);
            cmd.Parameters.AddWithValue("@admin_password", pwd);
            SqlDataReader rd;

            Conn.Open();
            rd = cmd.ExecuteReader();

            if (rd.Read())
            {
                Session["id"] = rd["admin_password"].ToString();

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

    }
}