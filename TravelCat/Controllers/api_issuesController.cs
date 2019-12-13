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
    public class api_issuesController : ApiController
    {
        private dbTravelCat db = new dbTravelCat();

        //// GET: api/api_issues
        //public IQueryable<issue> Getissue()
        //{
        //    return db.issue;
        //}

        //// GET: api/api_issues/5
        //[ResponseType(typeof(issue))]
        //public IHttpActionResult Getissue(int id)
        //{
        //    issue issue = db.issue.Find(id);
        //    if (issue == null)
        //    {
        //        return NotFound();
        //    }

        //    return Ok(issue);
        //}

        //// PUT: api/api_issues/5
        //[ResponseType(typeof(void))]
        //public IHttpActionResult Putissue(int id, issue issue)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    if (id != issue.id)
        //    {
        //        return BadRequest();
        //    }

        //    db.Entry(issue).State = EntityState.Modified;

        //    try
        //    {
        //        db.SaveChanges();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!issueExists(id))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return StatusCode(HttpStatusCode.NoContent);
        //}
        [HttpPost]
        // POST: api/api_issues
        [ResponseType(typeof(issue))]
        public IHttpActionResult Post(string memberId,string issueContent,int issue_id, int problem_id = 1)
        {
            issue issue = new issue();
            issue.member_id = memberId;
            issue.issue_content = issueContent;
            issue.admin_id = 1;
            issue.issue_id = issue_id;
            issue.report_date = DateTime.Now;
            issue.problem_id = problem_id.ToString();
            db.issue.Add(issue);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = issue.id }, issue);
        }

        // DELETE: api/api_issues/5
        [ResponseType(typeof(issue))]
        public IHttpActionResult Deleteissue(int id)
        {
            issue issue = db.issue.Find(id);
            if (issue == null)
            {
                return NotFound();
            }

            db.issue.Remove(issue);
            db.SaveChanges();

            return Ok(issue);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool issueExists(int id)
        {
            return db.issue.Count(e => e.id == id) > 0;
        }
    }
}