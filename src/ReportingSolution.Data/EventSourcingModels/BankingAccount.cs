using ReportingSolution.Data.Events;

namespace ReportingSolution.Data.EventSourcingModels
{
    public class BankingAccount
    {
        private readonly List<IDomainEvent> _stateChanges = [];

        public Guid Id { get; private set; }
        public string OwnerName { get; private set; } = null!;
        public decimal Balance { get; private set; }

        public IEnumerable<IDomainEvent> GetUncommittedStateChanges() => _stateChanges;

        public void Apply(IDomainEvent domainEvent)
        {
            switch (domainEvent)
            {
                case AccountOpened accountOpened:
                    Id = accountOpened.AggregateId;
                    OwnerName = accountOpened.OwnerName;
                    _stateChanges.Add(accountOpened);
                    break;
                default:
                    break;
            }
        }
    }
}
