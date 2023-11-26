using System.Threading.Tasks;

namespace CodeCoverage.Interfaces.Handler
{
    public interface IAzureHandler
    {
        public Task<string?> GetCodeCoverageAsPercentage(string organization, string project, string definitionId, string branchName);
        public Task<string?> GetCodeCoverageAsStatusBadge(string organization, string project, string definitionId, string branchName);
    }
}
