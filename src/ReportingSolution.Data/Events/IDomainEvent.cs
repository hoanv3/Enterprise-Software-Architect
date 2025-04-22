namespace ReportingSolution.Data.Events
{
    public interface IDomainEvent
    {
        Guid AggregateId { get; }
        DateTime OccurredOn { get; }
    }

    public record AccountOpened(Guid AggregateId, string OwnerName, DateTime OccurredOn) : IDomainEvent;
    public record MoneyDeposited(Guid AggregateId, decimal Amount, DateTime OccurredOn) : IDomainEvent;
    public record MoneyWithdrawn(Guid AggregateId, decimal Amount, DateTime OccurredOn) : IDomainEvent;
}
