using Microsoft.TeamFoundation.Build.WebApi;
using Microsoft.TeamFoundation.TestManagement.WebApi;
using System.Threading.Tasks;

namespace CodeCoverage.Interfaces.Services
{
    public interface IAzureService
    {
        public Task<Build> GetLatestBuild(string organization, string project, string definitionId, string branchName);
        public Task<CodeCoverageSummary> GetCodeCoverage(string organization, string project, string buildId);
    }
}
