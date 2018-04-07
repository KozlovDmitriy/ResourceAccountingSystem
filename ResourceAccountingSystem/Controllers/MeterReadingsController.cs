using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ResourceAccountingSystem.Model;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ResourceAccountingSystem.Controllers
{
    [Produces("application/json")]
    [Route("api/MeterReadings")]
    public class MeterReadingsController : Controller
    {
        private readonly ResourceAccountingContext _context;

        public MeterReadingsController(ResourceAccountingContext context)
        {
            _context = context;
        }

        // GET: api/MeterReadings
        [HttpGet]
        public IEnumerable<MeterReading> GetMeterReading()
        {
            return _context.MeterReading;
        }

        // GET: api/MeterReadings/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetMeterReading([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var meterReading = await _context.MeterReading.SingleOrDefaultAsync(m => m.Id == id);

            if (meterReading == null)
            {
                return NotFound();
            }

            return Ok(meterReading);
        }

        // PUT: api/MeterReadings/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMeterReading([FromRoute] int id, [FromBody] MeterReading meterReading)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != meterReading.Id)
            {
                return BadRequest();
            }

            _context.Entry(meterReading).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MeterReadingExists(id))
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

        // POST: api/MeterReadings
        [HttpPost]
        public async Task<IActionResult> PostMeterReading([FromBody] MeterReading meterReading)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.MeterReading.Add(meterReading);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetMeterReading", new { id = meterReading.Id }, meterReading);
        }

        // DELETE: api/MeterReadings/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMeterReading([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var meterReading = await _context.MeterReading.SingleOrDefaultAsync(m => m.Id == id);
            if (meterReading == null)
            {
                return NotFound();
            }

            _context.MeterReading.Remove(meterReading);
            await _context.SaveChangesAsync();

            return Ok(meterReading);
        }

        private bool MeterReadingExists(int id)
        {
            return _context.MeterReading.Any(e => e.Id == id);
        }
    }
}