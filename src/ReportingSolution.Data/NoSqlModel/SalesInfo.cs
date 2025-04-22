namespace ReportingSolution.Data.NoSqlModel
{
    public class SalesInfo : BaseDocument
    {
        public string ProductId { get; set; } = null!;

        public string? Description { get; set; }
    }
}
