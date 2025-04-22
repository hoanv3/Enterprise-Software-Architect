using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos;
using Microsoft.EntityFrameworkCore;
using ReportingSolution.AKSWeatherApi.Models;
using ReportingSolution.Data.NoSqlModel;

namespace ReportingSolution.AKSWeatherApi.Controllers
{
    [ApiController]
    [Route("api/optimistic-cosmos")]
    public class CosmosOptimisticConcurrencyController : ControllerBase
    {
        private readonly CosmosDbContext _context;

        public CosmosOptimisticConcurrencyController(CosmosDbContext context)
        {
            _context = context;
        }

        [HttpGet("{section}/{id}")]
        public async ValueTask<IActionResult> GetById(string id, string section, CancellationToken cancellationToken)
        {
            var entity = await _context
                .Set<Sport>()
                .Where(x => x.Section == section && x.Id == id)
                .FirstOrDefaultAsync(cancellationToken);
            if (entity == null)
            {
                return NotFound();
            }

            return Ok(entity);
        }

        [HttpPost("")]
        //[AutoValidateAntiforgeryToken]
        public async ValueTask<IActionResult> Post(SportModel model, CancellationToken cancellationToken)
        {
            var document = new Sport
            {
                SportName = model.SportName,
                Section = model.Section,
            };
            _context.Set<Sport>().Add(document);

            await _context.SaveChangesAsync(cancellationToken);
            return Ok(document);
        }

        [HttpPut("")]
        //[AutoValidateAntiforgeryToken]
        public async ValueTask<IActionResult> Put(SportModel model, CancellationToken cancellationToken)
        {
            if (!Request.Headers.TryGetValue("If-Match", out var etag) || string.IsNullOrWhiteSpace(etag))
            {
                return BadRequest("Missing If-Match header");
            }

            var document = await _context
                .Set<Sport>()
                .FirstOrDefaultAsync(x => x.Id == model.Id && x.Section == model.Section, cancellationToken);
            if (document is null)
            {
                return NotFound();
            }

            document.SportName = model.SportName;
            _context.Entry(document).OriginalValues["Etag"] = etag.ToString();

            try
            {
                await _context.SaveChangesAsync(cancellationToken);
                return Ok(document);
            }
            catch (CosmosException ex) when (ex.StatusCode == HttpStatusCode.PreconditionFailed)
            {
                return Conflict("Document has been modified by another process.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
