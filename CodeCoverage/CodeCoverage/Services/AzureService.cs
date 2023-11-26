using CodeCoverage.Interfaces.Services;
using Microsoft.TeamFoundation.Build.WebApi;
using Microsoft.TeamFoundation.TestManagement.WebApi;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace CodeCoverage.Services
{
    public class AzureService : IAzureService
    {
        private readonly HttpClient _httpClient;

        public AzureService(string accessToken)
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

        public async Task<Build> GetLatestBuild(string organization, string project, string pipelineId, string branchName)
        {
            var url = $"https://dev.azure.com/{organization}/{project}/_apis/build/latest/{pipelineId}?branchName={branchName}&api-version=7.2-preview.1";

            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();
            var contentAsJson = await response.Content.ReadAsStringAsync();
            var build = JsonConvert.DeserializeObject<Build>(contentAsJson);
            return build;
        }

        public async Task<CodeCoverageSummary> GetCodeCoverage(string organization, string project, string buildId)
        {
            var url = $"https://vstmr.dev.azure.com/{organization}/{project}/_apis/testresults/codecoverage?buildId={buildId}&api-version=7.2-preview.1";

            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();
            var contentAsJson = await response.Content.ReadAsStringAsync();
            var coverage = JsonConvert.DeserializeObject<CodeCoverageSummary>(contentAsJson);
            return coverage;
        }
    }
}
