using CodeCoverage.Interfaces.Handler;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CodeCoverage.Controllers
{
    [ApiController]
    [Route("api/{organization}/{project}/{definitionId}/[controller]")]
    public class CodeCoverageController : ControllerBase
    {
        private readonly ICodeCoverageHandler _codeCoverageHandler;
        public CodeCoverageController(ICodeCoverageHandler codeCoverageHandler)
        {
            _codeCoverageHandler = codeCoverageHandler;
        }

        [HttpGet()]
        [Produces("application/json", Type = typeof(Models.CodeCoverageSummary))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Models.CodeCoverageSummary>> GetSummary(string organization, string project, string definitionId, string branchName)
        {
            var coverage = await _codeCoverageHandler.GetSummary(organization, project, definitionId, branchName);
            return coverage is null ? NotFound() : Ok(coverage);
        }

        [HttpGet("percentage")]
        [Produces("application/json", Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetPercentage(string organization, string project, string definitionId, string branchName)
        {
            var coverage = await _codeCoverageHandler.GetPercentage(organization, project, definitionId, branchName);
            return coverage is null ? NotFound() : Ok(coverage);
        }

        [HttpGet("status")]
        [Produces("image/svg+xml; charset=utf-8", Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetStatus(string organization, string project, string definitionId, string branchName = "main", int decimalPlaces = 0, string? displayName = default, int errorThreshhold = 30, int warningThreshhold = 70)
        {
            if (decimalPlaces < 0)
                return BadRequest("Invalid decimalPlaces");
            if (errorThreshhold < 0 || errorThreshhold > 100)
                return BadRequest("Invalid errorThreshhold");
            if (warningThreshhold < 0 || warningThreshhold > 100)
                return BadRequest("Invalid warningThreshhold");
            if (warningThreshhold <= errorThreshhold)
                return BadRequest("errorThreshhold must be greater than warningThreshhold");

            var svg = await _codeCoverageHandler.GetStatusBadge(organization, project, definitionId, branchName, decimalPlaces, displayName, errorThreshhold, warningThreshhold);
            return svg is null ? NotFound() : Content(svg, "image/svg+xml; charset=utf-8");
            ;
        }

        [HttpPost()]
        [Produces("application/json", Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> PostCoverageStatistics(string organization, string project, string definitionId, string branchName)
        {
            var coverage = await _codeCoverageHandler.GetPercentage(organization, project, definitionId, branchName);
            return coverage is null ? NotFound() : Ok(coverage);
        }
    }
}
