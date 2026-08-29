using System.Net.Http.Json;

namespace CountriesAPI
{
    public class Client
    {
        private readonly HttpClient httpClient;

        public Client(HttpClient httpClient)
        {
            this.httpClient = httpClient;
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
            var response = await httpClient.GetAsync($"{request}&offset={offset}");

            // NOTE(Sergei): I want to make use of the messages the API returns but there's so many things that could be null here.
            // I feel like I'm doing it wrong, want to talk to someone with experience regarding how to handle this.
            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadFromJsonAsync<ErrorResponse>();
                var message = string.Join(Environment.NewLine, error.Errors.Select(x => x.Message));
                throw new HttpRequestException(message, null, response.StatusCode);
            }

            return await response.Content.ReadFromJsonAsync<Response<T>>();
        }
    }
}
