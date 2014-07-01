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
    public class RequestApiController : BaseApiController
    {
        // GET api/Request
        public IEnumerable<IORRequest> GetIORRequests()
        {
            return db.IORRequests.AsEnumerable();
        }

        // GET api/Request/5
        public IORRequest GetIORRequest(int id)
        {
            IORRequest iorrequest = db.IORRequests.Find(id);
            if (iorrequest == null)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }

            return iorrequest;
        }

        // PUT api/Request/5
        public HttpResponseMessage PutIORRequest(int id, IORRequest iorrequest)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }

            if (id != iorrequest.Id)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }

            db.Entry(iorrequest).State = EntityState.Modified;

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

        // POST api/Request
        public HttpResponseMessage PostIORRequest(IORRequest iorrequest)
        {
            if (ModelState.IsValid)
            {
                db.IORRequests.Add(iorrequest);
                db.SaveChanges();

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, iorrequest);
                response.Headers.Location = new Uri(Url.Link("DefaultApi", new { id = iorrequest.Id }));
                return response;
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }
        }

        // DELETE api/Request/5
        public HttpResponseMessage DeleteIORRequest(int id)
        {
            IORRequest iorrequest = db.IORRequests.Find(id);
            if (iorrequest == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            db.IORRequests.Remove(iorrequest);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, ex);
            }

            return Request.CreateResponse(HttpStatusCode.OK, iorrequest);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}