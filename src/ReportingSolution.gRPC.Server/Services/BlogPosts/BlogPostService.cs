using Grpc.Core;
using MediatR;
using ReportingSolution.gRPC.Server.Protos;
using ReportingSolution.gRPC.Server.Services.BlogPosts.Commands;

namespace ReportingSolution.gRPC.Server.Services
{
  public class BlogPostService : BlogPost.BlogPostBase
  {
    private readonly IMediator _mediator;

    public BlogPostService(IMediator mediator)
    {
      _mediator = mediator;
    }

    public override async Task<CreateBlogPostResponse> CreateBlogPost(CreateBlogPostRequest request, ServerCallContext context)
    {
      var command = new CreateBlogPostCommand
      {
        Title = request.Title,
        Category = request.Category,
        Author = request.Author,
        Content = request.Content
      };
      var result = await _mediator.Send(command, context.CancellationToken);

      return result;
    }
  }
}
