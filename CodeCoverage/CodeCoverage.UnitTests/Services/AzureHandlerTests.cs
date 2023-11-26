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
            var subBuild = new Build()
            {
                Definition = new DefinitionReference()
                {
                    Name = "Test"
                }
            };
            var sub = Substitute.For<IAzureService>();
            sub.GetLatestBuild(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>()).Returns(subBuild);
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


        [Theory]
        [InlineData(0, 0, 0, "Test1", 30, 70, "0%", AzureHandler.Red, "Test1")]
        [InlineData(100, 0, 0, "Test1", 30, 70, "0%", AzureHandler.Red, "Test1")]
        [InlineData(100, 100, 0, "Test1", 30, 70, "100%", AzureHandler.Green, "Test1")]
        [InlineData(100, 100, 0, null, 30, 70, "100%", AzureHandler.Green, "Test")]
        [InlineData(100, 100, 0, "", 30, 70, "100%", AzureHandler.Green, "Test")]
        [InlineData(100, 100, 0, " ", 30, 70, "100%", AzureHandler.Green, "Test")]
        [InlineData(100, 30, 0, "Test1", 30, 70, "30%", AzureHandler.Yellow, "Test1")]
        [InlineData(100, 70, 0, "Test1", 30, 70, "70%", AzureHandler.Green, "Test1")]
        [InlineData(10000, 9999, 0, "Test1", 30, 70, "99%", AzureHandler.Green, "Test1")]
        [InlineData(100000, 1, 2, "Test1", 30, 100, "0%", AzureHandler.Red, "Test1")]
        [InlineData(100000, 1, 4, "Test1", 30, 100, "0,0010%", AzureHandler.Red, "Test1")]
        [InlineData(100000, 99999, 0, "Test1", 30, 100, "99%", AzureHandler.Yellow, "Test1")]
        [InlineData(100000, 99999, 0, "Test1", 30, 70, "99%", AzureHandler.Green, "Test1")]
        [InlineData(100000, 99999, 1, "Test1", 30, 100, "100%", AzureHandler.Green, "Test1")]
        [InlineData(10000000, 10000000, 0, "Test1", 30, 70, "100%", AzureHandler.Green, "Test1")]
        [InlineData(236, 20, 0, "Test1", 30, 70, "8%", AzureHandler.Red, "Test1")]
        [InlineData(30, 23, 0, "Test1", 30, 70, "76%", AzureHandler.Green, "Test1")]
        [InlineData(30, 23, 1, "Test1", 30, 70, "76,7%", AzureHandler.Green, "Test1")]
        [InlineData(30, 23, 2, "Test1", 30, 70, "76,67%", AzureHandler.Green, "Test1")]
        [InlineData(30, 23, 3, "Test1", 30, 70, "76,667%", AzureHandler.Green, "Test1")]
        [InlineData(80, 22, 0, "Test1", 0, 0, "27%", AzureHandler.Green, "Test1")]
        [InlineData(80, 22, 0, "Test1", 0, 30, "27%", AzureHandler.Yellow, "Test1")]
        [InlineData(80, 22, 0, "Test1", 27, 28, "27%", AzureHandler.Yellow, "Test1")]
        [InlineData(80, 22, 0, "Test1", 30, 70, "27%", AzureHandler.Red, "Test1")]
        public async Task GetCodeCoverageAsStatusBadge(int total, int covered, int decimalPlaces, string? displayName, int errorThreshhold, int warningThreshhold, string expectedPercentage, string expectedColor, string expectedDisplayName)
        {
            SetCoverageSubstitue(total, covered);
            var handler = new AzureHandler(_azureService);
            var svg = await handler.GetCodeCoverageAsStatusBadge("", "", "", "", decimalPlaces, displayName, errorThreshhold, warningThreshhold);
            svg.Should().Contain(">" + expectedPercentage + "</text>");
            svg.Should().Contain(expectedColor);
            svg.Should().Contain(expectedDisplayName);
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