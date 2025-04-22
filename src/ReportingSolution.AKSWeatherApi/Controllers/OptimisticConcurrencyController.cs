using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ReportingSolution.AKSWeatherApi.Models;
using ReportingSolution.Data.NoSqlModel;

namespace ReportingSolution.AKSWeatherApi.Controllers
{
    [ApiController]
    [Route("api/optimistic")]
    [AutoValidateAntiforgeryToken]
    public class OptimisticConcurrencyController : ControllerBase
    {
        private readonly StevenContext _context;

        public OptimisticConcurrencyController(StevenContext context)
        {
            _context = context;
        }

        [HttpGet("{id:int}")]
        public async ValueTask<IActionResult> GetById(int id, CancellationToken cancellationToken)
        {
            var entity = await _context
                .Set<Users>()
                .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
            if (entity == null)
            {
                return NotFound();
            }

            return Ok(entity);
        }

        [HttpPost("")]
        //[AutoValidateAntiforgeryToken]
        public async ValueTask<IActionResult> Post(UserInfo userInfo, CancellationToken cancellationToken)
        {
            var entity = new Users
            {
                UserName = userInfo.UserName,
            };

            _context.Set<Users>().Add(entity);
            await _context.SaveChangesAsync(cancellationToken);

            return Ok(entity);
        }

        [HttpPut("")]
        //[AutoValidateAntiforgeryToken]
        public async ValueTask<IActionResult> Put(UserInfo userInfo, CancellationToken cancellationToken)
        {
            if (!Request.Headers.TryGetValue("If-Match", out var etag))
            {
                return BadRequest("Missing If-Match header");
            }

            var entity = await _context
                .Set<Users>()
                .FirstOrDefaultAsync(x => x.Id == userInfo.Id, cancellationToken);
            if (entity == null)
            {
                return NotFound();
            }

            entity.UserName = userInfo.UserName;
            _context.Entry(entity).OriginalValues["LastModified"] = etag;

            try
            {
                await _context.SaveChangesAsync(cancellationToken);
                return Ok(entity);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
