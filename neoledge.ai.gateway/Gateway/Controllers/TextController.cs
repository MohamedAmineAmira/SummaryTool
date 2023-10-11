using Gateway.Data;
using Gateway.Models;
using Gateway.Models.Presenter;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace Gateway.Controllers
{
    [Route("api/text")]
    [ApiController]
    public class TextController : ControllerBase
    {
        private readonly TextDbContext _context;

        public TextController(TextDbContext context) => _context = context;

        // GET: api/texts
        [HttpGet]
        public async Task<IEnumerable<Text>> Get()
        {
            var texts = await _context.Texts.ToListAsync();

            var sortedTexts = texts.OrderByDescending(text => DateTime.ParseExact(text.CreatedDATE, "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture));

            return sortedTexts;
        }

        // GET: api/texts/{id}
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Text), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(long id)
        {
            var text = await _context.Texts.FindAsync(id);
            return text == null ? NotFound() : Ok(text);
        }


        // POST: api/texts
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> Create([FromForm] TextPresenter textPresenter)
        {
            var text = new Text
            {
                Title = textPresenter.Title,
                Type = textPresenter.Type,
                Language = textPresenter.Language,
                PlainText = textPresenter.PlainText,
                Priority = textPresenter.Priority,
                CreatedDATE = textPresenter.CreatedDATE
            };
            await _context.Texts.AddAsync(text);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = text.Id }, text);
        }

        // PUT: api/texts/edit
        [HttpPut("edit")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Update(Text text)
        {
            var textInDb = await _context.Texts.FindAsync(text.Id);
            if (textInDb == null)
                return BadRequest();

            // Copy properties from 'text' to 'textInDb'
            textInDb.State = text.State;  // Update other properties as needed
            textInDb.PrepareText = text.PrepareText;
            textInDb.ProcessText = text.ProcessText;
            await _context.SaveChangesAsync();

            return NoContent();
        }


        // DELETE: api/texts/{id}
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(long id)
        {
            var textToDelete = await _context.Texts.FindAsync(id);
            if (textToDelete == null) return NotFound();

            _context.Texts.Remove(textToDelete);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        [HttpGet("search")]
        [ProducesResponseType(typeof(Text), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetLastText()
        {
            var orderedText = await _context.Texts
                .Where(c => c.State != State.Done)
                .OrderByDescending(c => c.Priority) // Then, order by priorite in ascending order.
                .ThenByDescending(c => c.CreatedDATE) // First, order by CreatedDATE in descending order.
                .FirstOrDefaultAsync();
            if (orderedText == null)
            {
                return NoContent(); // Return a 204 No Content response.
            }

            return Ok(orderedText); // Return the first matching text if found.


        }
    }
}
