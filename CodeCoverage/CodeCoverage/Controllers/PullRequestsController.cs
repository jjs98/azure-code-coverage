using CodeCoverage.Interfaces.Handler;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.TeamFoundation.SourceControl.WebApi;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CodeCoverage.Controllers
{
    [ApiController]
    [Route("api/{organization}/{project}/{repositoryId}/[controller]")]
    public class PullRequestsController : ControllerBase
    {
        private readonly IPullRequestsHandler _pullRequestsHandler;
        public PullRequestsController(IPullRequestsHandler pullRequestsHandler)
        {
            _pullRequestsHandler = pullRequestsHandler;
        }

        [HttpGet("{pullRequestId}")]
        [Produces("application/json", Type = typeof(GitPullRequest))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Get(string organization, string project, string repositoryId, int pullRequestId)
        {
            var pullRequest = await _pullRequestsHandler.Get(organization, project, repositoryId, pullRequestId);
            return pullRequest is null ? NotFound() : Ok(pullRequest);
        }

        [HttpGet]
        [Produces("application/json", Type = typeof(IEnumerable<GitPullRequest>))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAll(string organization, string project, string repositoryId)
        {
            var pullRequests = await _pullRequestsHandler.GetAll(organization, project, repositoryId);
            return pullRequests is null ? NotFound() : Ok(pullRequests);
            ;
        }

        [HttpPost("{pullRequestId}/statuses")]
        [Produces("application/json", Type = typeof(GitPullRequestStatus))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> PostStatus(string organization, string project, string repositoryId, int pullRequestId, Models.PullRequestStatus pullRequestStatus)
        {
            var gitPullRequestStatus = pullRequestStatus.ConvertToGitPullRequestStatus();
            var pullRequests = await _pullRequestsHandler.PostStatus(organization, project, repositoryId, pullRequestId, gitPullRequestStatus);
            return pullRequests is null ? NotFound() : Ok(pullRequests);
            ;
        }

        [HttpPost("{pullRequestId}/threads")]
        [Produces("application/json", Type = typeof(GitPullRequestCommentThread))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> PostThread(string organization, string project, string repositoryId, int pullRequestId, Models.PullRequestCommentThread pullRequestCommentThread)
        {
            var gitPullRequestCommentThread = pullRequestCommentThread.ConvertToGitPullRequestCommentThread();
            var pullRequests = await _pullRequestsHandler.PostCommentThread(organization, project, repositoryId, pullRequestId, gitPullRequestCommentThread);
            return pullRequests is null ? NotFound() : Ok(pullRequests);
            ;
        }

        [HttpPatch("{pullRequestId}/threads/{threadId}")]
        [Produces("application/json", Type = typeof(GitPullRequestCommentThread))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> PatchThreadStatus(string organization, string project, string repositoryId, int pullRequestId, int threadId, CommentThreadStatus status)
        {
            var gitPullRequestCommentThread = new GitPullRequestCommentThread
            {
                Id = threadId,
                Status = status
            };
            var pullRequests = await _pullRequestsHandler.PatchCommentThread(organization, project, repositoryId, pullRequestId, gitPullRequestCommentThread);
            return pullRequests is null ? NotFound() : Ok(pullRequests);
            ;
        }
    }
}
