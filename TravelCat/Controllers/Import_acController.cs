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
    public class Import_acController : Controller
    {
        dbTravelCat db = new dbTravelCat();


        public class Rootobject
        {
            public string Id { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }
            public string Participation { get; set; }
            public string Location { get; set; }
            public string Add { get; set; }
            public string Region { get; set; }
            public string Town { get; set; }
            public string Tel { get; set; }
            public string Org { get; set; }
            public DateTime Start { get; set; }
            public DateTime End { get; set; }
            public string Cycle { get; set; }
            public string Noncycle { get; set; }
            public string Website { get; set; }
            public string Picture1 { get; set; }
            public string Picdescribe1 { get; set; }
            public string Picture2 { get; set; }
            public string Picdescribe2 { get; set; }
            public string Picture3 { get; set; }
            public string Picdescribe3 { get; set; }
            public float Px { get; set; }
            public float Py { get; set; }
            public string Class1 { get; set; }
            public string Class2 { get; set; }
            public string Map { get; set; }
            public string Travellinginfo { get; set; }
            public string Parkinginfo { get; set; }
            public string Charge { get; set; }
            public string Remarks { get; set; }
        }


        // GET: ImportTEST
        public void LoadJson()
        {
            using (StreamReader r = new StreamReader(@"D:\TravelCAT\TravelCat\activity_C_f.json"))
            {
                var json = r.ReadToEnd();
                List<Rootobject> rt = JsonConvert.DeserializeObject<List<Rootobject>>(json);

                for (int i = 0; i < rt.Count; i++)
                {
                    activity items = new activity();
                    items.activity_id = db.Database.SqlQuery<string>("Select dbo.GetactivityId()").FirstOrDefault();
                    items.activity_intro = rt[i].Description;
                    items.activity_tel = rt[i].Tel;
                    items.activity_title = rt[i].Name;
                    items.latitude = rt[i].Py.ToString();
                    items.longitude = rt[i].Px.ToString();
                    items.website = rt[i].Website;
                    items.address_detail = rt[i].Add;
                    items.city = rt[i].Region;
                    items.district = rt[i].Town;
                    items.begin_date = rt[i].Start.ToString();
                    items.end_date = rt[i].End.ToString();
                    items.organizer = rt[i].Org;
                    items.transport_info = rt[i].Travellinginfo;

                    if (rt[i].Picture1 != "")
                    {
                        tourism_photo tp = new tourism_photo();
                        tp.id = db.Database.SqlQuery<int>("Select count(*) from dbo.activity").FirstOrDefault();
                        tp.tourism_id = items.activity_id;
                        tp.tourism_photo1 = rt[i].Picture1;
                        db.tourism_photo.Add(tp);
                    }
                    if (rt[i].Picture2 != "")
                    {
                        tourism_photo tp = new tourism_photo();
                        tp.id = db.Database.SqlQuery<int>("Select count(*) from dbo.activity").FirstOrDefault();
                        tp.tourism_id = items.activity_id;
                        tp.tourism_photo1 = rt[i].Picture2;
                        db.tourism_photo.Add(tp);
                    }
                    if (rt[i].Picture3 != "")
                    {
                        tourism_photo tp = new tourism_photo();
                        tp.id=db.Database.SqlQuery<int>("Select count(*) from dbo.activity").FirstOrDefault();
                        tp.tourism_id = items.activity_id;
                        tp.tourism_photo1 = rt[i].Picture3;
                        db.tourism_photo.Add(tp);
                    }

                    db.activity.Add(items);
                    try
                    {
                        db.SaveChanges();
                    }
                    catch (DbEntityValidationException ex)
                    {
                        var entityError = ex.EntityValidationErrors.SelectMany(x => x.ValidationErrors).Select(x => x.ErrorMessage);
                        var getFullMessage = string.Join("; ", entityError);
                        var exceptionMessage = string.Concat(ex.Message, "errors are: ", getFullMessage);
                        //NLog
                        Console.WriteLine(exceptionMessage.ToString());

                    }


                }

                Console.WriteLine("success");



            }





        }




    }
}