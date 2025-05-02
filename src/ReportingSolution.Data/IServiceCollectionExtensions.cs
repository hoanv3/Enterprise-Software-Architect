using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ReportingSolution.Data.BlazorServerModels;
using ReportingSolution.Data.NoSqlModel;

namespace ReportingSolution.Data
{
  public static class IServiceCollectionExtensions
  {
    public static IServiceCollection AddData(this IServiceCollection serviceCollection, IConfiguration configuration)
    {
      var stevenDBConnectionString = configuration.GetConnectionString("StevenDB");
      if (!string.IsNullOrWhiteSpace(stevenDBConnectionString))
      {
        serviceCollection.AddDbContext<StevenContext>(opt => opt.UseSqlServer(stevenDBConnectionString), ServiceLifetime.Scoped);
      }

      var cosmosSection = configuration.GetSection("CosmosDB");
      if (cosmosSection.Exists())
      {
        serviceCollection.AddDbContext<CosmosDbContext>(opt =>
          {
            opt.UseCosmos(
                      accountEndpoint: configuration["CosmosDB:EndPoint"]!,
                      accountKey: configuration["CosmosDB:AccountKey"]!,
                      databaseName: configuration["CosmosDB:DatabaseName"]!);
          }); 
      }

      var blazorServerConnectionString = configuration.GetConnectionString("BlazorServer");
      if (!string.IsNullOrWhiteSpace(blazorServerConnectionString))
      {
        serviceCollection.AddDbContext<BlazorServerContext>(opt => opt.UseSqlServer(blazorServerConnectionString), ServiceLifetime.Scoped);
      }

      return serviceCollection;
    }
  }
}
