using personal_site_api.Infrastructure;
using System;
using System.ComponentModel.DataAnnotations;

namespace personal_site_api.Models
{
    public class Comment
    {
        public int Id { get; set; }

        [Required]
        public string Body { get; set; }

        public Entry Entry { get; set; }

        [Required]
        public int EntryId { get; set; }

        public DateTime DatePosted { get; set; }
    }
}