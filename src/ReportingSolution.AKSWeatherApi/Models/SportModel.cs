using System.ComponentModel.DataAnnotations;

namespace ReportingSolution.AKSWeatherApi.Models
{
    public class SportModel
    {
        public string Id { get; set; } = null!;

        [Required]
        public required string SportName { get; set; }

        [Required]
        public required string Section { get; set; }
    }
}
