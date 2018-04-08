using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ResourceAccountingSystem.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ResourceAccountingSystem.Controllers
{
    [Produces("application/json")]
    [Route("api/Houses")]
    public class HousesController : Controller
    {
        private const int PAGE_SIZE = 2;
        private readonly ResourceAccountingContext _context;

        public HousesController(ResourceAccountingContext context)
        {
            _context = context;
        }

        // GET: api/Houses/GetPagesCount
        [HttpGet]
        [Route("GetPagesCount")]
        public async Task<IActionResult> GetPagesCount()
        {
            var count = await _context.House.CountAsync();
            var pagesCount = count / PAGE_SIZE + (count % PAGE_SIZE > 0 ? 1 : 0);
            return Ok(pagesCount);
        }

        // GET: api/Houses/GetPage?page=1
        [HttpGet]
        [Route("GetPage")]
        public async Task<IActionResult> GetPage(int page, int? houseIdFilter = null)
        {
            if (page < 1)
                return NoContent();
            
            var query = _context.House.Include(i => i.Street).AsQueryable();
            if (houseIdFilter.HasValue)
            {
                query = query.Where(i => i.Id == houseIdFilter);
            }
            else
            {
                query = query.Skip(PAGE_SIZE * (page - 1)).Take(PAGE_SIZE);
            }
            var houses = await query                
                .Select(i => new {
                    id = i.Id,
                    zip = i.Zip,
                    houseNumber = i.HouseNumber,
                    street = i.Street.Name
                })
                .ToListAsync();
            return Ok(houses);
        }

        // GET: api/Houses/Get?id=5
        [HttpGet]
        [Route("Get")]
        public async Task<IActionResult> GetHouse(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var house = await _context.House.SingleOrDefaultAsync(m => m.Id == id);

            if (house == null)
            {
                return NotFound();
            }

            return Ok(house);
        }


        public class HouseData
        {
            public string street;
            public string zip;
            public string houseNumber;
        }


        // PUT: api/Houses/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutHouse([FromRoute] int id, [FromBody] HouseData houseData)
        {
            var street = houseData.street;
            var zip = houseData.zip;
            var houseNumber = houseData.houseNumber;

            if (id == default(int))
                return BadRequest($"Field {nameof(id)} is required");

            if (String.IsNullOrWhiteSpace(street))
                return BadRequest($"Field {nameof(street)} is required");

            if (String.IsNullOrWhiteSpace(zip))
                return BadRequest($"Field {nameof(zip)} is required");

            if (String.IsNullOrWhiteSpace(houseNumber))
                return BadRequest($"Field {nameof(houseNumber)} is required");

            int hn = 0;
            if (!int.TryParse(houseNumber, out hn))
                return BadRequest($"Field {nameof(houseNumber)} is not correct");

            var streetObj = await _context.Street.FirstOrDefaultAsync(i => i.Name == street);
            if (streetObj == null)
            {
                streetObj = new Street { Name = street };
            }
            else
            {
                var streetId = streetObj.Id;
                var anotherHouse = await _context.House
                    .FirstOrDefaultAsync(i => i.HouseNumber == hn && i.StreetId == streetId && i.Id != id);
                if (anotherHouse != null)
                    return BadRequest($"Another house with Id: {anotherHouse.Id} has the same address");
            }

            var house = await _context.House.FirstOrDefaultAsync(i => i.Id == id);
            if (house == null)
                return NotFound();

            house.Street = streetObj;
            house.StreetId = streetObj.Id;
            house.Zip = zip;
            house.HouseNumber = hn;

            _context.Entry(house).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        // POST: api/Houses
        [HttpPost]
        public async Task<IActionResult> PostHouse([FromBody] HouseData houseData)//string street, [FromBody] string zip, [FromBody] string houseNumber)
        {
            var street = houseData.street;
            var zip = houseData.zip;
            var houseNumber = houseData.houseNumber;

            if (String.IsNullOrWhiteSpace(street))
                return BadRequest($"Field {nameof(street)} is required");

            if (String.IsNullOrWhiteSpace(zip))
                return BadRequest($"Field {nameof(zip)} is required");

            if (String.IsNullOrWhiteSpace(houseNumber))
                return BadRequest($"Field {nameof(houseNumber)} is required");

            int hn = 0;
            if (!int.TryParse(houseNumber, out hn))
                return BadRequest($"Field {nameof(houseNumber)} is not correct");

            var streetObj = await _context.Street.FirstOrDefaultAsync(i => i.Name == street);
            if (streetObj == null)
            {
                streetObj = new Street { Name = street };
            }
            else
            {
                var streetId = streetObj.Id;
                var anotherHouse = await _context.House
                    .FirstOrDefaultAsync(i => i.HouseNumber == hn && i.StreetId == streetId);
                if (anotherHouse != null)
                    return BadRequest($"Another house with Id: {anotherHouse.Id} has the same address");
            }
            var house = new House { Zip = zip, HouseNumber = hn, Street = streetObj, StreetId = streetObj.Id };
            _context.House.Add(house);
            await _context.SaveChangesAsync();

            return Ok();
        }

        // DELETE: api/Houses/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteHouse([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var house = await _context.House.SingleOrDefaultAsync(m => m.Id == id);
            if (house == null)
            {
                return NotFound();
            }

            _context.House.Remove(house);
            await _context.SaveChangesAsync();

            return Ok(house);
        }

        private bool HouseExists(int id)
        {
            return _context.House.Any(e => e.Id == id);
        }
    }
}