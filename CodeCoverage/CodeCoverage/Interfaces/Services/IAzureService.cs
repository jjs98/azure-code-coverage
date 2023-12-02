using Microsoft.TeamFoundation.Build.WebApi;
using Microsoft.TeamFoundation.SourceControl.WebApi;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CodeCoverage.Interfaces.Services
{
    public interface IAzureService
    {
        public Task<Build> GetLatestBuild(string organization, string project, string definitionId, string branchName);
        public Task<Models.CodeCoverageSummary> GetCodeCoverage(string organization, string project, string buildId);
        public Task<IEnumerable<GitPullRequest>> GetPullRequests(string organization, string project, string respositoryId);
        public Task<GitPullRequest> GetPullRequest(string organization, string project, string respositoryId, int pullRequestId);
        public Task<GitPullRequestStatus> PostPullRequestStatus(string organization, string project, string respositoryId, int pullRequestId, GitPullRequestStatus pullRequestStatus);
        public Task<GitPullRequestCommentThread> PostPullRequestCommentThread(string organization, string project, string respositoryId, int pullRequestId, GitPullRequestCommentThread pullRequestCommentThread);
        public Task<GitPullRequestCommentThread> PatchPullRequestCommentThread(string organization, string project, string respositoryId, int pullRequestId, GitPullRequestCommentThread pullRequestCommentThread);
    }
}
