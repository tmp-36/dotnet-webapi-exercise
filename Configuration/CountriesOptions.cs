namespace CountriesAPI.Configuration;

public class CountriesOptions
{
    public const string SectionName = "Countries";
    public string BaseUrl { get; set; } = string.Empty;

    public string? ApiKey { get; set; }

    public string MinimumLevel { get; set; } = "Warning";
}
