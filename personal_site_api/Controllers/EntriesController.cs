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
    //uses default api routes from Startup configuration
    public class EntriesController : BaseApiController
    {
        private ApplicationDbContext _context;

        public EntriesController()
        {
            _context = new ApplicationDbContext();
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }


        [HttpGet]
        public IHttpActionResult GetEntry(int id)
        {
            var entry = _context.Entries.SingleOrDefault(e => e.Id == id);

            if (entry == null)
                return NotFound();

            var entryDto = new EntryDto
            {
                Id = entry.Id,
                Title = entry.Title,
                Body = entry.Body,
                DatePosted = entry.DatePosted
            };

            return Ok(entryDto);
        }

        [HttpPost]
        public IHttpActionResult PostEntry(EntryDto entryDto)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            //id gets set by db
            var entry = new Entry
            {
                Title = entryDto.Title,
                Body = entryDto.Body,
                DatePosted = DateTime.Now
            };

            _context.Entries.Add(entry);
            _context.SaveChanges();

            //get the id and datetime that was not passed in to dto
            entryDto.Id = entry.Id;
            entryDto.DatePosted = entry.DatePosted;

            Uri returnUri = new Uri(Request.RequestUri + "/" + entry.Id);

            //return 201 with GetEntry. must pass in dto for Created result
            return Created(returnUri, entryDto);
        }
    }
}
