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
    public class api_message_emoji_detailsController : ApiController
    {
        private dbTravelCat db = new dbTravelCat();

        // GET: api/api_message_emoji_details
        public IQueryable<message_emoji_details> Getmessage_emoji_details()
        {
            return db.message_emoji_details;
        }

        // GET: api/api_message_emoji_details/5
        [ResponseType(typeof(message_emoji_details))]
        public IHttpActionResult Getmessage_emoji_details(long id)
        {
            message_emoji_details message_emoji_details = db.message_emoji_details.Find(id);
            if (message_emoji_details == null)
            {
                return NotFound();
            }

            return Ok(message_emoji_details);
        }

        // PUT: api/api_message_emoji_details/5
        [ResponseType(typeof(void))]
        public IHttpActionResult Putmessage_emoji_details(long id, message_emoji_details message_emoji_details)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != message_emoji_details.id)
            {
                return BadRequest();
            }

            db.Entry(message_emoji_details).State = EntityState.Modified;

            try
            
            
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!message_emoji_detailsExists(id))
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
        // POST: api/api_message_emoji_details
        [ResponseType(typeof(message_emoji_details))]
        public IHttpActionResult Postmessage_emoji_details(message_emoji_details message_emoji_details)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.message_emoji_details.Add(message_emoji_details);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = message_emoji_details.id }, message_emoji_details);
        }
        [HttpDelete]
        // DELETE: api/api_message_emoji_details/5
        [ResponseType(typeof(message_emoji_details))]
        public IHttpActionResult Deletemessage_emoji_details(message_emoji_details details)
        {
            var emoji = db.message_emoji_details.Where(m => m.msg_id == details.msg_id && m.member_id == details.member_id && m.emoji_id == details.emoji_id && m.tourism_id == details.tourism_id).FirstOrDefault();

            db.message_emoji_details.Remove(emoji);
            db.SaveChanges();

            return Ok(emoji);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool message_emoji_detailsExists(long id)
        {
            return db.message_emoji_details.Count(e => e.id == id) > 0;
        }
    }
}
