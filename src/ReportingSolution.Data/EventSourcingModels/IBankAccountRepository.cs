namespace ReportingSolution.Data.EventSourcingModels
{
    public interface IBankAccountRepository
    {
        ValueTask<BankAccount> GetByIdAsync(Guid aggregateId, CancellationToken cancellationToken);

        ValueTask SaveAsync(BankAccount bankAccount, CancellationToken cancellationToken);
    }
}
