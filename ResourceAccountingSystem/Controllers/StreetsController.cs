using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ResourceAccountingSystem.Model;

namespace ResourceAccountingSystem.Controllers
{
    [Produces("application/json")]
    [Route("api/Streets")]
    public class StreetsController : Controller
    {
        private readonly ResourceAccountingContext _context;

        public StreetsController(ResourceAccountingContext context)
        {
            _context = context;
        }

        // GET: api/Streets
        [HttpGet]
        public IEnumerable<Street> GetStreet()
        {
            return _context.Street;
        }

        // GET: api/Streets/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetStreet([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var street = await _context.Street.SingleOrDefaultAsync(m => m.Id == id);

            if (street == null)
            {
                return NotFound();
            }

            return Ok(street);
        }

        // PUT: api/Streets/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutStreet([FromRoute] int id, [FromBody] Street street)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != street.Id)
            {
                return BadRequest();
            }

            _context.Entry(street).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StreetExists(id))
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

        // POST: api/Streets
        [HttpPost]
        public async Task<IActionResult> PostStreet([FromBody] Street street)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Street.Add(street);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetStreet", new { id = street.Id }, street);
        }

        // DELETE: api/Streets/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStreet([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var street = await _context.Street.SingleOrDefaultAsync(m => m.Id == id);
            if (street == null)
            {
                return NotFound();
            }

            _context.Street.Remove(street);
            await _context.SaveChangesAsync();

            return Ok(street);
        }

        private bool StreetExists(int id)
        {
            return _context.Street.Any(e => e.Id == id);
        }
    }
}