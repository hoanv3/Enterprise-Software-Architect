using ReportingSolution.gRPC.SDK.Models;

namespace ReportingSolution.gRPC.SDK.Services
{
  public interface IBookGrpcService
  {
    ValueTask<BookModel> GetBookByIdAsync(string bookId, CancellationToken cancellationToken);

    ValueTask<BookCreationResponse> CreateBookAsync(BookCreationRequest request, CancellationToken cancellationToken);
  }
}
