using System.Text.Json.Serialization;

namespace ReportingSolution.Data.NoSqlModel
{
    public class BaseDocument
    {
        public string Id { get; set; } = null!;

        public string Section { get; set; } = null!;

        [JsonPropertyName("_etag")]
        public string Etag { get; set; } = null!;
    }
}
