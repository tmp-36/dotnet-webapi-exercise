using CountriesAPI;
using CountriesAPI.Configuration;
using CountriesAPI.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Net;
using System.Text.Json;



var builder = Host.CreateApplicationBuilder(args);

Console.WriteLine(
    $"Environment: {builder.Environment.EnvironmentName}");

Console.WriteLine(
    $"Secrets ID: {builder.Environment
        .ApplicationName}");

builder.Services.Configure<CountriesOptions>(builder.Configuration.GetSection(CountriesOptions.SectionName));

builder.Services.AddHttpClient<ICountriesService, CountriesService>((sp, client) =>
{
    var options = sp.GetRequiredService<IOptions<CountriesOptions>>().Value;
    client.BaseAddress = new Uri(options.BaseUrl);
    client.DefaultRequestHeaders.Add(AuthConstants.ApiKeyHeaderName, $"Bearer {options.ApiKey}");

});

IHost host = builder.Build();

await using (var scope = host.Services.CreateAsyncScope())
{
    var serviceProvider = scope.ServiceProvider;
    var countriesService = serviceProvider.GetRequiredService<ICountriesService>();

    var requestBuilder = new RequestBuilder();
    requestBuilder.AddField("names.common");
    requestBuilder.AddField("population");
    requestBuilder.AddField("currencies");
    requestBuilder.SetLimit(100);
    string request = requestBuilder.Build();

    var countries = await countriesService.GetAllData<Country>(request);

    Task1(countries, 5, "N0");
    Task2(countries, 5);
}

host.Dispose();

static void Task1(List<Country> countries, int count, string format)
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

static void Task2(List<Country> countries, int count)
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
