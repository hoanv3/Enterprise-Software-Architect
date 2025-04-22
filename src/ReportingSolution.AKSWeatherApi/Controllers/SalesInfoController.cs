using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos;
using Microsoft.EntityFrameworkCore;
using ReportingSolution.AKSWeatherApi.Models;
using ReportingSolution.Data.NoSqlModel;

namespace ReportingSolution.AKSWeatherApi.Controllers
{
    [ApiController]
    [Route("api/sales-info")]
    public class SalesInfoController : ControllerBase
    {
        private readonly CosmosDbContext _context;

        public SalesInfoController(CosmosDbContext context)
        {
            _context = context;
        }

        [HttpGet("{section}/{id}")]
        public async ValueTask<IActionResult> GetById(string id, string section, CancellationToken cancellationToken)
        {
            var entity = await _context
                .Set<SalesInfo>()
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
        public async ValueTask<IActionResult> Post(SalesInfoModel model, CancellationToken cancellationToken)
        {
            var document = new SalesInfo
            {
                Description = model.Description,
                ProductId = model.ProductId,
                Section = model.Section
            };
            _context.Set<SalesInfo>().Add(document);

            await _context.SaveChangesAsync(cancellationToken);
            return Ok(document);
        }

        [HttpPut("")]
        //[AutoValidateAntiforgeryToken]
        public async ValueTask<IActionResult> Put(SalesInfoModel model, CancellationToken cancellationToken)
        {
            if (!Request.Headers.TryGetValue("If-Match", out var etag) || string.IsNullOrWhiteSpace(etag))
            {
                return BadRequest("Missing If-Match header.");
            }

            var document = await _context
                .Set<SalesInfo>()
                .FirstOrDefaultAsync(x => x.Id == model.Id && x.Section == model.Section, cancellationToken);
            if (document is null)
            {
                return NotFound();
            }

            document.ProductId = model.ProductId;
            document.Description = model.Description;
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
