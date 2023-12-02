using System.Threading.Tasks;

namespace CodeCoverage.Interfaces.Handler
{
    public interface ICodeCoverageHandler
    {
        public Task<Models.CodeCoverageSummary> GetSummary(string organization, string project, string definitionId, string branchName);
        public Task<string?> GetPercentage(string organization, string project, string definitionId, string branchName);
        public Task<string?> GetStatusBadge(string organization, string project, string definitionId, string branchName, int decimalPlaces, string? displayName, int errorThreshhold, int warningThreshhold);
    }
}
