using Grpc.Core;
using ReportingSolution.gRPC.SDK.Models;
using ReportingSolution.gRPC.Server.Protos;

namespace ReportingSolution.gRPC.SDK.Services
{
  internal sealed class BookGrpcService : IBookGrpcService
  {
    private readonly Book.BookClient _bookClient;

    public BookGrpcService(Book.BookClient bookClient)
    {
      _bookClient = bookClient;
    }

    public async ValueTask<BookModel> GetBookByIdAsync(string bookId, CancellationToken cancellationToken)
    {
      try
      {
        var response = await _bookClient.GetBookByIdAsync(new GetBookByIdRequest { Id = bookId }, cancellationToken: cancellationToken);

        return new BookModel
        {
          Title = response.Title,
          Author = response.Author,
          Genre = response.Genre,
          Price = response.Price
        };
      }
      catch (RpcException)
      {
        throw;
      }
    }

    public async ValueTask<BookCreationResponse> CreateBookAsync(BookCreationRequest request, CancellationToken cancellationToken)
    {
      var bookCreationModel = new CreateBookRequest
      {
        Author = request.Author,
        Genre = request.Genre,
        Price = request.Price,
        Title = request.Title
      };
      try
      {
        var response = await _bookClient.CreateBookAsync(bookCreationModel, cancellationToken: cancellationToken);
        return new BookCreationResponse { Id = response.Id };
      }
      catch (RpcException)
      {
        throw;
      }
    }
  }
}
