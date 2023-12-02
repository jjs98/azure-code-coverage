using Microsoft.TeamFoundation.SourceControl.WebApi;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CodeCoverage.Interfaces.Handler
{
    public interface IPullRequestsHandler
    {
        public Task<IEnumerable<GitPullRequest>> GetAll(string organization, string project, string repositoryId);
        public Task<GitPullRequest> Get(string organization, string project, string repositoryId, int pullRequestId);
        public Task<GitPullRequestStatus> PostStatus(string organization, string project, string respositoryId, int pullRequestId, GitPullRequestStatus pullRequestStatus);
        public Task<GitPullRequestCommentThread> PostCommentThread(string organization, string project, string respositoryId, int pullRequestId, GitPullRequestCommentThread pullRequestCommentThread);
        public Task<GitPullRequestCommentThread> PatchCommentThread(string organization, string project, string respositoryId, int pullRequestId, GitPullRequestCommentThread pullRequestCommentThread);
    }
}
