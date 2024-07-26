
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace App.Services
{
    using Newtonsoft.Json;
    using System;
    using System.Text.Json;

    public class HttpService
    {
        public static string BaseUrl { get; set; } = "https://localhost:7161/";
        private HttpClient _httpClient;

        public HttpService()
        {
            
        }

        public async Task<string> GetAsync(string url)
        {
            _httpClient = new HttpClient();
            HttpResponseMessage response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }

        public async Task<string> PostAsync<T>(string endPoint, T model)
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri(BaseUrl);
            string jsonData = JsonConvert.SerializeObject(model); // JsonSerializer.Serialize(model);
            
            HttpContent content = new StringContent(jsonData, Encoding.UTF8, "application/json");

            using (var response = await _httpClient.PostAsync(endPoint, content))
            {
                response.EnsureSuccessStatusCode();
               // throw new Exception();
                return await response.Content.ReadAsStringAsync();
            }
        }   
    }

}