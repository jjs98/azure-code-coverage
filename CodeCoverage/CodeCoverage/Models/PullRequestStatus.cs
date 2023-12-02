using Microsoft.TeamFoundation.SourceControl.WebApi;

namespace CodeCoverage.Models
{
    public class PullRequestStatus
    {
        public int IterationId { get; set; }
        public GitStatusState State { get; set; }
        public string Description { get; set; }
        public GitStatusContext Context { get; set; }

        public GitPullRequestStatus ConvertToGitPullRequestStatus()
        {
            return new GitPullRequestStatus
            {
                IterationId = IterationId,
                State = State,
                Description = Description,
                Context = Context
            };
        }
    }
}
