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
    public class api_collections_detailController : ApiController
    {
        private dbTravelCat db = new dbTravelCat();

        // GET: api/api_collections_detail
        public IQueryable<collections_detail> Getcollections_detail()
        {
            return db.collections_detail;
        }

        // GET: api/api_collections_detail/5
        [ResponseType(typeof(collections_detail))]
        public IHttpActionResult Getcollections_detail(int id)
        {
            collections_detail collections_detail = db.collections_detail.Find(id);
            if (collections_detail == null)
            {
                return NotFound();
            }

            return Ok(collections_detail);
        }

        // PUT: api/api_collections_detail/5
        [ResponseType(typeof(void))]
        public IHttpActionResult Putcollections_detail(int id, collections_detail collections_detail)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != collections_detail.collection_id)
            {
                return BadRequest();
            }

            db.Entry(collections_detail).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!collections_detailExists(id))
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

        // POST: api/api_collections_detail
        [ResponseType(typeof(collections_detail))]
        public IHttpActionResult Postcollections_detail(collections_detail collections_detail)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.collections_detail.Add(collections_detail);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = collections_detail.collection_id }, collections_detail);
        }

        // DELETE: api/api_collections_detail/5
        [ResponseType(typeof(collections_detail))]
        public IHttpActionResult Deletecollections_detail(int id)
        {
            collections_detail collections_detail = db.collections_detail.Find(id);
            if (collections_detail == null)
            {
                return NotFound();
            }

            db.collections_detail.Remove(collections_detail);
            db.SaveChanges();

            return Ok(collections_detail);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool collections_detailExists(int id)
        {
            return db.collections_detail.Count(e => e.collection_id == id) > 0;
        }
    }
}