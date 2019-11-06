using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using TravelCat.Models;

namespace TravelCat.Controllers
{
    public class activities_1Controller : ApiController
    {
        private dbTravelCat db = new dbTravelCat();

        // GET: api/activities_1 
        public IQueryable<activity> Getactivity()
        {
            return db.activity;
        }

        // GET: api/activities_1/5
        [ResponseType(typeof(activity))]
        public IHttpActionResult Getactivity(string id)
        {
            activity activity = db.activity.Find(id);
            if (activity == null)
            {
                return NotFound();
            }

            return Ok(activity);
        }

        // PUT: api/activities_1/5
        [ResponseType(typeof(void))]
        public IHttpActionResult Putactivity(string id, activity activity)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != activity.activity_id)
            {
                return BadRequest();
            }

            db.Entry(activity).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!activityExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/activities_1
        [ResponseType(typeof(activity))]
        public IHttpActionResult Postactivity(activity activity)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.activity.Add(activity);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (activityExists(activity.activity_id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = activity.activity_id }, activity);
        }

        // DELETE: api/activities_1/5
        [ResponseType(typeof(activity))]
        public IHttpActionResult Deleteactivity(string id)
        {
            activity activity = db.activity.Find(id);
            if (activity == null)
            {
                return NotFound();
            }

            db.activity.Remove(activity);
            db.SaveChanges();

            return Ok(activity);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool activityExists(string id)
        {
            return db.activity.Count(e => e.activity_id == id) > 0;
        }
    }
}