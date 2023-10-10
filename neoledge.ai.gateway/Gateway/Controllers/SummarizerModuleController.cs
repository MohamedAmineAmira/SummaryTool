using Gateway.Data;
using Gateway.Models;
using Gateway.Models.Presenter;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Gateway.Controllers
{
    [Route("api/SummarizerModule")]
    [ApiController]
    public class SummarizerModuleController : ControllerBase
    {
        private readonly TextDbContext _context;

        public SummarizerModuleController(TextDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<SummarizerModule>>> GetSummarizerModules()
        {
            return await _context.SummarizerModules.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<SummarizerModule>> GetSummarizerModuleById(int id)
        {
            var summarizerModule = await _context.SummarizerModules.FindAsync(id);

            if (summarizerModule == null)
            {
                return NotFound();
            }

            return summarizerModule;
        }

        [HttpPost]
        public async Task<ActionResult<SummarizerModule>> AddSummarizerModule([FromForm] SummerizeModulePresenter presenter)
        {

            var toolbox = await _context.TextAnalyticsToolboxes.FindAsync(presenter.IdToolbox);

            if (toolbox == null)
            {
                return BadRequest("Invalid Toolbox Id");
            }

            var summarizerModule = new SummarizerModule
            {
                IsActive = presenter.IsActive,
                Language = presenter.Language,
                Name = presenter.Name,
                Url = presenter.Url,
                Toolbox = toolbox

            };
            var a = _context.SummarizerModules.Add(summarizerModule);
            await _context.SaveChangesAsync();
            var id = a.Entity.Id;
            var saved = await _context.SummarizerModules.FindAsync(id);

            string json = JsonConvert.SerializeObject(saved, Formatting.Indented, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
            return Ok(json);
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> EditSummarizerModule(int id, SummarizerModule summarizerModule)
        {
            if (id != summarizerModule.Id)
            {
                return BadRequest();
            }

            _context.Entry(summarizerModule).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SummarizerModuleExists(id))
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
        public async Task<IActionResult> DeleteSummarizerModule(int id)
        {
            var summarizerModule = await _context.SummarizerModules.FindAsync(id);
            if (summarizerModule == null)
            {
                return NotFound();
            }

            _context.SummarizerModules.Remove(summarizerModule);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool SummarizerModuleExists(int id)
        {
            return _context.SummarizerModules.Any(e => e.Id == id);
        }
    }
}
