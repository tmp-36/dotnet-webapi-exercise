namespace CountriesAPI
{
    public class RequestBuilder
    {
        private string baseUrl = "https://api.restcountries.com/countries/v5";
        private List<string> fields = new();
        private int limit = 25;

        public void AddField(string field)
        {
            fields.Add(field);
        }

        public void SetLimit(int limit)
        {
            this.limit = limit;
        }

        public string Build()
        {
            var parameters = new List<string>();

            if (fields.Count > 0)
            {
                parameters.Add($"response_fields={string.Join(",", fields)}");
            }

            parameters.Add($"limit={limit}");

            return $"{baseUrl}?{string.Join("&", parameters)}";
        }
    }
}
