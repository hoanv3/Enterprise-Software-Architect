using Grpc.Core;
using ReportingSolution.gRPC.Server.Protos;

namespace ReportingSolution.gRPC.Server.Services
{
    public class BlogPostService : BlogPost.BlogPostBase
    {
        public override Task<CreateBlogPostResponse> CreateBlogPost(CreateBlogPostRequest request, ServerCallContext context)
        {
            return base.CreateBlogPost(request, context);
        }
    }
}
