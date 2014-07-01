using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

using IORPromoteTool.Models;

namespace IORPromoteTool.Controllers
{
    public class PromoteFrequencyApiController : BaseApiController
    {
        // GET api/PromoteFrequency
        public IEnumerable<PromoteFrequency> GetPromoteFrequencies()
        {
            return db.PromoteFrequencies.AsEnumerable();
        }

        // GET api/PromoteFrequency/5
        public PromoteFrequency GetPromoteFrequency(int id)
        {
            PromoteFrequency promotefrequency = db.PromoteFrequencies.Find(id);
            if (promotefrequency == null)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }

            return promotefrequency;
        }

        // PUT api/PromoteFrequency/5
        public HttpResponseMessage PutPromoteFrequency(int id, PromoteFrequency promotefrequency)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }

            if (id != promotefrequency.Id)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }

            db.Entry(promotefrequency).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, ex);
            }

            return Request.CreateResponse(HttpStatusCode.OK);
        }

        // POST api/PromoteFrequency
        public HttpResponseMessage PostPromoteFrequency(PromoteFrequency promotefrequency)
        {
            if (ModelState.IsValid)
            {
                db.PromoteFrequencies.Add(promotefrequency);
                db.SaveChanges();

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, promotefrequency);
                response.Headers.Location = new Uri(Url.Link("DefaultApi", new { id = promotefrequency.Id }));
                return response;
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }
        }

        // DELETE api/PromoteFrequency/5
        public HttpResponseMessage DeletePromoteFrequency(int id)
        {
            PromoteFrequency promotefrequency = db.PromoteFrequencies.Find(id);
            if (promotefrequency == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            db.PromoteFrequencies.Remove(promotefrequency);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, ex);
            }

            return Request.CreateResponse(HttpStatusCode.OK, promotefrequency);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}