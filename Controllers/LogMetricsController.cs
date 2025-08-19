using Microsoft.AspNetCore.Mvc;
using BibliotecaApi.Data;
using BibliotecaApi.Models;
using Microsoft.EntityFrameworkCore;

namespace BibliotecaApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LogMetricsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public LogMetricsController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<LogMetric>>> GetMetrics()
        {
            return await _context.LogMetrics.ToListAsync();
        }
    }
}