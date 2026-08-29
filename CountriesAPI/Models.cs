using System.Text.Json.Serialization;

namespace CountriesAPI
{
    public class ErrorResponse
    {
        public List<Error> Errors { get; set; }
    }

    public class Error
    {
        public string Message { get; set; }
    }

    public class Response<T>
    {
        public Data<T> Data { get; set; }
    }

    public class Data<T>
    {
        public List<T> Objects { get; set; }
        public Meta Meta { get; set; }
    }

    public class Meta
    {
        public int Total { get; set; }
        public int Count { get; set; }
        public int Limit { get; set; }
        public int Offset { get; set; }
        public bool More { get; set; }
        [JsonPropertyName("request_id")] public string RequestId { get; set; }
    }

    public class Country
    {
        public Names Names { get; set; }
        public List<Currency> Currencies { get; set; }
        public int Population { get; set; }
        [JsonPropertyName("_meta")] public CountryMeta Meta { get; set; }
    }

    public class Names
    {
        public string Common { get; set; }
    }

    public class Currency
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string Symbol { get; set; }
    }

    public class CountryMeta
    {
        public long LastUpdatedTimestamp { get; set; }
    }

    
}
