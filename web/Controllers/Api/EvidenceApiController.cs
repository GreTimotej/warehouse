using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using web.Data;
using web.Models;
using web.Filters;

namespace web.Controllers_Api
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiKeyAuth]
    public class EvidenceApiController : ControllerBase
    {
        private readonly WarehouseContext _context;

        public EvidenceApiController(WarehouseContext context)
        {
            _context = context;
        }

        // GET: api/EvidenceApi
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Evidence>>> GetEvidences()
        {
            return await _context.Evidences.ToListAsync();
        }

        // GET: api/EvidenceApi/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Evidence>> GetEvidence(int id)
        {
            var evidence = await _context.Evidences.FindAsync(id);

            if (evidence == null)
            {
                return NotFound();
            }

            return evidence;
        }

        // PUT: api/EvidenceApi/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEvidence(int id, Evidence evidence)
        {
            if (id != evidence.ID)
            {
                return BadRequest();
            }

            _context.Entry(evidence).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EvidenceExists(id))
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

        // POST: api/EvidenceApi
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Evidence>> PostEvidence(Evidence evidence)
        {
            _context.Evidences.Add(evidence);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetEvidence", new { id = evidence.ID }, evidence);
        }

        // DELETE: api/EvidenceApi/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEvidence(int id)
        {
            var evidence = await _context.Evidences.FindAsync(id);
            if (evidence == null)
            {
                return NotFound();
            }

            _context.Evidences.Remove(evidence);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool EvidenceExists(int id)
        {
            return _context.Evidences.Any(e => e.ID == id);
        }
    }
}
