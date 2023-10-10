using Gateway.Data;
using Gateway.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Gateway.Controllers
{
    [Route("api/datapreprocessor")]
    [ApiController]
    public class DataPreprocessorController : ControllerBase
    {
        private readonly TextDbContext _context;

        public DataPreprocessorController(TextDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<DataPreprocessor>>> GetDataPreprocessors()
        {
            return await _context.DataPreprocessors.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<DataPreprocessor>> GetDataPreprocessorById(int id)
        {
            var dataPreprocessor = await _context.DataPreprocessors.FindAsync(id);

            if (dataPreprocessor == null)
            {
                return NotFound();
            }
            return dataPreprocessor;
        }

        [HttpPost]
        public async Task<ActionResult<DataPreprocessor>> AddDataPreprocessor(DataPreprocessor dataPreprocessor)
        {
            _context.DataPreprocessors.Add(dataPreprocessor);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetDataPreprocessorById), new { id = dataPreprocessor.Id }, dataPreprocessor);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> EditDataPreprocessor(int id, DataPreprocessor dataPreprocessor)
        {
            if (id != dataPreprocessor.Id)
            {
                return BadRequest();
            }

            _context.Entry(dataPreprocessor).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DataPreprocessorExists(id))
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

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDataPreprocessor(int id)
        {
            var dataPreprocessor = await _context.DataPreprocessors.FindAsync(id);
            if (dataPreprocessor == null)
            {
                return NotFound();
            }

            _context.DataPreprocessors.Remove(dataPreprocessor);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool DataPreprocessorExists(int id)
        {
            return _context.DataPreprocessors.Any(e => e.Id == id);
        }
    }
}
