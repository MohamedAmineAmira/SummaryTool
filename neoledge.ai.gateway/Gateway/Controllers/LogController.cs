using Gateway.Services;
using Microsoft.AspNetCore.Mvc;

namespace Gateway.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LogController : ControllerBase
    {
        private readonly ILogService _logService;

        public LogController(ILogService logService)
        {
            _logService = logService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> getLog(int id)
        {
            var logs = await _logService.GetLog(id);
            return Ok(logs);

        }

    }
}
