using System;
using System.ComponentModel.DataAnnotations;

namespace personal_site_api.Dtos
{
    public class CommentDto
    {
        public int Id { get; set; }

        [Required]
        public string Body { get; set; }

        public int EntryId { get; set; }

        public DateTime? DatePosted { get; set; }
    }
}