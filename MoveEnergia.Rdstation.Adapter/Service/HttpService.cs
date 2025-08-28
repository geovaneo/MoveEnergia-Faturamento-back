using Microsoft.Extensions.Logging;
using MoveEnergia.Rdstation.Adapter.Interface.Service;
using Newtonsoft.Json;
using System.Text;

namespace MoveEnergia.Rdstation.Adapter.Service
{
    public class HttpService : IHttpService
    {
        private readonly ILogger<HttpService> _logger;
        private readonly IHttpClientFactory _httpClientFactory;

        public HttpService(ILogger<HttpService> logger, IHttpClientFactory httpClientFactory)
        {
            _logger = logger;
            _httpClientFactory = httpClientFactory;
        }

        private HttpClient CreateClient() => _httpClientFactory.CreateClient();

        public async Task<T?> GetAsync<T>(string url)
        {
            var client = CreateClient();
            var response = await client.GetAsync(url);

            if (!response.IsSuccessStatusCode)
                return default;

            var content = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<T>(content);
        }

        public async Task<TResponse?> PostAsync<TRequest, TResponse>(string url, TRequest data)
        {
            var client = CreateClient();

            var json = JsonConvert.SerializeObject(data);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync(url, content);

            if (!response.IsSuccessStatusCode)
                return default;

            var result = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<TResponse>(result);
        }

        public async Task<TResponse?> PutAsync<TRequest, TResponse>(string url, TRequest data)
        {
            var client = CreateClient();

            var json = JsonConvert.SerializeObject(data);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PutAsync(url, content);

            if (!response.IsSuccessStatusCode)
                return default;

            var result = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<TResponse>(result);
        }

        public async Task<bool> DeleteAsync(string url)
        {
            var client = CreateClient();
            var response = await client.DeleteAsync(url);
            return response.IsSuccessStatusCode;
        }
    }
}
