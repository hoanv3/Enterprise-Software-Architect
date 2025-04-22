namespace ReportingSolution.Data.NoSqlModel
{
    public class BankingEventData : BaseDocument
    {
        public Guid AggregateId { get; set; }
        public string EventType { get; set; } = string.Empty;
        public string Data { get; set; } = string.Empty;
        public DateTime OccurredOn { get; set; }
    }
}
