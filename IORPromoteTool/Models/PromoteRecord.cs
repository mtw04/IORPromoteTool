//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace IORPromoteTool.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class PromoteRecord
    {
        public int Id { get; set; }
        public string Promoter { get; set; }
        private System.DateTime _PromoteDate; public System.DateTime PromoteDate { get { return _PromoteDate; } set { _PromoteDate = DateKindHelper.DefaultToUtc(value); }}
        public string Title { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        public string Priority { get; set; }
        public string Tags { get; set; }
        public string Deadline { get; set; }
        public int CardId { get; set; }
        public string Submitter { get; set; }
    }
}
