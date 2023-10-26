using Gateway.Data;
using Gateway.Models;
using Microsoft.EntityFrameworkCore;

namespace Gateway.Services
{
    public class LogService : ILogService
    {
        private readonly TextDbContext _context;

        public LogService(TextDbContext context) => _context = context;

        public async void CreateLog(long idText)
        {
            Text? text = await _context.Texts.FindAsync(idText);
            Log log = new()
            {
                Text = text,
                Type = 0,
                Value = DateTime.Now,
            };
            _context.Logs.Add(log);
            await _context.SaveChangesAsync();
        }


        public async Task AddLog(long idText, int state)
        {
            DateTime dateTime = DateTime.Now;
            Text? text = await _context.Texts.FindAsync(idText);

            if (text == null)
            {
                // Handle the case where the text with the given id wasn't found.
                return;
            }

            Log log = new Log
            {
                Text = text,
                Type = (Models.Type)state,
                Value = dateTime,
            };

            if (state == 2)
            {
                var dataPre = await _context.DataPreprocessors
                    .Where(d => d.Language == text.Language && d.IsActive)
                    .SingleOrDefaultAsync();

                if (dataPre != null)
                {
                    log.OperationName = dataPre.Name;
                }
                else
                {

                    return;
                }
            }
            else if (state == 4)
            {
                var textProcessor = await _context.TextProcessors
                    .Where(d => d.Language == text.Language && d.IsActive)
                    .SingleOrDefaultAsync();

                if (textProcessor != null)
                {
                    log.OperationName = textProcessor.Name;
                }
                else
                {
                    // Handle the case where no matching TextProcessor was found.
                    return;
                }
            }

            _context.Logs.Add(log);
            await _context.SaveChangesAsync();
        }


        public async Task<IEnumerable<Log>> GetLog(int textId)
        {
            var logs = await _context.Logs.Where(log => log.Text.Id == textId).ToListAsync();
            return logs;
        }


    }
}
