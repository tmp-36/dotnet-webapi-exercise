using System.Text.Json.Serialization;

namespace CountriesAPI.Models
{
    public class Meta
    {
        public int Total { get; set; }
        public int Count { get; set; }
        public int Limit { get; set; }
        public int Offset { get; set; }
        public bool More { get; set; }
        [JsonPropertyName("request_id")] public string RequestId { get; set; }
    }
}
