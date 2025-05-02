using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using ReportingSolution.gRPC.SDK.Services;
using ReportingSolution.gRPC.Server.Protos;

namespace ReportingSolution.gRPC.SDK
{
  public static class IServiceCollectionExtensions
  {
    public static IServiceCollection AddGrpcSdk(this IServiceCollection services, IConfiguration configuration)
    {
      var uri = configuration["grpcUri"];
      if (!string.IsNullOrWhiteSpace(uri))
      {
        services.AddGrpcClient<Book.BookClient>(options =>
          {
            options.Address = new Uri(uri);
          });
        services.TryAddScoped<IBookGrpcService, BookGrpcService>();
      }
      return services;
    }
  }
}
