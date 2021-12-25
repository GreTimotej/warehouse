using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using web.Data;
using web.Models;

namespace web.Controllers_Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class DistributorApiController : ControllerBase
    {
        private readonly WarehouseContext _context;

        public DistributorApiController(WarehouseContext context)
        {
            _context = context;
        }

        // GET: api/DistributorApi
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Distributor>>> GetDistributors()
        {
            return await _context.Distributors.ToListAsync();
        }

        // GET: api/DistributorApi/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Distributor>> GetDistributor(int id)
        {
            var distributor = await _context.Distributors.FindAsync(id);

            if (distributor == null)
            {
                return NotFound();
            }

            return distributor;
        }

        // PUT: api/DistributorApi/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDistributor(int id, Distributor distributor)
        {
            if (id != distributor.ID)
            {
                return BadRequest();
            }

            _context.Entry(distributor).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DistributorExists(id))
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

        // POST: api/DistributorApi
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Distributor>> PostDistributor(Distributor distributor)
        {
            _context.Distributors.Add(distributor);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetDistributor", new { id = distributor.ID }, distributor);
        }

        // DELETE: api/DistributorApi/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDistributor(int id)
        {
            var distributor = await _context.Distributors.FindAsync(id);
            if (distributor == null)
            {
                return NotFound();
            }

            _context.Distributors.Remove(distributor);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool DistributorExists(int id)
        {
            return _context.Distributors.Any(e => e.ID == id);
        }
    }
}
