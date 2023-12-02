using CodeCoverage.Interfaces.Services;
using CodeCoverage.Models;
using Microsoft.TeamFoundation.Build.WebApi;
using Microsoft.TeamFoundation.SourceControl.WebApi;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CodeCoverage.Services
{
    public class AzureService : IAzureService
    {
        private readonly IHttpService _httpService;

        public AzureService(IHttpService httpService)
        {
            _httpService = httpService;
        }

        public async Task<Build> GetLatestBuild(string organization, string project, string definitionId, string branchName)
        {
            var url = $"https://dev.azure.com/{organization}/{project}/_apis/build/latest/{definitionId}?branchName={branchName}&api-version=7.2-preview.1";

            return await _httpService.GetAsync<Build>(url);
        }

        public async Task<Models.CodeCoverageSummary> GetCodeCoverage(string organization, string project, string buildId)
        {
            var url = $"https://vstmr.dev.azure.com/{organization}/{project}/_apis/testresults/codecoverage?buildId={buildId}&api-version=7.2-preview.1";

            return await _httpService.GetAsync<Models.CodeCoverageSummary>(url);
        }

        public async Task<IEnumerable<GitPullRequest>> GetPullRequests(string organization, string project, string respositoryId)
        {
            var url = $"https://dev.azure.com/{organization}/{project}/_apis/git/repositories/{respositoryId}/pullrequests?api-version=7.2-preview.1";

            var response = await _httpService.GetAsync<ValueList<GitPullRequest>>(url);
            return response.Value;
        }

        public async Task<GitPullRequest> GetPullRequest(string organization, string project, string respositoryId, int pullRequestId)
        {
            var url = $"https://dev.azure.com/{organization}/{project}/_apis/git/repositories/{respositoryId}/pullrequests/{pullRequestId}?api-version=7.2-preview.1";

            return await _httpService.GetAsync<GitPullRequest>(url);
        }

        public async Task<GitPullRequestStatus> PostPullRequestStatus(string organization, string project, string respositoryId, int pullRequestId, GitPullRequestStatus pullRequestStatus)
        {
            var url = $"https://dev.azure.com/{organization}/{project}/_apis/git/repositories/{respositoryId}/pullrequests/{pullRequestId}/statuses?api-version=7.2-preview.1";

            return await _httpService.PostAsync(url, pullRequestStatus);
        }

        public async Task<GitPullRequestCommentThread> PostPullRequestCommentThread(string organization, string project, string respositoryId, int pullRequestId, GitPullRequestCommentThread pullRequestCommentThread)
        {
            var url = $"https://dev.azure.com/{organization}/{project}/_apis/git/repositories/{respositoryId}/pullrequests/{pullRequestId}/threads?api-version=7.2-preview.1";

            return await _httpService.PostAsync(url, pullRequestCommentThread);
        }

        public async Task<GitPullRequestCommentThread> PatchPullRequestCommentThread(string organization, string project, string respositoryId, int pullRequestId, GitPullRequestCommentThread pullRequestCommentThread)
        {
            var threadId = pullRequestCommentThread.Id;
            var url = $"https://dev.azure.com/{organization}/{project}/_apis/git/repositories/{respositoryId}/pullrequests/{pullRequestId}/threads/{threadId}?api-version=7.2-preview.1";

            return await _httpService.PatchAsync(url, pullRequestCommentThread);
        }
    }
}
