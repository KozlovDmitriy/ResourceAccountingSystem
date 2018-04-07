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
    [Route("api/Meters")]
    public class MetersController : Controller
    {
        private readonly ResourceAccountingContext _context;

        public MetersController(ResourceAccountingContext context)
        {
            _context = context;
        }

        // GET: api/Meters
        [HttpGet]
        public IEnumerable<Meter> GetMeter()
        {
            return _context.Meter;
        }

        // GET: api/Meters/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetMeter([FromRoute] string id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var meter = await _context.Meter.SingleOrDefaultAsync(m => m.SerialNumber == id);

            if (meter == null)
            {
                return NotFound();
            }

            return Ok(meter);
        }

        // PUT: api/Meters/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMeter([FromRoute] string id, [FromBody] Meter meter)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != meter.SerialNumber)
            {
                return BadRequest();
            }

            _context.Entry(meter).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MeterExists(id))
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

        // POST: api/Meters
        [HttpPost]
        public async Task<IActionResult> PostMeter([FromBody] Meter meter)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Meter.Add(meter);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (MeterExists(meter.SerialNumber))
                {
                    return new StatusCodeResult(StatusCodes.Status409Conflict);
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetMeter", new { id = meter.SerialNumber }, meter);
        }

        // DELETE: api/Meters/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMeter([FromRoute] string id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var meter = await _context.Meter.SingleOrDefaultAsync(m => m.SerialNumber == id);
            if (meter == null)
            {
                return NotFound();
            }

            _context.Meter.Remove(meter);
            await _context.SaveChangesAsync();

            return Ok(meter);
        }

        private bool MeterExists(string id)
        {
            return _context.Meter.Any(e => e.SerialNumber == id);
        }
    }
}