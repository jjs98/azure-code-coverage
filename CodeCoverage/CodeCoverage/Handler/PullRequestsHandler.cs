using CodeCoverage.Interfaces.Handler;
using CodeCoverage.Interfaces.Services;
using Microsoft.TeamFoundation.SourceControl.WebApi;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CodeCoverage.Handler
{
    public class PullRequestsHandler : IPullRequestsHandler
    {
        private readonly IAzureService _azureService;
        public PullRequestsHandler(IAzureService azureService)
        {
            _azureService = azureService;
        }

        public async Task<GitPullRequest> Get(string organization, string project, string repositoryId, int pullRequestId)
        {
            return await _azureService.GetPullRequest(organization, project, repositoryId, pullRequestId);
        }

        public async Task<IEnumerable<GitPullRequest>> GetAll(string organization, string project, string repositoryId)
        {
            return await _azureService.GetPullRequests(organization, project, repositoryId);
        }

        public async Task<GitPullRequestStatus> PostStatus(string organization, string project, string respositoryId, int pullRequestId, GitPullRequestStatus pullRequestStatus)
        {
            return await _azureService.PostPullRequestStatus(organization, project, respositoryId, pullRequestId, pullRequestStatus);
        }

        public async Task<GitPullRequestCommentThread> PostCommentThread(string organization, string project, string respositoryId, int pullRequestId, GitPullRequestCommentThread pullRequestCommentThread)
        {
            return await _azureService.PostPullRequestCommentThread(organization, project, respositoryId, pullRequestId, pullRequestCommentThread);
        }

        public async Task<GitPullRequestCommentThread> PatchCommentThread(string organization, string project, string respositoryId, int pullRequestId, GitPullRequestCommentThread pullRequestCommentThread)
        {
            return await _azureService.PatchPullRequestCommentThread(organization, project, respositoryId, pullRequestId, pullRequestCommentThread);
        }
    }
}
