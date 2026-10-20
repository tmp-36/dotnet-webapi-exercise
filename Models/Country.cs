using System.Text.Json.Serialization;

namespace CountriesAPI.Models
{
    public class Country
    {
        public Names Names { get; set; }
        public List<Currency> Currencies { get; set; }
        public int Population { get; set; }
        [JsonPropertyName("_meta")] public CountryMeta Meta { get; set; }
    }
}
