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
using LeankitLibrary.Entities;

namespace IORPromoteTool.Controllers
{
    public class PromoteCardApiController : BaseApiController
    {
        #region Private Variables

        private PromoteController promoteLib = new PromoteController();

        #endregion

        #region API Functions

        // GET api/PromoteCard
        [HttpGet]
        public IEnumerable<Card> GetPromoteCards()
        {
            IEnumerable<Card> cards = this.promoteLib.GetBacklogList().ToList();
            return cards;
        }

        // POST api/PromoteCard
        [HttpPost]
        public HttpResponseMessage PostPromoteCard(Card card)
        {
            if (ModelState.IsValid)
            {
                // Move Card
                this.promoteLib.KanbanMove(card);

                // Update Card
                this.promoteLib.KanbanUpdate(card);

                // Update last promote date
                this.promoteLib.CurrentUser.LastPromote = DateTime.Today;

                // Update Records DB
                PromoteRecord promoRecord = this.promoteLib.PopulatePromoRecord(card, this.promoteLib.GetUserFullName());
                this.promoteLib.db.PromoteRecords.Add(promoRecord);

                // Save DB changes
                this.promoteLib.db.SaveChanges();

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, promoRecord);
                response.Headers.Location = new Uri(Url.Link("DefaultApi", new { id = promoRecord.Id }));
                return response;
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }
        }

        #endregion

        // Dispose
        protected override void Dispose(bool disposing)
        {
            this.promoteLib.db.Dispose();
            base.Dispose(disposing);
        }
    }
}