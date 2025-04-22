using System.ComponentModel.DataAnnotations;

namespace ReportingSolution.AKSWeatherApi.Models
{
    public class SalesInfoModel
    {
        public string? Id { get; set; }

        public required string ProductId { get; set; }

        public string? Description { get; set; }

        [Required]
        public required string Section { get; set; }
    }
}
