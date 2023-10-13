using Gateway.Data;
using Gateway.Models;
using Gateway.Models.Presenter;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Gateway.Controllers
{
    [Route("api/textAnalyticsToolbox/")]
    [ApiController]
    public class TextAnalyticsToolboxController : ControllerBase
    {
        private readonly TextDbContext _context;

        public TextAnalyticsToolboxController(TextDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TextAnalyticsToolbox>>> GetTextAnalyticsToolboxes()
        {
            var toolboxes = await _context.TextAnalyticsToolboxes.Include(t => t.TextProcessors).ToListAsync();

            string json = JsonConvert.SerializeObject(
                toolboxes,
                Formatting.Indented,
                new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                });

            return Content(json, "application/json");
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TextAnalyticsToolbox>> GetTextAnalyticsToolbox(int id)
        {
            var textAnalyticsToolbox = await _context.TextAnalyticsToolboxes
                .Include(t => t.TextProcessors)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (textAnalyticsToolbox == null)
            {
                return NotFound();
            }

            string json = JsonConvert.SerializeObject(
                textAnalyticsToolbox,
                Formatting.Indented,
                new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                });

            return Content(json, "application/json");
        }



        [HttpPost]
        public async Task<ActionResult<TextAnalyticsToolbox>> AddTextAnalyticsToolbox(TextAnalyticsToolboxPresenter textAnalyticsToolboxPresenter)
        {
            var textAnalyticsToolbox = new TextAnalyticsToolbox
            {
                Name = textAnalyticsToolboxPresenter.Name,
                IsActive = textAnalyticsToolboxPresenter.IsActive,
            };

            _context.TextAnalyticsToolboxes.Add(textAnalyticsToolbox);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetTextAnalyticsToolbox), new { id = textAnalyticsToolbox.Id }, textAnalyticsToolbox);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<TextAnalyticsToolbox>> UpdateTextAnalyticsToolbox(int id, TextAnalyticsToolboxPresenter textAnalyticsToolboxPresenter)
        {
            var textAnalyticsToolbox = await _context.TextAnalyticsToolboxes.FindAsync(id);

            if (textAnalyticsToolbox == null)
            {
                return NotFound();
            }

            textAnalyticsToolbox.Name = textAnalyticsToolboxPresenter.Name;
            textAnalyticsToolbox.IsActive = textAnalyticsToolboxPresenter.IsActive;

            _context.Entry(textAnalyticsToolbox).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return Ok(textAnalyticsToolbox);
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTextAnalyticsToolbox(int id)
        {
            var textAnalyticsToolbox = await _context.TextAnalyticsToolboxes.FindAsync(id);
            if (textAnalyticsToolbox == null)
            {
                return NotFound();
            }

            _context.TextAnalyticsToolboxes.Remove(textAnalyticsToolbox);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TextAnalyticsToolboxExists(int id)
        {
            return _context.TextAnalyticsToolboxes.Any(e => e.Id == id);
        }

        [HttpGet("search/{language}")]
        public async Task<ActionResult<string>> SearchToolbox(string language)
        {
            // Step 1: Search for an active Toolbox
            var activeToolbox = await _context.TextAnalyticsToolboxes
                .FirstOrDefaultAsync(t => t.IsActive);

            if (activeToolbox == null)
            {
                return NotFound("No active toolbox found.");
            }

            // Step 2: Search for an active TextProcessor with the same language
            var activeTextProcessor = await _context.TextProcessors
                .FirstOrDefaultAsync(tp => tp.Language == language
                                          && tp.Toolbox.Id == activeToolbox.Id
                                          && tp.IsActive);

            if (activeTextProcessor == null)
            {
                return NotFound("No active TextProcessor found with the specified language.");
            }

            // Step 3: Return the URL from the TextProcessor
            return activeTextProcessor.Url;
        }

    }
}

