using System.ComponentModel.DataAnnotations;

namespace ReportingSolution.AKSWeatherApi.Models
{
    public class ProductModel
    {
        public string? Id { get; set; }

        [Required, StringLength(100)]
        public required string ProductName { get; set; }

        public decimal Price { get; set; }

        [Range(0, int.MaxValue)]
        public int Quantity { get; set; }

        [Required]
        public required string Section { get; set; }
    }
}
