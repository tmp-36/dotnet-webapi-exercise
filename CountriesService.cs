using CountriesAPI.Models;
using System.Net.Http.Json;

public class CountriesService : ICountriesService
{
    private readonly HttpClient _httpClient;

    public CountriesService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<T>> GetAllData<T>(string request)
    {
        var items = new List<T>();
        int offset = 0;

        while (true)
        {
            var page = await GetPage<T>(request, offset);

            // NOTE(Sergei): Preallocate as soon as we know total count
            items.EnsureCapacity(page.Data.Meta.Total);
            items.AddRange(page.Data.Objects);

            if (!page.Data.Meta.More)
            {
                return items;
            }

            offset += page.Data.Meta.Count;
        }
    }

    public async Task<Response<T>> GetPage<T>(string request, int offset)
    {
        var response = await _httpClient.GetAsync($"{request}&offset={offset}");
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<Response<T>>();
    }
}