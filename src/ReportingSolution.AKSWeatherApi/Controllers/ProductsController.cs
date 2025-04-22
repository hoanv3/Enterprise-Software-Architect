using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos;
using Microsoft.EntityFrameworkCore;
using ReportingSolution.AKSWeatherApi.Models;
using ReportingSolution.Data.NoSqlModel;

namespace ReportingSolution.AKSWeatherApi.Controllers
{
    [ApiController]
    [Route("api/products")]
    public class ProductsController : ControllerBase
    {
        private readonly CosmosDbContext _context;

        public ProductsController(CosmosDbContext context)
        {
            _context = context;
        }

        [HttpGet("{section}/{id}")]
        public async ValueTask<IActionResult> GetById(string id, string section, CancellationToken cancellationToken)
        {
            var entity = await _context
                .Set<Product>()
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
        public async ValueTask<IActionResult> Post(ProductModel model, CancellationToken cancellationToken)
        {
            var document = new Product
            {
                ProductName = model.ProductName,
                Price = model.Price,
                Quantity = model.Quantity,
                Section = model.Section,
            };
            _context.Set<Product>().Add(document);

            await _context.SaveChangesAsync(cancellationToken);
            return Ok(document);
        }

        [HttpPut("")]
        //[AutoValidateAntiforgeryToken]
        public async ValueTask<IActionResult> Put(ProductModel model, CancellationToken cancellationToken)
        {
            if (!Request.Headers.TryGetValue("If-Match", out var etag) || string.IsNullOrWhiteSpace(etag))
            {
                return BadRequest("Missing If-Match header.");
            }

            var document = await _context
                .Set<Product>()
                .FirstOrDefaultAsync(x => x.Id == model.Id && x.Section == model.Section, cancellationToken);
            if (document is null)
            {
                return NotFound();
            }

            document.ProductName = model.ProductName;
            document.Price = model.Price;
            document.Quantity = model.Quantity;
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
