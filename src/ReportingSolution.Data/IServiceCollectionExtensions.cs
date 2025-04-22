using Ardalis.GuardClauses;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ReportingSolution.Data.NoSqlModel;

namespace ReportingSolution.Data
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection AddData(this IServiceCollection serviceCollection, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("StevenDB");
            Guard.Against.NullOrWhiteSpace(connectionString);
            serviceCollection.AddDbContext<StevenContext>(opt => opt.UseSqlServer(connectionString));

            serviceCollection.AddDbContext<CosmosDbContext>(opt =>
            {
                opt.UseCosmos(
                    accountEndpoint: configuration["CosmosDB:EndPoint"]!,
                    accountKey: configuration["CosmosDB:AccountKey"]!,
                    databaseName: configuration["CosmosDB:DatabaseName"]!);
            });

            return serviceCollection;
        }
    }
}
