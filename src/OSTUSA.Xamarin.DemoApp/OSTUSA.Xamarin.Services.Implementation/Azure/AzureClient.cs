using System;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;
namespace OSTUSA.XamarinDemo.Services.Azure
{
    public class AzureClient
    {
        private const string BaseUrl = "https://ostiotdemo.azurewebsites.net/api/";

        private readonly HttpClient _httpClient;

        public AzureClient()
        {
            _httpClient = new HttpClient() { BaseAddress = new Uri(BaseUrl) };
        }

        public async Task<T> GetAsync<T>(string method)
        {
            var response = await _httpClient.GetAsync(method);
            var responseBody = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
                return JsonConvert.DeserializeObject<T>(responseBody);

            throw new Exception(responseBody);
        }
    }
}
