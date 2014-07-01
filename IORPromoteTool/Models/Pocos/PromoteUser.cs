using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace IORPromoteTool.Models
{
    /// <summary>
    /// Partial class which adds additional properties and sets the metadata for the poco.
    /// </summary>
    [MetadataType(typeof(PostMetadata))]
    public partial class PromoteUser
    {
    }

    /// <summary>
    /// Specifies metadata for post.
    /// The class is opt in, so all properties that should be included for JSON serialization should be listed here with JsonProperty.
    /// </summary>
    public class PostMetadata
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Login name is required.")]
        //[DataMember(IsRequired = true)] 
        [Display(Name = "Login name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "User full name is required.")]
        //[DataMember(IsRequired = true)] 
        [Display(Name = "User full name")]
        public string FullName { get; set; }

        //[Required(ErrorMessage = "Promotion frequency is required.")]
        //[DataMember(IsRequired = true)] 
        [Display(Name = "Eligible promotion frequency (days)")]
        [Range(0, 90, ErrorMessage = "Frequency must be between 0 and 90 days.")]
        public int Frequency { get; set; }

        private Nullable<System.DateTime> _LastPromote; public Nullable<System.DateTime> LastPromote { get { return _LastPromote; } set { _LastPromote = DateKindHelper.DefaultToUtc(value); } }
    }
}