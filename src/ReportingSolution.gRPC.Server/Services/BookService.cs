using Grpc.Core;
using Microsoft.EntityFrameworkCore;
using ReportingSolution.Data.BlazorServerModels;
using ReportingSolution.gRPC.Server.Protos;

namespace ReportingSolution.gRPC.Server.Services
{
  public class BookService : Book.BookBase
  {
    private readonly BlazorServerContext _context;
    private readonly TimeProvider _timerProvider;

    public BookService(BlazorServerContext context, TimeProvider timerProvider)
    {
      _context = context;
      _timerProvider = timerProvider;
    }

    public override async Task<CreateBookResponse> CreateBook(CreateBookRequest request, ServerCallContext context)
    {
      var book = new Books
      {
        Author = request.Author,
        Genre = request.Genre,
        Price = (float?)request.Price,
        Title = request.Title,
        PubDate = _timerProvider.GetUtcNow().DateTime,
        Id = Guid.NewGuid().ToString()
        // ETag to ensure we're not overwriting a state change made by others is outstanding
      };

      _context.Set<Books>().Add(book);
      //await _context.SaveChangesAsync(context.CancellationToken);

      return new CreateBookResponse
      {
        Id = book.Id
      };
    }

    public override async Task<GetBookByIdResponse> GetBookById(GetBookByIdRequest request, ServerCallContext context)
    {
      var entity = await _context
        .Set<Books>()
        .AsNoTracking()
        .FirstOrDefaultAsync(x => x.Id == request.Id, context.CancellationToken);
      if (entity is null)
      {
        throw new RpcException(new Status(StatusCode.NotFound, "Book not found"));
      }

      return new GetBookByIdResponse
      {
        Author = entity.Author,
        Genre = entity.Genre,
        //Price = (double?)entity.Price,
        Title = entity.Title
      };
    }
  }
}
