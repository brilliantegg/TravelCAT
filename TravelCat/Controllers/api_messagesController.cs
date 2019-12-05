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
    public class api_messagesController : ApiController
    {
        private dbTravelCat db = new dbTravelCat();

        // GET: api/api_messages
        public IQueryable<message> Getmessage()
        {
            return db.message;
        }

        // GET: api/api_messages/5
        [ResponseType(typeof(message))]
        public IHttpActionResult Getmessage(long id)
        {
            message message = db.message.Find(id);
            if (message == null)
            {
                return NotFound();
            }

            return Ok(message);
        }

        // PUT: api/api_messages/5
        [ResponseType(typeof(void))]
        public IHttpActionResult Putmessage(long id, message message)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != message.msg_id)
            {
                return BadRequest();
            }

            db.Entry(message).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!messageExists(id))
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

        // POST: api/api_messages
        [ResponseType(typeof(message))]
        public IHttpActionResult Postmessage(message message)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.message.Add(message);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = message.msg_id }, message);
        }

        // DELETE: api/api_messages/5
        [ResponseType(typeof(message))]
        public IHttpActionResult Deletemessage(long id)
        {
            message message = db.message.Find(id);
            if (message == null)
            {
                return NotFound();
            }

            db.message.Remove(message);
            db.SaveChanges();

            return Ok(message);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool messageExists(long id)
        {
            return db.message.Count(e => e.msg_id == id) > 0;
        }
    }
}