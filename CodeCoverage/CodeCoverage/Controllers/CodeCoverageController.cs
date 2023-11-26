using CodeCoverage.Interfaces.Handler;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CodeCoverage.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CodeCoverageController : ControllerBase
    {
        private readonly IAzureHandler _azureHandler;
        public CodeCoverageController(IAzureHandler azureHandler)
        {
            _azureHandler = azureHandler;
        }

        [HttpGet]
        public async Task<IActionResult> GetCoveragePercentage(string organization, string project, string pipelineId, string branchName)
        {
            var coverage = await _azureHandler.GetCodeCoverageAsPercentage(organization, project, pipelineId, branchName);
            return coverage is null ? NotFound() : Ok(coverage);
        }

        [HttpGet]
        [Route("Status")]
        public async Task<IActionResult> GetCoverageStatus(string organization, string project, string pipelineId, string branchName)
        {
            var svg = await _azureHandler.GetCodeCoverageAsStatusBadge(organization, project, pipelineId, branchName);
            return svg is null ? NotFound() : Content(svg, "image/svg+xml; charset=utf-8");
            ;
        }
    }
}
