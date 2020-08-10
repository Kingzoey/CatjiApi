﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CatjiApi.Models;

namespace CatjiApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LikeblogcommentsController : ControllerBase
    {
        private readonly ModelContext _context;

        public LikeblogcommentsController(ModelContext context)
        {
            _context = context;
        }

        // GET: api/Likeblogcomments
        [HttpGet]
        public IEnumerable<Likeblogcomment> GetLikeblogcomment()
        {
            return _context.Likeblogcomment;
        }

        // GET: api/Likeblogcomments/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetLikeblogcomment([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var likeblogcomment = await _context.Likeblogcomment.FindAsync(id);

            if (likeblogcomment == null)
            {
                return NotFound();
            }

            return Ok(likeblogcomment);
        }

        // PUT: api/Likeblogcomments/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutLikeblogcomment([FromRoute] int id, [FromBody] Likeblogcomment likeblogcomment)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != likeblogcomment.Usid)
            {
                return BadRequest();
            }

            _context.Entry(likeblogcomment).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LikeblogcommentExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Likeblogcomments
        [HttpPost]
        public async Task<IActionResult> PostLikeblogcomment([FromBody] Likeblogcomment likeblogcomment)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Likeblogcomment.Add(likeblogcomment);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (LikeblogcommentExists(likeblogcomment.Usid))
                {
                    return new StatusCodeResult(StatusCodes.Status409Conflict);
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetLikeblogcomment", new { id = likeblogcomment.Usid }, likeblogcomment);
        }

        // DELETE: api/Likeblogcomments/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLikeblogcomment([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var likeblogcomment = await _context.Likeblogcomment.FindAsync(id);
            if (likeblogcomment == null)
            {
                return NotFound();
            }

            _context.Likeblogcomment.Remove(likeblogcomment);
            await _context.SaveChangesAsync();

            return Ok(likeblogcomment);
        }

        private bool LikeblogcommentExists(int id)
        {
            return _context.Likeblogcomment.Any(e => e.Usid == id);
        }
    }
}