using Microsoft.TeamFoundation.SourceControl.WebApi;
using Microsoft.VisualStudio.Services.WebApi;

namespace CodeCoverage.Models
{
    public class PullRequestCommentThread
    {
        public int IterationId { get; set; }
        public CommentThreadStatus Status { get; set; }
        public string Content { get; set; }

        public GitPullRequestCommentThread ConvertToGitPullRequestCommentThread()
        {
            return new GitPullRequestCommentThread
            {
                Comments = new[]
                {
                    new Comment
                    {
                        Author = new IdentityRef
                        {
                            DisplayName = "My Service"
                        },
                        Content = Content,
                        CommentType = CommentType.System

                    }
                },
                Status = Status,
                Properties = new PropertiesCollection
                {
                    { "IterationId" ,IterationId }
                }
            };
        }
    }
}
