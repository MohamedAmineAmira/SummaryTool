using Gateway.Data;
using Gateway.Models;
using Gateway.Models.Presenter;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Gateway.Controllers
{
    [Route("api/textProcessor")]
    [ApiController]
    public class TextProcessorController : ControllerBase
    {
        private readonly TextDbContext _context;

        public TextProcessorController(TextDbContext context)
        {
            _context = context;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TextProcessor>> GetTextProcessorById(int id)
        {
            var textProcessor = await _context.TextProcessors
                .Include(t => t.Toolbox)
                .FirstOrDefaultAsync(t => t.Id == id);
            if (textProcessor == null)
            {
                return NotFound();
            }

            string json = JsonConvert.SerializeObject(
                textProcessor,
                Formatting.Indented,
                new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                });

            return Content(json, "application/json");
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TextProcessor>>> GetTextProcessors()
        {
            var textProcessors = await _context.TextProcessors.Include(t => t.Toolbox).OrderBy(t => t.Language).ToListAsync();

            string json = JsonConvert.SerializeObject(
                textProcessors,
                Formatting.Indented,
                new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                });

            return Content(json, "application/json");
        }



        [HttpPost]
        public async Task<ActionResult<TextProcessor>> AddTextProcessor(TextProcessorPresenter presenter)
        {

            var toolbox = await _context.TextAnalyticsToolboxes.FindAsync(presenter.ToolboxId);

            if (toolbox == null)
            {
                return BadRequest("Invalid Toolbox Id");
            }

            var textProcessor = new TextProcessor
            {
                IsActive = presenter.IsActive,
                Language = presenter.Language,
                Name = presenter.Name,
                Url = presenter.Url,
                Toolbox = toolbox

            };
            var a = _context.TextProcessors.Add(textProcessor);
            await _context.SaveChangesAsync();
            var id = a.Entity.Id;
            var saved = await _context.TextProcessors.FindAsync(id);

            string json = JsonConvert.SerializeObject(saved, Formatting.Indented, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
            return Ok(json);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<TextProcessor>> UpdateTextProcessor(int id, TextProcessorPresenter presenter)
        {
            var toolbox = await _context.TextAnalyticsToolboxes.FindAsync(presenter.ToolboxId);

            if (toolbox == null)
            {
                return BadRequest("Invalid Toolbox Id");
            }

            var textProcessor = await _context.TextProcessors.FindAsync(id);

            if (textProcessor == null)
            {
                return NotFound();
            }

            textProcessor.IsActive = presenter.IsActive;
            textProcessor.Language = presenter.Language;
            textProcessor.Name = presenter.Name;
            textProcessor.Url = presenter.Url;
            textProcessor.Toolbox = toolbox;

            _context.Entry(textProcessor).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            // Retrieve the updated entity
            var updatedTextProcessor = await _context.TextProcessors.FindAsync(id);

            string json = JsonConvert.SerializeObject(updatedTextProcessor, Formatting.Indented, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });

            return Ok(json);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTextProcessingModules(int id)
        {
            var textProcessor = await _context.TextProcessors.FindAsync(id);
            if (textProcessor == null)
            {
                return NotFound();
            }

            _context.TextProcessors.Remove(textProcessor);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
