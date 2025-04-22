using System.ComponentModel.DataAnnotations;

namespace ReportingSolution.AKSWeatherApi.Models
{
    public class UserInfo
    {
        public int? Id { get; set; }

        [Required, StringLength(100)]
        public required string UserName { get; set; }
    }
}
