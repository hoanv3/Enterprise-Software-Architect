namespace ReportingSolution.Data.Configuration
{
    public class CosmosOptions
    {
        public required string EndPoint { get; set; }

        public required string AccountKey { get; set; }

        public required string DatabaseName { get; set; }
    }
}
