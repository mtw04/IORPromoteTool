using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;

using IORPromoteTool.Models;
using LeankitLibrary;
using LeankitLibrary.Entities;
using LeankitLibrary.EntitiesCustom;
using LeankitLibrary.Helper;

namespace IORPromoteTool.Controllers
{
    public class PromoteCardController : BaseController
    {
        #region Redirect

        // Action Filter
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext.ActionDescriptor.ActionName != "Index" && filterContext.ActionDescriptor.ActionName != "History")
            {
                base.OnActionExecuting(filterContext); // Stop filtering if redirect is back at Index
            }
            else if (!this.ValidUserRights())
            {
                filterContext.Result = RedirectToAction("DenyUser", "PromoteCard");
            }
            else if (!this.ValidPromoteDate())
            {
                //filterContext.Result = new RedirectToRouteResult(new System.Web.Routing.RouteValueDictionary { { "controller", "PromoteCard" }, { "action", "Index" } });
                filterContext.Result = RedirectToAction("DenyFrequency", "PromoteCard");
            }
        }

        #endregion

        #region Private Variables

        protected InputData inputSettings = null;
        protected InputData InputSettings
        {
            get
            {
                if (this.inputSettings == null)
                {
                    this.GetBoardSettings();
                }
                return this.inputSettings;
            }
        }

        private KanbanOperation kanbanAPI = null;
        private KanbanOperation KanbanAPI
        {
            get
            {
                this.kanbanAPI = new KanbanOperation(this.InputSettings);
                return this.kanbanAPI;
            }
        }

        private const int pageSize = 50;
        private string baseUrl = @"http://laserfiche.leankit.com/Boards/View/";

        #endregion

        // Constructor
        public PromoteCardController()
        {
        }
        
        #region Actions

        //
        // GET: /PromoteCard
        public ActionResult Index(int page = 1, int sortBy = 1, bool isAsc = false, string search = null)
        {
            #region Search

            IEnumerable<Card> cards = this.GetBacklogList().Where(
                m => search == null 
                    || m.Id.ToString().Contains(search) 
                    || m.Title.ToLower().Contains(search.ToLower()) 
                    || m.PriorityText.ToLower().Contains(search.ToLower()) 
                    || (m.Submitter != null && m.Submitter.ToLower().Contains(search.ToLower()))
                    || m.Tags.ToLower().Contains(search.ToLower())
                    || m.TypeName.ToLower().Contains(search.ToLower())
                    );

            #endregion

            #region Sort

            switch (sortBy)
            {
                case 1:
                    cards = isAsc ? cards.OrderBy(m => m.Id) : cards.OrderByDescending(m => m.Id);
                    break;
                case 2:
                    cards = isAsc ? cards.OrderBy(m => m.Submitter) : cards.OrderByDescending(m => m.Submitter);
                    break;
                case 3:
                    cards = isAsc ? cards.OrderBy(m => m.Title) : cards.OrderByDescending(m => m.Title);
                    break;
                case 4:
                    cards = isAsc ? cards.OrderBy(m => m.Priority) : cards.OrderByDescending(m => m.Priority);
                    break;
                //case 5:
                //    cards = isAsc ? cards.OrderBy(m => m.Tags) : cards.OrderByDescending(m => m.Tags);
                //    break;
                case 6:
                    cards = isAsc ? cards.OrderBy(m => m.TypeName) : cards.OrderByDescending(m => m.TypeName);
                    break;
                case 7:
                    cards = isAsc ? cards.OrderBy(m => m.DueDate) : cards.OrderByDescending(m => m.DueDate);
                    break;
            }

            #endregion

            #region Pagination

            int totalPages = (int)Math.Ceiling((double)cards.Count() / pageSize);
            
            cards = cards.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            #endregion

            #region ViewBag

            ViewBag.CurrentPage = page;
            ViewBag.PageSize = pageSize;
            ViewBag.TotalPages = totalPages; 

            ViewBag.Search = search;
            ViewBag.SortBy = sortBy;
            ViewBag.IsAsc = isAsc;

            ViewBag.BaseUrl = this.baseUrl + this.InputSettings.BoardId + "/";

            #endregion

            return View(cards);
        }

        // Obsolete
        // GET: /PromoteCard/Details
        public ActionResult Details(int id = 0)
        {
            List<Card> cards = this.GetBacklogList();
            Card card = cards.FirstOrDefault(c => c.Id == id);

            if (card == null)
            {
                return HttpNotFound();
            }
            return View(card);
        }

        //
        // GET: /PromoteCard/Promote
        public ActionResult Promote(int id = 0)
        {
            ViewBag.BaseUrl = this.baseUrl + this.InputSettings.BoardId + "/";

            Card card = this.GetBacklogList().Single(m => m.Id == id);

            if (card == null)
            {
                return HttpNotFound();
            }
            return View(card);
        }

        //
        // POST: 
        // Added ValidateInput(false) to avoid IIS dangerous Form value issue
        [HttpPost, ActionName("Promote"), ValidateInput(false)] 
        public ActionResult PromoteConfirmed(Card card)
        {
            // Save card to Promotion Record database
            if (ModelState.IsValid)
            {
                // Move Card
                this.KanbanMove(card);

                // Update Card
                this.KanbanUpdate(card);

                // Update last promote date
                this.CurrentUser.LastPromote = DateTime.Today;

                // Update Records DB
                PromoteRecord promoRecord = PopulatePromoRecord(card, this.GetUserFullName());
                db.PromoteRecords.Add(promoRecord);

                // Save DB changes
                this.db.SaveChanges();

                ViewBag.BaseUrl = this.baseUrl + this.InputSettings.BoardId + "/";

                return RedirectToAction("Approve", card);
            }
            return View(card);
        }

        //
        // GET: /PromoteCard/Approve
        public ActionResult Approve(Card card)
        {
            PromoteCardWrapper proCardWrapper = new PromoteCardWrapper(this.CurrentUser, card);
            return View(proCardWrapper);
        }

        //
        // GET: /PromoteCard/DenyUser
        public ActionResult DenyUser()
        {
            return View();
        }

        //
        // GET: /PromoteCard/DenyFrequency
        public ActionResult DenyFrequency()
        {
            int daysEligible = this.CurrentUser.Frequency - (DateTime.Today - this.CurrentUser.LastPromote.Value).Days;
            PromoteCardWrapper proCardWrapper = new PromoteCardWrapper(this.CurrentUser, daysEligible);

            return View(proCardWrapper);
        }

        //
        // GET: /PromoteCard/History
        public ActionResult History(int page = 1, int sortBy = 1, bool isAsc = false)
        {
            IEnumerable<PromoteRecord> promoteCardList = db.PromoteRecords; //.Where(m => m.Promoter == this.CurrentUser.FullName).ToList();
            List<Lane> laneList = this.KanbanAPI.GetLaneList();
            List<PromoteHistoryWrapper> cardsList = new List<PromoteHistoryWrapper>();
            string laneName = null;
            List<Card> boardList = this.KanbanAPI.GetBoardList();

            foreach (PromoteRecord record in promoteCardList)
            {
                // Make one GetBoard() API call to get all cards. Search through db for correct card and retrieve it's Lane ID
                laneName = this.GetLaneName(this.GetCard(record.CardId, boardList), laneList);

                // Make GetCard() API call to retrieve each Lane ID
                //laneName = this.GetLaneName(this.GetCard(record.CardId), laneList);

                // Append Lane data to wrapper
                cardsList.Add(CreateWrapper(laneName, record));
            }

            IEnumerable<PromoteHistoryWrapper> cards = cardsList; // Convert List to IEnumerable to support LINQ queries for Sort

            #region Sort

            switch (sortBy)
            {
                case 1:
                    cards = isAsc ? cards.OrderBy(m => m.CardId) : cards.OrderByDescending(m => m.CardId);
                    break;
                case 2:
                    cards = isAsc ? cards.OrderBy(m => m.PromoteDate) : cards.OrderByDescending(m => m.PromoteDate);
                    break;
                case 3:
                    cards = isAsc ? cards.OrderBy(m => m.Promoter) : cards.OrderByDescending(m => m.Promoter);
                    break;
                case 4:
                    cards = isAsc ? cards.OrderBy(m => m.Submitter) : cards.OrderByDescending(m => m.Submitter);
                    break;
                case 5:
                    cards = isAsc ? cards.OrderBy(m => m.Title) : cards.OrderByDescending(m => m.Title);
                    break;
                case 6:
                    cards = isAsc ? cards.OrderBy(m => m.Lane) : cards.OrderByDescending(m => m.Lane);
                    break;
            }

            #endregion

            #region Pagination

            int totalPages = 0;
            totalPages = this.GetTotalPages(cards, totalPages);
            cards = cards.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            #endregion

            #region ViewBag

            ViewBag.CurrentPage = page;
            ViewBag.PageSize = pageSize;
            ViewBag.TotalPages = totalPages;

            ViewBag.SortBy = sortBy;
            ViewBag.IsAsc = isAsc;

            ViewBag.BaseUrl = this.baseUrl + this.InputSettings.BoardId + "/";

            #endregion

            return View(cards);
        }

        #endregion

        #region Internal Functions

        private int GetTotalPages(IEnumerable<PromoteHistoryWrapper> cards, int totalPages)
        {
            if ((double)cards.Count() != 0)
                totalPages = (int)Math.Ceiling((double)cards.Count() / pageSize);
            else
                totalPages = 1;
            return totalPages;
        }

        private PromoteRecord PopulatePromoRecord(Card card, string userFullName)
        {
            PromoteRecord promoRecord = new PromoteRecord();

            promoRecord.Promoter = userFullName;
            promoRecord.PromoteDate = DateTime.Now.Date;
            promoRecord.Title = card.Title;
            promoRecord.Description = card.Description;
            promoRecord.Type = card.TypeName;
            promoRecord.Priority = card.PriorityText;
            promoRecord.Tags = card.Tags;
            promoRecord.Deadline = card.DueDate;
            promoRecord.CardId = card.Id;
            promoRecord.Submitter = card.Submitter;
            return promoRecord;
        }

        private void KanbanMove(Card card)
        {
            this.InputSettings.KanbanCard = card;
            this.InputSettings.Operation = "MoveCard";
            DictionaryHelper.AddToDict(this.InputSettings.InputParam, "Operation", this.InputSettings.Operation);
            DictionaryHelper.AddToDict(this.InputSettings.InputParam, "BoardLane", this.InputSettings.PromoteLane);
            DictionaryHelper.AddToDict(this.InputSettings.InputParam, "CardId", Convert.ToString(this.InputSettings.KanbanCard.Id));
            DictionaryHelper.AddToDict(this.InputSettings.InputParam, "BoardId", this.InputSettings.BoardId);

            this.KanbanAPI.AutoRun();
        }

        private void KanbanUpdate(Card card)
        {
            this.InputSettings.KanbanCard = card;
            this.InputSettings.Operation = "UpdateCard";
            DictionaryHelper.AddToDict(this.InputSettings.InputParam, "Operation", this.InputSettings.Operation);
            DictionaryHelper.AddToDict(this.InputSettings.InputParam, "CardType", this.InputSettings.KanbanCard.TypeName);
            DictionaryHelper.AddToDict(this.InputSettings.InputParam, "Priority", this.InputSettings.KanbanCard.PriorityText);
            DictionaryHelper.AddToDict(this.InputSettings.InputParam, "BoardLane", this.InputSettings.PromoteLane);
            DictionaryHelper.AddToDict(this.InputSettings.InputParam, "CardId", Convert.ToString(this.InputSettings.KanbanCard.Id));
            DictionaryHelper.AddToDict(this.InputSettings.InputParam, "BoardId", this.InputSettings.BoardId);

            // Add local JSON parameters
            DictionaryHelper.AddToDict(this.InputSettings.JsonParam, "Title", "[Promoted] " + this.InputSettings.KanbanCard.Title);
            DictionaryHelper.AddToDict(this.InputSettings.JsonParam, "Description", this.InputSettings.KanbanCard.Description + @" [Promoter: """ + this.GetUserFullName() + @"""]");
            DictionaryHelper.AddToDict(this.InputSettings.JsonParam, "DueDate", this.InputSettings.KanbanCard.DueDate);
            DictionaryHelper.AddToDict(this.InputSettings.JsonParam, "Tags", this.InputSettings.KanbanCard.Tags + ",Promoted");

            this.KanbanAPI.AutoRun();
        }

        private List<Card> GetBacklogList()
        {
            List<Card> backlogList = this.KanbanAPI.GetBacklog();

            backlogList = this.ProcessList(backlogList); 
            return backlogList;
        }

        private void GetBoardSettings()
        {
            this.inputSettings = new InputData();

            this.inputSettings.BoardId = this.IORSettings.BoardId;
            this.inputSettings.PromoteLane = this.IORSettings.PromoteLane;
            this.inputSettings.TempPath = this.IORSettings.TempPath;
            this.inputSettings.LogInfo = this.IORSettings.LogInfo;
            this.inputSettings.LogDetails = this.IORSettings.LogDetails;

            this.inputSettings.InputParam = new Dictionary<string, string>();
            this.inputSettings.JsonParam = new Dictionary<string, string>();
            this.inputSettings.KanbanCard = null;
            this.inputSettings.Operation = null;
        }

        private List<Card> ProcessList(List<Card> backlogList)
        {
            foreach (Card card in backlogList)
            {
                // Sanitize description
                if (card.Description.IndexOf(@"<p>") == 0)
                {
                    card.Description = card.Description.Substring(3, card.Description.Length - 7);
                }

                // Find submitter name if it exists
                int start = card.Description.IndexOf(@"[Submitter:") + 13;
                int end = card.Description.IndexOf(@"[Approver:") - 3;

                if (end > 0)
                {
                    int length = end - start;
                    card.Submitter = card.Description.Substring(start, length);
                }
            }
            return backlogList;
        }

        private string GetLaneName(Card card, List<Lane> laneList)
        {
            // Skip searching of Archive to save time. Assume all cards NOT in backlog or board are in archive.
            if (card == null)
                return "Archive";
            else
                return laneList.Where(m => m.Id == card.LaneId).FirstOrDefault().Name;
        }

        private Card GetCard(int cardId)
        {
            return this.KanbanAPI.GetCard(cardId);
        }

        private Card GetCard(int cardId, List<Card> boardList)
        {
            return boardList.Where(m => m.Id == cardId).FirstOrDefault();
        }

        private PromoteHistoryWrapper CreateWrapper(string lane, PromoteRecord record)
        {
            PromoteHistoryWrapper historyWrapper = new PromoteHistoryWrapper();

            historyWrapper.CardId = record.CardId;
            historyWrapper.PromoteDate = record.PromoteDate;
            historyWrapper.Promoter = record.Promoter;
            historyWrapper.Submitter = record.Submitter;
            historyWrapper.Title = record.Title;
            historyWrapper.Lane = lane;

            return historyWrapper;
        }

        #endregion

        protected override void Dispose(bool disposing)
        {
            this.db.Dispose();
            base.Dispose(disposing);
        }
    }
}
