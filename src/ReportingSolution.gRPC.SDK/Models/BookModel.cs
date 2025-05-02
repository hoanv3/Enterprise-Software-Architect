namespace ReportingSolution.gRPC.SDK.Models
{
  public class BookModel
  {
    public required string Author { get; set; }

    public required string Genre { get; set; }

    public required string Title { get; set; }

    public required double Price { get; set; }
  }
}
