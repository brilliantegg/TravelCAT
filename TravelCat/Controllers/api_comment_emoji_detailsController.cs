using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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
    public class api_comment_emoji_detailsController : ApiController
    {
        private dbTravelCat db = new dbTravelCat();

        // GET: api/api_comment_emoji_details
        public IQueryable<comment_emoji_details> Getcomment_emoji_details()
        {
            return db.comment_emoji_details;
        }

        // GET: api/api_comment_emoji_details/5
        [ResponseType(typeof(comment_emoji_details))]
        public IHttpActionResult Getcomment_emoji_details(long id)
        {
            comment_emoji_details comment_emoji_details = db.comment_emoji_details.Find(id);
            if (comment_emoji_details == null)
            {
                return NotFound();
            }

            return Ok(comment_emoji_details);
        }

        // PUT: api/api_comment_emoji_details/5
        [ResponseType(typeof(void))]
        public IHttpActionResult Putcomment_emoji_details(long id, comment_emoji_details comment_emoji_details)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != comment_emoji_details.id)
            {
                return BadRequest();
            }

            db.Entry(comment_emoji_details).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!comment_emoji_detailsExists(id))
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
        [HttpPost]
        // POST: api/api_comment_emoji_details
        [ResponseType(typeof(comment_emoji_details))]
        public IHttpActionResult Postcomment_emoji_details(comment_emoji_details comment_emoji_details)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.comment_emoji_details.Add(comment_emoji_details);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = comment_emoji_details.id }, comment_emoji_details);
        }
        [HttpDelete]
        // DELETE: api/api_comment_emoji_details/5
        [ResponseType(typeof(comment_emoji_details))]
        public IHttpActionResult Deletecomment_emoji_details(long id)
        {
            comment_emoji_details comment_emoji_details = db.comment_emoji_details.Find(id);
            if (comment_emoji_details == null)
            {
                return NotFound();
            }

            db.comment_emoji_details.Remove(comment_emoji_details);
            db.SaveChanges();

            return Ok(comment_emoji_details);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool comment_emoji_detailsExists(long id)
        {
            return db.comment_emoji_details.Count(e => e.id == id) > 0;
        }
        
    }
}