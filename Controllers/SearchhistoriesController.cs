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
    public class SearchhistoriesController : ControllerBase
    {
        private readonly ModelContext _context;

        public SearchhistoriesController(ModelContext context)
        {
            _context = context;
        }

        // GET: api/Searchhistories
        [HttpGet]
        public IEnumerable<Searchhistory> GetSearchhistory()
        {
            return _context.Searchhistory;
        }

        // GET: api/Searchhistories/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetSearchhistory([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var searchhistory = await _context.Searchhistory.FindAsync(id);

            if (searchhistory == null)
            {
                return NotFound();
            }

            return Ok(searchhistory);
        }

        // PUT: api/Searchhistories/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSearchhistory([FromRoute] int id, [FromBody] Searchhistory searchhistory)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != searchhistory.Usid)
            {
                return BadRequest();
            }

            _context.Entry(searchhistory).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SearchhistoryExists(id))
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

        // POST: api/Searchhistories
        [HttpPost]
        public async Task<IActionResult> PostSearchhistory([FromBody] Searchhistory searchhistory)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Searchhistory.Add(searchhistory);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (SearchhistoryExists(searchhistory.Usid))
                {
                    return new StatusCodeResult(StatusCodes.Status409Conflict);
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetSearchhistory", new { id = searchhistory.Usid }, searchhistory);
        }

        // DELETE: api/Searchhistories/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSearchhistory([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var searchhistory = await _context.Searchhistory.FindAsync(id);
            if (searchhistory == null)
            {
                return NotFound();
            }

            _context.Searchhistory.Remove(searchhistory);
            await _context.SaveChangesAsync();

            return Ok(searchhistory);
        }

        private bool SearchhistoryExists(int id)
        {
            return _context.Searchhistory.Any(e => e.Usid == id);
        }
    }
}