using MediatR;
using ReportingSolution.gRPC.Server.Protos;

namespace ReportingSolution.gRPC.Server.Services.BlogPosts.Commands
{
  public class CreateBlogPostCommand : IRequest<CreateBlogPostResponse>
  {
    public required string Title { get; set; }

    public required string Category { get; set; }

    public required string Author { get; set; }

    public required string Content { get; set; }
  }
}
