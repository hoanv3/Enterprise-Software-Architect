namespace ReportingSolution.AKSWeatherApi.Models
{
  public class BookCreationRequest
  {
    public required string Author { get; set; }

    public required string Genre { get; set; }

    public required string Title { get; set; }

    public required double Price { get; set; }
  }
}
