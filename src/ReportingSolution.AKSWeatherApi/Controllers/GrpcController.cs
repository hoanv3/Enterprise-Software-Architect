using Microsoft.AspNetCore.Mvc;
using ReportingSolution.AKSWeatherApi.Models;
using ReportingSolution.gRPC.SDK.Services;

namespace ReportingSolution.AKSWeatherApi.Controllers
{
  [ApiController]
  [Route("api/grpc")]
  public class GrpcController : ControllerBase
  {
    private readonly IBookGrpcService _bookGrpcService;

    public GrpcController(IBookGrpcService bookGrpcService)
    {
      _bookGrpcService = bookGrpcService;
    }

    [HttpPost("")]
    public async Task<IActionResult> CreateBook(BookCreationRequest request, CancellationToken cancellationToken)
    {
      var result = await _bookGrpcService.CreateBookAsync(new gRPC.SDK.Models.BookCreationRequest
      {
        Author = request.Author,
        Genre = request.Genre,
        Price = request.Price,
        Title = request.Title
      }, cancellationToken);

      return Ok(result);
    }

    [HttpGet("{bookId}")]
    public async Task<IActionResult> CreateBook(string bookId, CancellationToken cancellationToken)
    {
      var result = await _bookGrpcService.GetBookByIdAsync(bookId, cancellationToken);

      return Ok(new BookModel
      {
        Author = result.Author,
        Genre = result.Genre,
        Price= result.Price,
        Title = result.Title
      });
    }
  }
}
