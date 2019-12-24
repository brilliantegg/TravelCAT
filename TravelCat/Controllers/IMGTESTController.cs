using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TravelCat.Controllers
{
    public class IMGTESTController : Controller
    {
        // GET: IMGTEST
        public void Index()
        {
            int i = 1;
            foreach (string fname in System.IO.Directory.GetFileSystemEntries("D:\\TravelCAT\\TravelCat\\images\\restaurant"))
            {
                Response.Write(i+".     "+fname+"<br/>");
                i++;
            }
        }
    }
}