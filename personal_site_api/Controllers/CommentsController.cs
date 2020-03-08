using personal_site_api.Dtos;
using personal_site_api.Infrastructure;
using personal_site_api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace personal_site_api.Controllers
{
    [RoutePrefix("api/comments")]
    public class CommentsController : ApiController
    {
        private ApplicationDbContext _context;

        private CommentsController()
        {
            _context = new ApplicationDbContext();
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }

        //api/comments/entry/entryId
        [HttpGet]
        [Route("entry/{entryId:int}")]
        public IHttpActionResult GetCommentsForEntry(int entryId)
        {
            var comments = _context.Comments.Where(c => c.EntryId == entryId)
                .ToList()
                .OrderByDescending(c => c.DatePosted)
                .Select(c => new CommentDto {
                    Id = c.Id,
                    Body = c.Body,
                    EntryId = c.EntryId,
                    DatePosted = c.DatePosted
                });

            return Ok(comments);
        }

        //api/comments/id
        [HttpGet]
        public IHttpActionResult GetComment(int id)
        {
            var comment = _context.Comments.SingleOrDefault(c => c.Id == id);

            if (comment == null)
                return NotFound();

            var commentDto = new CommentDto
            {
                Id = comment.Id,
                Body = comment.Body,
                EntryId = comment.EntryId,
                DatePosted = comment.DatePosted
            };

            return Ok(commentDto);
        }

        //api/comments
        [HttpPost]
        public IHttpActionResult PostComment(CommentDto commentDto)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var comment = new Comment
            {
                Body = commentDto.Body,
                EntryId = commentDto.EntryId,
                DatePosted = DateTime.Now
            };

            _context.Comments.Add(comment);
            _context.SaveChanges();

            commentDto.Id = comment.Id;
            commentDto.DatePosted = comment.DatePosted;

            Uri returnUri = new Uri(Request.RequestUri + "/" + comment.Id);

            return Created(returnUri, commentDto);
        }
    }
}
