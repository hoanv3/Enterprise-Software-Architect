using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using ReportingSolution.Data.Events;
using ReportingSolution.Data.EventSourcingModels;
using ReportingSolution.Data.NoSqlModel;

namespace ReportingSolution.Infrastructure.Repositories
{
    internal sealed class BankAccountRepository : IBankAccountRepository
    {
        private readonly CosmosDbContext _cosmosDbContext;

        public BankAccountRepository(CosmosDbContext cosmosDbContext)
        {
            _cosmosDbContext = cosmosDbContext;
        }

        public async ValueTask<BankAccount> GetByIdAsync(Guid aggregateId, CancellationToken cancellationToken)
        {
            var events = await _cosmosDbContext
                .Set<BankingEventData>()
                .Where(x => x.AggregateId == aggregateId)
                .ToListAsync(cancellationToken);

            // Need to apply source generator
            var domainEvents = events
                .Select(x => JsonConvert.DeserializeObject(x.Data, Type.GetType(x.EventType)!) as IDomainEvent)
                .Where(x => x is not null)
                .ToList();

            var aggregate = new BankAccount();
            aggregate.RebuildAggregate(domainEvents!);
            return aggregate;
        }

        public async ValueTask SaveAsync(BankAccount bankAccount, CancellationToken cancellationToken)
        {
            var domainEvents = bankAccount.GetUncommittedStateChanges();
            var entities = domainEvents.Select(x => new BankingEventData
            {
                AggregateId = x.AggregateId,
                Data = JsonConvert.SerializeObject(x),
                EventType = x.GetType().AssemblyQualifiedName!,
                OccurredOn = x.OccurredOn,
                Section = "banking"
            });
            await _cosmosDbContext
                .Set<BankingEventData>()
                .AddRangeAsync(entities, cancellationToken);

            await _cosmosDbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
