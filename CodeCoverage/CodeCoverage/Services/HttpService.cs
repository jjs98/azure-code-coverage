using CodeCoverage.Interfaces.Services;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace CodeCoverage.Services
{
    public class HttpService : IHttpService
    {
        private readonly HttpClient _httpClient;

        public HttpService(string accessToken)
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic",
                Convert.ToBase64String(
                    Encoding.ASCII.GetBytes(
                        string.Format("{0}:{1}", "", accessToken))));
            _httpClient = client;
        }

        public async Task<T> GetAsync<T>(string url) where T : class
        {
            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();
            var responseContentAsJson = await response.Content.ReadAsStringAsync();
            var responseContent = JsonConvert.DeserializeObject<T>(responseContentAsJson);
            return responseContent;
        }

        public async Task<T> PostAsync<T>(string url, T body) where T : class
        {
            var bodyAsJson = JsonConvert.SerializeObject(body);
            var content = new StringContent(bodyAsJson, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(url, content);
            response.EnsureSuccessStatusCode();
            var responseContentAsJson = await response.Content.ReadAsStringAsync();
            var responseContent = JsonConvert.DeserializeObject<T>(responseContentAsJson);
            return responseContent;
        }

        public async Task<T> PatchAsync<T>(string url, T body) where T : class
        {
            var bodyAsJson = JsonConvert.SerializeObject(body);
            var content = new StringContent(bodyAsJson, Encoding.UTF8, "application/json");
            var response = await _httpClient.PatchAsync(url, content);
            response.EnsureSuccessStatusCode();
            var responseContentAsJson = await response.Content.ReadAsStringAsync();
            var responseContent = JsonConvert.DeserializeObject<T>(responseContentAsJson);
            return responseContent;
        }
    }
}
