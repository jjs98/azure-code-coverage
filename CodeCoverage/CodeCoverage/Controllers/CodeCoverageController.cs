using CodeCoverage.Interfaces.Handler;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CodeCoverage.Controllers
{
    [ApiController]
    [Route("api/[controller]/{organization}/{project}/{definitionId}")]
    public class CodeCoverageController : ControllerBase
    {
        private readonly IAzureHandler _azureHandler;
        public CodeCoverageController(IAzureHandler azureHandler)
        {
            _azureHandler = azureHandler;
        }

        [HttpGet]
        public async Task<IActionResult> GetCoveragePercentage(string organization, string project, string definitionId, string branchName)
        {
            var coverage = await _azureHandler.GetCodeCoverageAsPercentage(organization, project, definitionId, branchName);
            return coverage is null ? NotFound() : Ok(coverage);
        }

        [HttpGet]
        [Route("Status")]
        public async Task<IActionResult> GetCoverageStatus(string organization, string project, string definitionId, string branchName = "main", int decimalPlaces = 0, string? displayName = default, int errorThreshhold = 30, int warningThreshhold = 70)
        {
            if (decimalPlaces < 0)
                return BadRequest("Invalid decimalPlaces");
            if (errorThreshhold < 0 || errorThreshhold > 100)
                return BadRequest("Invalid errorThreshhold");
            if (warningThreshhold < 0 || warningThreshhold > 100)
                return BadRequest("Invalid warningThreshhold");
            if (warningThreshhold <= errorThreshhold)
                return BadRequest("errorThreshhold must be greater than warningThreshhold");

            var svg = await _azureHandler.GetCodeCoverageAsStatusBadge(organization, project, definitionId, branchName, decimalPlaces, displayName, errorThreshhold, warningThreshhold);
            return svg is null ? NotFound() : Content(svg, "image/svg+xml; charset=utf-8");
            ;
        }
    }
}
