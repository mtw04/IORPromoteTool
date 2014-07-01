using System.Collections.Generic;
using System;
using LeankitLibrary.Entities;
using System.ComponentModel.DataAnnotations;

namespace IORPromoteTool.Models
{
    public class UserWrapper
    {
        public PromoteUser PromoteUser { get; set; }
    }

    public class PromoteCardWrapper : UserWrapper
    {
        public Card Card { get; set; }
        public int DaysToEligible { get; set; }

        public PromoteCardWrapper(PromoteUser user, Card card)
        {
            this.PromoteUser = user;
            this.Card = card;
        }

        public PromoteCardWrapper(PromoteUser user, int eligible)
        {
            this.PromoteUser = user;
            this.DaysToEligible = eligible;
        }
    }

    public class PromoteUserWrapper : UserWrapper
    {
        public List<string> UserList { get; set; }
    }

    public class PromoteHistoryWrapper
    {
        public int CardId { get; set; }
        public string Submitter { get; set; }
        public string Title { get; set; }
        public string Lane { get; set; }
        public string Promoter { get; set; }

        [DataType(DataType.Date)]
        public DateTime PromoteDate { get; set; }
    }
}