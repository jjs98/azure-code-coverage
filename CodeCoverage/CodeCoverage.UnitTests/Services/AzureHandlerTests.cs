using CodeCoverage.Handler;
using CodeCoverage.Interfaces.Services;
using FluentAssertions;
using Microsoft.TeamFoundation.Build.WebApi;
using Microsoft.TeamFoundation.TestManagement.WebApi;
using NSubstitute;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace CodeCoverage.UnitTests.Services
{
    public class AzureHandlerTests
    {
        private readonly IAzureService _azureService;
        public AzureHandlerTests()
        {
            var sub = Substitute.For<IAzureService>();
            sub.GetLatestBuild(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>()).Returns(new Build());
            _azureService = sub;
        }

        [Theory]
        [InlineData(236, 20, "8")]
        [InlineData(100, 25, "25")]
        [InlineData(30, 23, "76")]
        [InlineData(80, 22, "27")]
        [InlineData(10000, 9999, "99")]
        [InlineData(100000, 99999, "99")]
        [InlineData(10000000, 10000000, "100")]
        [InlineData(100, 100, "100")]
        [InlineData(0, 0, "0")]
        public async Task GetCodeCoverageAsPercentage(int total, int covered, string expected)
        {
            SetCoverageSubstitue(total, covered);
            var handler = new AzureHandler(_azureService);
            var percentage = await handler.GetCodeCoverageAsPercentage("", "", "", "");
            percentage.Should().Be(expected);
        }

        private void SetCoverageSubstitue(int total, int covered)
        {
            var subCoverage = new CodeCoverageSummary()
            {
                CoverageData = new List<CodeCoverageData>()
                {
                    new(){
                        CoverageStats = new List<CodeCoverageStatistics>()
                        {
                            new() {
                                Total = total,
                                Covered = covered,
                            }
                        }
                    }
                }
            };
            _azureService.GetCodeCoverage(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>()).Returns(subCoverage);
        }
    }
}