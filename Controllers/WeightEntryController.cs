using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace WeightLossTracker.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WeightEntryController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public WeightEntryController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/WeightEntry
        [HttpGet]
        public async Task<ActionResult<IEnumerable<WeightEntry>>> GetWeightEntries()
        {
            return await _context.WeightEntries.ToListAsync();
        }

        // GET: api/WeightEntry/5
        [HttpGet("{id}")]
        public async Task<ActionResult<WeightEntry>> GetWeightEntry(int id)
        {
            var weightEntry = await _context.WeightEntries.FindAsync(id);

            if (weightEntry == null)
            {
                return NotFound();
            }

            return weightEntry;
        }

        // POST: api/WeightEntry
        [HttpPost]
        public async Task<ActionResult<WeightEntry>> PostWeightEntry(WeightEntry weightEntry)
        {
            _context.WeightEntries.Add(weightEntry);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetWeightEntry), new { id = weightEntry.Id }, weightEntry);
        }

        // PUT: api/WeightEntry/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutWeightEntry(int id, WeightEntry weightEntry)
        {
            if (id != weightEntry.Id)
            {
                return BadRequest();
            }

            _context.Entry(weightEntry).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!WeightEntryExists(id))
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

        // DELETE: api/WeightEntry/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteWeightEntry(int id)
        {
            var weightEntry = await _context.WeightEntries.FindAsync(id);
            if (weightEntry == null)
            {
                return NotFound();
            }

            _context.WeightEntries.Remove(weightEntry);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool WeightEntryExists(int id)
        {
            return _context.WeightEntries.Any(e => e.Id == id);
        }
    }
}