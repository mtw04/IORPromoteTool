using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml.Serialization;
using IORPromoteTool.Models;
using LeankitLibrary;
using LeankitLibrary.Entities;
using LeankitLibrary.EntitiesCustom;
using LeankitLibrary.Helper;

namespace IORPromoteTool.Controllers
{
    public class PromoteController : AccountController
    {
        #region Private Variables

        public PromoteController()
        {
        }

        private InputData inputSettings = null;
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
        protected KanbanOperation KanbanAPI
        {
            get
            {
                this.kanbanAPI = new KanbanOperation(this.InputSettings);
                return this.kanbanAPI;
            }
        }

        #endregion

        #region Private Functions

        private void GetBoardSettings()
        {
            this.inputSettings = new InputData();

            this.inputSettings.BoardId = this.IORSettings.BoardId;
            this.inputSettings.PromoteLane = this.IORSettings.PromoteLane;
            this.inputSettings.TempPath = this.IORSettings.TempPath;
            this.inputSettings.LogInfo = this.IORSettings.LogInfo;
            this.inputSettings.LogDetails = this.IORSettings.LogDetails;
            this.inputSettings.Environment = this.IORSettings.Environment;

            this.inputSettings.InputParam = new Dictionary<string, string>();
            this.inputSettings.JsonParam = new Dictionary<string, string>();
            this.inputSettings.KanbanCard = null;
            this.inputSettings.Operation = null;
        }

        public List<Card> GetBacklogList()
        {
            KanbanOperation KanbanAPI = new KanbanOperation(this.InputSettings);
            List<Card> backlogList = this.KanbanAPI.GetBacklog();

            backlogList = this.ProcessList(backlogList);
            return backlogList;
        }

        private List<Card> ProcessList(List<Card> backlogList)
        {
            foreach (Card card in backlogList)
            {
                // Sanitize description
                //if (card.Description.IndexOf(@"<p>") == 0)
                //{
                //    card.Description = card.Description.Substring(3, card.Description.Length - 7);
                //}

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

        public void KanbanMove(Card card)
        {
            this.InputSettings.KanbanCard = card;
            this.InputSettings.Operation = "MoveCard";
            DictionaryHelper.AddToDict(this.InputSettings.InputParam, "Operation", this.InputSettings.Operation);
            DictionaryHelper.AddToDict(this.InputSettings.InputParam, "BoardLane", this.InputSettings.PromoteLane);
            DictionaryHelper.AddToDict(this.InputSettings.InputParam, "CardId", Convert.ToString(this.InputSettings.KanbanCard.Id));
            DictionaryHelper.AddToDict(this.InputSettings.InputParam, "BoardId", this.InputSettings.BoardId);

            this.KanbanAPI.AutoRun();
        }

        public void KanbanUpdate(Card card)
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

        public PromoteRecord PopulatePromoRecord(Card card, string userFullName)
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

        #endregion
    }
}
