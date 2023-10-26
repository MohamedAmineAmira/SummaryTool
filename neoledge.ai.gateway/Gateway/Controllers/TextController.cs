using Gateway.Data;
using Gateway.Models;
using Gateway.Models.Presenter;
using Gateway.Services;
using iTextSharp.text.pdf;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Globalization;
using System.Security.Claims;

namespace Gateway.Controllers
{
    [Route("api/text")]
    [ApiController]
    public class TextController : ControllerBase
    {
        private readonly TextDbContext _context;
        private readonly ILogService _logService;

        public TextController(TextDbContext context, ILogService logService)
        {
            _context = context;
            _logService = logService;
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IEnumerable<Text>> Get()
        {
            var texts = await _context.Texts.ToListAsync();
            var sortedTexts = texts.OrderByDescending(text => DateTime.ParseExact(text.CreatedDATE, "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture));
            return sortedTexts;
        }

        [Authorize(Roles = "SimpleUser")]
        [HttpGet("getByUser")]
        public async Task<IEnumerable<Text>> GetByIdUser()
        {
            var idUser = HttpContext.User.FindFirstValue("userID");
            var texts = await _context.Texts.Where(t => t.User.Id == idUser).ToListAsync();
            var sortedTexts = texts.OrderByDescending(text => DateTime.ParseExact(text.CreatedDATE, "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture));
            return sortedTexts;
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Text), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(long id)
        {
            var text = await _context.Texts.FindAsync(id);
            return text == null ? NotFound() : Ok(text);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> Create(TextPresenter textPresenter)
        {
            var idUser = HttpContext.User.FindFirstValue("userID");
            ApplicationUser? user = await _context.Users.FindAsync(idUser);
            var text = new Text
            {
                Title = textPresenter.Title,
                Type = textPresenter.Type,
                Language = textPresenter.Language,
                PlainText = textPresenter.PlainText,
                Priority = textPresenter.Priority,
                CreatedDATE = textPresenter.CreatedDATE,
                User = user
            };
            var a = _context.Texts.Add(text);
            await _context.SaveChangesAsync();
            var id = a.Entity.Id;
            _logService.CreateLog(id);
            var saved = await _context.Texts.FindAsync(id);

            string json = JsonConvert.SerializeObject(saved, Formatting.Indented, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
            return Ok(json);

        }

        [HttpPost("uploadFile")]
        public IActionResult UploadFile(IFormFile pdfFile)
        {
            if (pdfFile == null || pdfFile.Length == 0)
                return BadRequest("Invalid file");

            try
            {
                // Save the file to a temporary location
                var filePath = Path.GetTempFileName();
                using (var stream = System.IO.File.Create(filePath))
                {
                    pdfFile.CopyTo(stream);
                }
                // Process the PDF using iTextSharp
                string extractedText = ProcessPdfText(filePath);
                // Delete the temporary file
                System.IO.File.Delete(filePath);

                return Ok(extractedText);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while processing the PDF: {ex.Message}");
            }
        }

        private string ProcessPdfText(string filePath)
        {
            using (PdfReader reader = new PdfReader(filePath))
            {
                string extractedText = string.Empty;

                for (int i = 1; i <= reader.NumberOfPages; i++)
                {
                    extractedText += iTextSharp.text.pdf.parser.PdfTextExtractor.GetTextFromPage(reader, i);
                }

                return extractedText;
            }
        }

        [HttpPut("edit")]
        [ProducesResponseType(typeof(Text), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateText(TextPresenter textPresenter)
        {
            var text = await _context.Texts.FindAsync(textPresenter.Id);

            if (text == null)
            {
                return NotFound();
            }

            text.Title = textPresenter.Title;
            text.Language = textPresenter.Language;
            text.Type = textPresenter.Type;
            text.PlainText = textPresenter.PlainText;
            text.Priority = textPresenter.Priority;

            if (text.State != (State)textPresenter.State)
            {
                text.State = (State)textPresenter.State;
                await _logService.AddLog(text.Id, textPresenter.State);
            }

            text.PrepareText = textPresenter.PrepareText;
            text.ProcessText = textPresenter.ProcessText;
            text.CreatedDATE = textPresenter.CreatedDATE;

            await _context.SaveChangesAsync();

            return NoContent();
        }

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
                .Where(c => c.State != State.Done && c.State != State.Error)
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
