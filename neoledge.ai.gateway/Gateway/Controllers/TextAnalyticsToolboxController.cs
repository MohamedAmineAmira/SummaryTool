using Gateway.Data;
using Gateway.Models;
using Gateway.Models.Presenter;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Gateway.Controllers
{
    [Route("api/TextAnalyticsToolbox")]
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
            return await _context.TextAnalyticsToolboxes.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TextAnalyticsToolbox>> GetTextAnalyticsToolbox(int id)
        {
            var textAnalyticsToolbox = await _context.TextAnalyticsToolboxes.FindAsync(id);

            if (textAnalyticsToolbox == null)
            {
                return NotFound();
            }
            return textAnalyticsToolbox;
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
        public async Task<IActionResult> EditTextAnalyticsToolbox(int id, TextAnalyticsToolbox textAnalyticsToolbox)
        {
            if (id != textAnalyticsToolbox.Id)
            {
                return BadRequest();
            }

            _context.Entry(textAnalyticsToolbox).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TextAnalyticsToolboxExists(id))
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
    }
}

