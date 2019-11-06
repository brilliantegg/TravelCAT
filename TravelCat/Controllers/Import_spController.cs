using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Validation;
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
    public class Import_spController : Controller
    {
        dbTravelCat db = new dbTravelCat();
                       
        public class Rootobject
        {
            public string Id { get; set; }
            public string Name { get; set; }
            public string Zone { get; set; }
            public string Toldescribe { get; set; }
            public string Description { get; set; }
            public string Tel { get; set; }
            public string Add { get; set; }
            public string Zipcode { get; set; }
            public string Region { get; set; }
            public string Town { get; set; }
            public string Travellinginfo { get; set; }
            public string Opentime { get; set; }
            public string Picture1 { get; set; }
            public string Picdescribe1 { get; set; }
            public string Picture2 { get; set; }
            public string Picdescribe2 { get; set; }
            public string Picture3 { get; set; }
            public string Picdescribe3 { get; set; }
            public string Map { get; set; }
            public string Gov { get; set; }
            public float Px { get; set; }
            public float Py { get; set; }
            public string Orgclass { get; set; }
            public string Class1 { get; set; }
            public string Class2 { get; set; }
            public string Class3 { get; set; }
            public string Level { get; set; }
            public string Website { get; set; }
            public string Parkinginfo { get; set; }
            public float? Parkinginfo_Px { get; set; }
            public float? Parkinginfo_Py { get; set; }
            public string Ticketinfo { get; set; }
            public string Remarks { get; set; }
            public string Keyword { get; set; }
            public DateTime? Changetime { get; set; }
        }
                     
        // GET: ImportTEST
        public void LoadJson()
        {
            using (StreamReader r = new StreamReader(@"D:\TravelCAT\TravelCat\spot_C_f.json"))
            {
                var json = r.ReadToEnd();
                List<Rootobject> rt = JsonConvert.DeserializeObject<List<Rootobject>>(json);

                for (int i = 5263; i < rt.Count; i++)
                {
                    spot items = new spot();
                    items.spot_id = db.Database.SqlQuery<string>("Select dbo.GetspotId()").FirstOrDefault();
                    items.spot_intro = rt[i].Toldescribe;
                    items.spot_tel = rt[i].Tel;
                    items.spot_title = rt[i].Name;
                    items.latitude = rt[i].Py.ToString();
                    items.longitude = rt[i].Px.ToString();
                    items.addition_note = rt[i].Remarks;
                    items.address_detail = rt[i].Add;
                    items.city = rt[i].Region;
                    items.district = rt[i].Town;
                    items.open_time = rt[i].Opentime;
                    items.ticket_info = rt[i].Ticketinfo;
                    items.update_date = rt[i].Changetime;
                    items.travelling_info = rt[i].Travellinginfo;


                    if (String.IsNullOrEmpty(rt[i].Picture1) != true)
                    {
                        tourism_photo tp = new tourism_photo();
                        tp.tourism_id = items.spot_id;
                        tp.tourism_photo1 = rt[i].Picture1;
                        db.tourism_photo.Add(tp);
                    }
                    if (String.IsNullOrEmpty(rt[i].Picture2) != true)
                    {
                        tourism_photo tp = new tourism_photo();
                        tp.tourism_id = items.spot_id;
                        tp.tourism_photo1 = rt[i].Picture2;
                        db.tourism_photo.Add(tp);
                    }
                    if (String.IsNullOrEmpty(rt[i].Picture3) != true)
                    {
                        tourism_photo tp = new tourism_photo();
                        tp.tourism_id = items.spot_id;
                        tp.tourism_photo1 = rt[i].Picture3;
                        db.tourism_photo.Add(tp);
                    }

                    db.spot.Add(items);

                    db.SaveChanges();



                }

                Console.WriteLine("success");



            }





        }




    }
}