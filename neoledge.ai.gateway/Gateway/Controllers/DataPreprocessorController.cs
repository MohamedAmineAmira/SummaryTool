using Gateway.Data;
using Gateway.Models;
using Gateway.Models.Presenter;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Gateway.Controllers
{
    [Route("api/dataPreprocessor/")]
    [ApiController]
    public class DataPreprocessorController : ControllerBase
    {
        private readonly TextDbContext _context;

        public DataPreprocessorController(TextDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<DataPreprocessor>>> GetDataPreprocessorsOrderedByLanguage()
        {
            var dataPreprocessorsOrdered = await _context.DataPreprocessors
                .OrderBy(dp => dp.Language)
                .ToListAsync();

            return dataPreprocessorsOrdered;
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

        [HttpGet("getUrl/{language}")]
        public IActionResult GetDataPreprocessorUrl(string language)
        {
            var dataPreprocessor = _context.DataPreprocessors
                .FirstOrDefault(d => d.Language == language && d.IsActive);

            if (dataPreprocessor != null)
            {
                return Ok(dataPreprocessor.Url);
            }

            return NotFound("NotFound");
        }

        [HttpPost]
        public async Task<ActionResult<DataPreprocessor>> AddDataPreprocessor([FromForm] DataPreprocessorPresenter dataPreprocessorPresenter)
        {
            var dataPreprocessor = new DataPreprocessor
            {
                Name = dataPreprocessorPresenter.Name,
                Language = dataPreprocessorPresenter.Language,
                IsActive = dataPreprocessorPresenter.IsActive,
                Url = dataPreprocessorPresenter.Url
            };
            _context.DataPreprocessors.Add(dataPreprocessor);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetDataPreprocessorById), new { id = dataPreprocessor.Id }, dataPreprocessor);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> EditDataPreprocessor(int id, DataPreprocessorPresenter dataPreprocessorPresenter)
        {
            var existingDataPreprocessor = await _context.DataPreprocessors.FindAsync(id);

            if (existingDataPreprocessor == null)
            {
                return NotFound();
            }

            existingDataPreprocessor.Name = dataPreprocessorPresenter.Name;
            existingDataPreprocessor.Language = dataPreprocessorPresenter.Language;
            existingDataPreprocessor.IsActive = dataPreprocessorPresenter.IsActive;
            existingDataPreprocessor.Url = dataPreprocessorPresenter.Url;

            _context.Entry(existingDataPreprocessor).State = EntityState.Modified;

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
