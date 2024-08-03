using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace App.Services
{
    public class GoogleCSEService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly GoogleCSESettings _settings;

        public GoogleCSEService(IHttpClientFactory httpClientFactory, IOptions<GoogleCSESettings> settings)
        {
            _httpClientFactory = httpClientFactory;
            _settings = settings.Value;
        }

        public async Task<List<GoogleCSEItem>> SearchAsync(string query)
        {
            var client = _httpClientFactory.CreateClient();
            query = string.IsNullOrWhiteSpace(_settings.Site) ? query : $"site:{_settings.Site}+{query}";
            var requestUri = $"https://www.googleapis.com/customsearch/v1?q={query}&key={_settings.ApiKey}&cx={_settings.SearchEngineId}";
            var response = await client.GetAsync(requestUri);

            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadAsStringAsync();
            var searchResult = JsonConvert.DeserializeObject<SearchResult>(result);

            return searchResult.Items;
        }
    }
    public class GoogleCSESettings
    {
        public string? ApiKey { get; set; }
        public string? SearchEngineId { get; set; }
        public string? Site { get; set; }
    }
    public class SearchResult
    {
        public List<GoogleCSEItem> Items { get; set; }
    }

    public class GoogleCSEItem
    {
        public string Title { get; set; }
        public string Link { get; set; }
        public string Snippet { get; set; }
    }

}
