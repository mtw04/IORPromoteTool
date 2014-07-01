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
    public class PromoteUserApiController : BaseApiController
    {
        // GET api/PromoteUser
        public IEnumerable<PromoteUser> GetPromoteUsers()
        {
            return db.PromoteUsers.AsEnumerable();
        }

        // GET api/PromoteUser/5
        public PromoteUser GetPromoteUser(int id)
        {
            PromoteUser promoteuser = db.PromoteUsers.Find(id);
            if (promoteuser == null)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }

            return promoteuser;
        }

        // PUT api/PromoteUser/5
        public HttpResponseMessage PutPromoteUser(int id, PromoteUser promoteuser)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }

            if (id != promoteuser.Id)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }

            db.Entry(promoteuser).State = EntityState.Modified;

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

        // POST api/PromoteUser
        public HttpResponseMessage PostPromoteUser(PromoteUser promoteuser)
        {
            if (ModelState.IsValid)
            {
                db.PromoteUsers.Add(promoteuser);
                db.SaveChanges();

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, promoteuser);
                response.Headers.Location = new Uri(Url.Link("DefaultApi", new { id = promoteuser.Id }));
                return response;
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }
        }

        // DELETE api/PromoteUser/5
        public HttpResponseMessage DeletePromoteUser(int id)
        {
            PromoteUser promoteuser = db.PromoteUsers.Find(id);
            if (promoteuser == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            db.PromoteUsers.Remove(promoteuser);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, ex);
            }

            return Request.CreateResponse(HttpStatusCode.OK, promoteuser);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}