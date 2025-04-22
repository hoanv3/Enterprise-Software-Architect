using Ardalis.GuardClauses;
using ReportingSolution.Data.Events;

namespace ReportingSolution.Data.EventSourcingModels
{
    public class BankAccount
    {
        private readonly List<IDomainEvent> _stateChanges = [];

        public Guid Id { get; private set; }
        public string OwnerName { get; private set; } = null!;
        public decimal Balance { get; private set; }

        public IEnumerable<IDomainEvent> GetUncommittedStateChanges() => _stateChanges;

        public void RebuildAggregate(IEnumerable<IDomainEvent> domainEvents)
        {
            foreach (var domainEvent in domainEvents) 
            {
                Apply(domainEvent);
            }
        }

        public void Apply(IDomainEvent domainEvent)
        {
            switch (domainEvent)
            {
                case AccountOpened accountOpened:
                    Id = accountOpened.AggregateId;
                    OwnerName = accountOpened.OwnerName;
                    break;
                case MoneyDeposited moneyDeposited:
                    Balance += moneyDeposited.Amount;
                    break;
                case MoneyWithdrawn moneyWithdrawn:
                    Balance -= moneyWithdrawn.Amount;
                    break;
                default:
                    break;
            }
        }

        public static BankAccount OpenBankAccount(Guid id, string owner)
        {
            var bankAccount = new BankAccount();
            var accountOpened = new AccountOpened(id, owner, DateTime.UtcNow);
            bankAccount.Apply(accountOpened);
            bankAccount._stateChanges.Add(accountOpened);

            return bankAccount;
        }

        public void Deposit(decimal amount)
        {
            Guard.Against.NegativeOrZero(amount);
            var moneyDeposited = new MoneyDeposited(Id, Balance, DateTime.UtcNow);
            Apply(moneyDeposited);
            _stateChanges.Add(moneyDeposited);
        }

        public void Withdraw(decimal amount)
        {
            Guard.Against.NegativeOrZero(amount);
            var moneyWithdrawn = new MoneyWithdrawn(Id, Balance, DateTime.UtcNow);
            Apply(moneyWithdrawn);
            _stateChanges.Add(moneyWithdrawn);
        }
    }
}
