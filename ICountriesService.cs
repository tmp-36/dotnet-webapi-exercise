internal interface ICountriesService
{
    public Task<List<T>> GetAllData<T>(string request);
}