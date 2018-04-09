using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ResourceAccountingSystem.Model;
using System;
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

        public class ReadingData {
            public string readingType;
            public string readingIdentifier;
            public int readingValue;
        }

        // POST: api/MeterReadings
        [HttpPost]
        public async Task<IActionResult> PostMeterReading([FromBody] ReadingData meterReading)
        {
            if (meterReading == null)
                return BadRequest("Invalid arguments");

            if (meterReading.readingValue == default(int))
                return BadRequest($"Field {nameof(meterReading.readingValue)} is required");

            if (meterReading.readingValue < 0)
                return BadRequest($"New value must be > 0");

            string meterSn = null;
            switch (meterReading.readingType) {
                case "house":
                    meterSn = await _context.Meter
                        .Where(i => i.HouseId.ToString() == meterReading.readingIdentifier)
                        .Select(i => i.SerialNumber)
                        .FirstOrDefaultAsync();
                    break;
                case "meter":
                    meterSn = await _context.Meter
                        .Where(i => i.SerialNumber.ToString() == meterReading.readingIdentifier)
                        .Select(i => i.SerialNumber)
                        .FirstOrDefaultAsync();
                    break;
                default:
                    return BadRequest($"Invalid argument {meterReading.readingType}");
            }

            if (String.IsNullOrEmpty(meterSn))
                return BadRequest("Meter not found");

            var beforeValue = await _context.MeterReading
                .Where(i => i.MeterSerialNumber == meterSn)
                .OrderByDescending(i => i.Value)
                .Select(i => i.Value)
                .FirstOrDefaultAsync();

            if (beforeValue > meterReading.readingValue)
                return BadRequest($"New value: {nameof(meterReading.readingValue)} less then before value: {beforeValue}");

            var reading = new MeterReading {
                MeterSerialNumber = meterSn,
                ReadingDateTime = DateTime.UtcNow,
                Value = meterReading.readingValue
            };
            await _context.MeterReading.AddAsync(reading);
            await _context.SaveChangesAsync();
            return Ok();
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