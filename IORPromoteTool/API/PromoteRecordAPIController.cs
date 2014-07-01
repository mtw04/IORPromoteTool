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
    public class PromoteRecordApiController : BaseApiController
    {
        // GET api/PromoteRecord
        public IEnumerable<PromoteRecord> GetPromoteRecords()
        {
            return db.PromoteRecords.AsEnumerable();
        }

        // GET api/PromoteRecord/5
        public PromoteRecord GetPromoteRecord(int id)
        {
            PromoteRecord promoterecord = db.PromoteRecords.Find(id);
            if (promoterecord == null)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }

            return promoterecord;
        }

        // PUT api/PromoteRecord/5
        public HttpResponseMessage PutPromoteRecord(int id, PromoteRecord promoterecord)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }

            if (id != promoterecord.Id)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }

            db.Entry(promoterecord).State = EntityState.Modified;

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

        // POST api/PromoteRecord
        public HttpResponseMessage PostPromoteRecord(PromoteRecord promoterecord)
        {
            if (ModelState.IsValid)
            {
                db.PromoteRecords.Add(promoterecord);
                db.SaveChanges();

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, promoterecord);
                response.Headers.Location = new Uri(Url.Link("DefaultApi", new { id = promoterecord.Id }));
                return response;
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }
        }

        // DELETE api/PromoteRecord/5
        public HttpResponseMessage DeletePromoteRecord(int id)
        {
            PromoteRecord promoterecord = db.PromoteRecords.Find(id);
            if (promoterecord == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            db.PromoteRecords.Remove(promoterecord);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, ex);
            }

            return Request.CreateResponse(HttpStatusCode.OK, promoterecord);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}