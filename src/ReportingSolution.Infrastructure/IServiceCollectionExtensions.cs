using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using ReportingSolution.Data.EventSourcingModels;
using ReportingSolution.Data.NoSqlModel;
using ReportingSolution.Infrastructure.Repositories;

namespace ReportingSolution.Infrastructure
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection serviceCollection)
        {
            if (serviceCollection.Any(x => x.ServiceType == typeof(CosmosDbContext)))
            {
                serviceCollection.TryAddScoped<IBankAccountRepository, BankAccountRepository>();
            }

            return serviceCollection;
        }
    }
}
