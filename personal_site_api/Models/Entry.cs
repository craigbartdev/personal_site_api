using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace personal_site_api.Models
{
    public class Entry
    {
        public int Id { get; set; }

        //in StringLength ErrorMessage {0} is title, {1} is maxlength, {2} is minlength
        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be between {2} and {1} characters long.", MinimumLength = 1)]
        public string Title { get; set; }

        [Required]
        public string Body { get; set; }

        //nullible in dto
        //sql server sees nullible datetimes as type datetime2
        public DateTime DatePosted { get; set; }
    }
}