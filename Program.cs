using System.Text.Json;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;


var builder = Host.CreateApplicationBuilder(args);

//builder.Services.AddHttpClient("countries", httpClient =>
//{
//    httpClient.BaseAddress = new Uri("https://api.restcountries.com/countries/v5");
//    httpClient.DefaultRequestHeaders.TryAddWithoutValidation("APIKEY", builder.Configuration["HttpClient:APIKey"]);

//};

builder.Services.AddHttpClient<CountriesAPI.Client>(httpClient =>
{
    var apiKey = Environment.GetEnvironmentVariable("REST_COUNTRIES_API_KEY");

    if (!string.IsNullOrEmpty(apiKey))
    {
        httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");
    }
});
builder.Services.AddLogging(configure => configure.AddFilter("System.Net.Http.HttpClient.Default", LogLevel.Warning));
builder.Services.AddScoped<ICountriesService, CountriesService>();

IHost host = builder.Build();

await using (var scope = host.Services.CreateAsyncScope())
{

}

    var requestBuilder = new CountriesAPI.RequestBuilder();
requestBuilder.AddField("names.common");
requestBuilder.AddField("population");
requestBuilder.AddField("currencies");
requestBuilder.SetLimit(100);
string request = requestBuilder.Build();

try
{
    var client = host.Services.GetRequiredService<CountriesAPI.Client>();
    var countries = await client.GetAllData<CountriesAPI.Country>(request);

    Task1(countries, 5, "N0");
    Task2(countries, 5);
}
catch (HttpRequestException e)
{
    Console.Error.WriteLine(e.Message);
}
catch (JsonException e)
{
    Console.Error.WriteLine(e.Message);
}

host.Dispose();

static void Task1(List<CountriesAPI.Country> countries, int count, string format)
{
    // NOTE(Sergei): Quick O(n log n) solution. Can be replaced with a more efficient O(5n) solution using an array for top 5.
    var result = countries
        .OrderByDescending(x => x.Population)
        .Take(count);

    foreach (var country in result)
    {
        Console.WriteLine($"{country.Names.Common} - {country.Population.ToString(format)}");
    }
}

static void Task2(List<CountriesAPI.Country> countries, int count)
{
    var currencies = new Dictionary<string, int>();

    foreach (var country in countries)
    {
        foreach (var currency in country.Currencies)
        {
            currencies[currency.Code] = currencies.GetValueOrDefault(currency.Code) + 1;
        }
    }

    // NOTE(Sergei): Same as task 1, this can be O(5n).
    var result = currencies
        .OrderByDescending(x => x.Value)
        .Take(count);

    foreach (var currency in result)
    {
        Console.WriteLine($"{currency.Key} - {currency.Value} countries");
    }
}
