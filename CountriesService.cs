using Microsoft.Extensions.Options;
using System.Runtime;

public class CountriesService : ICountriesService
{
    private readonly HttpClient _httpClient;
    //private readonly HttpClientSettings _httpClientSettings;

    public CountriesService(HttpClient httpClient/*, IOptions<MySettings> settings*/)
    {
        _httpClient = httpClient;
        //_httpClientSettings = settings.Value;
    }

    //public async Task<string> DoSomethingAsync()
    //{
    //    httpClient.DefaultRequestHeaders.Add("Api-Key", _httpClientSettings.ApiKey);
    //}
}