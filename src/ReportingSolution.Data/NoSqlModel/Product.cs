namespace ReportingSolution.Data.NoSqlModel
{
    public class Product : BaseDocument
    {
        public string ProductName { get; set; } = null!;
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }
}
