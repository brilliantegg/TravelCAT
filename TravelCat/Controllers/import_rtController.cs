using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Xml;
using TravelCat.Models;

namespace TravelCat.Controllers
{
    public class import_rtController : Controller
    {
        dbTravelCat db = new dbTravelCat();


        public class Rootobject
        {
            public string Id { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }
            public string Add { get; set; }
            public string Zipcode { get; set; }
            public string Region { get; set; }
            public string Town { get; set; }
            public string Tel { get; set; }
            public string Opentime { get; set; }
            public string Website { get; set; }
            public string Picture1 { get; set; }
            public string Picdescribe1 { get; set; }
            public string Picture2 { get; set; }
            public string Picdescribe2 { get; set; }
            public string Picture3 { get; set; }
            public string Picdescribe3 { get; set; }
            public float Px { get; set; }
            public float Py { get; set; }
            public string Class { get; set; }
            public string Map { get; set; }
            public string Parkinginfo { get; set; }
        }



        public void LoadJson()
        {
            using (StreamReader r = new StreamReader(@"D:\TravelCAT\TravelCat\restaurant_C_f.json"))
            {
                var json = r.ReadToEnd();
                List<Rootobject> rt = JsonConvert.DeserializeObject<List<Rootobject>>(json);

                for (int i =0; i < rt.Count; i++)
                {
                    restaurant items = new restaurant();
                    items.restaurant_id = db.Database.SqlQuery<string>("Select dbo.GetrestId()").FirstOrDefault();
                    items.restaurant_intro = rt[i].Description;
                    items.restaurant_tel = rt[i].Tel;
                    items.restaurant_title = rt[i].Name;
                    items.latitude = rt[i].Py.ToString();
                    items.longitude = rt[i].Px.ToString();
                    items.open_time = rt[i].Opentime;
                    items.address_detail = rt[i].Add;
                    items.city = rt[i].Region;
                    items.district = rt[i].Town;

                    //if (String.IsNullOrEmpty(rt[i].Picture1) != true)
                    //{
                    //    tp.tourism_id = items.restaurant_id;
                    //    tp.tourism_photo1 = rt[i].Picture1;
                    //    db.tourism_photo.Add(tp);
                    //}
                    //if (String.IsNullOrEmpty(rt[i].Picture2) != true)
                    //{
                    //    tp.tourism_id = items.restaurant_id;
                    //    tp.tourism_photo1 = rt[i].Picture2;
                    //    db.tourism_photo.Add(tp);
                    //}
                    //if (String.IsNullOrEmpty(rt[i].Picture3) != true)
                    //{
                    //    tp.tourism_id = items.restaurant_id;
                    //    tp.tourism_photo1 = rt[i].Picture3;
                    //    db.tourism_photo.Add(tp);
                    //}
                    if (String.IsNullOrEmpty(rt[i].Picture1) != true)
                    {
                        tourism_photo tp = new tourism_photo();
                        tp.tourism_id = items.restaurant_id;
                        tp.tourism_photo1 = rt[i].Picture1;
                        db.tourism_photo.Add(tp);
                    }
                    if (String.IsNullOrEmpty(rt[i].Picture2) != true)
                    {
                        tourism_photo tp = new tourism_photo();
                        tp.tourism_id = items.restaurant_id;
                        tp.tourism_photo1 = rt[i].Picture2;
                        db.tourism_photo.Add(tp);
                    }
                    if (String.IsNullOrEmpty(rt[i].Picture3) != true)
                    {
                        tourism_photo tp = new tourism_photo();
                        tp.tourism_id = items.restaurant_id;
                        tp.tourism_photo1 = rt[i].Picture3;
                        db.tourism_photo.Add(tp);
                    }

                    db.restaurant.Add(items);
                    db.SaveChanges();

                }

                Console.WriteLine("success");



            }





        }




    }
}