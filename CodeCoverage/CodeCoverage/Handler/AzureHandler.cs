﻿using CodeCoverage.Interfaces.Handler;
using CodeCoverage.Interfaces.Services;
using Microsoft.TeamFoundation.TestManagement.WebApi;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace CodeCoverage.Handler
{
    public class AzureHandler : IAzureHandler
    {
        private readonly IAzureService _azureService;

        public const string Green = "#4EC820";
        public const string Yellow = "#ebc63b";
        public const string Red = "#ed1e45";

        public AzureHandler(IAzureService azureService)
        {
            _azureService = azureService;
        }

        public async Task<string?> GetCodeCoverageAsPercentage(string organization, string project, string definitionId, string branchName)
        {
            var build = await _azureService.GetLatestBuild(organization, project, definitionId, branchName);
            var coverage = await _azureService.GetCodeCoverage(organization, project, build.Id.ToString());
            var coverageData = coverage.CoverageData.FirstOrDefault()?.CoverageStats.FirstOrDefault();
            var percentage = CalculatePercentage(coverageData, 0);
            return percentage.ToString();
        }

        public async Task<string?> GetCodeCoverageAsStatusBadge(string organization, string project, string definitionId, string branchName, int decimalPlaces, string? displayName, int errorThreshhold, int warningThreshhold)
        {
            var build = await _azureService.GetLatestBuild(organization, project, definitionId, branchName);
            var coverage = await _azureService.GetCodeCoverage(organization, project, build.Id.ToString());
            var coverageData = coverage.CoverageData.FirstOrDefault()?.CoverageStats.FirstOrDefault();
            var percentage = CalculatePercentage(coverageData, decimalPlaces);

            return FormatSvg(percentage, string.IsNullOrWhiteSpace(displayName) ? build.Definition.Name : displayName, errorThreshhold, warningThreshhold);
        }

        private static decimal CalculatePercentage(CodeCoverageStatistics? statistics, int decimalPlaces)
        {
            decimal percentage = 0;
            if (statistics is not null && statistics.Total > 0)
                percentage = (decimal)statistics.Covered / (decimal)statistics.Total * (decimal)100;
            percentage = Math.Round(percentage, decimalPlaces, decimalPlaces == 0 ? MidpointRounding.ToNegativeInfinity : MidpointRounding.ToEven);

            // To make sure there are no decimals for 0 and 100%
            if (percentage == 100)
                percentage = 100;
            if (percentage == 0)
                percentage = 0;
            return percentage;
        }

        private static string FormatSvg(decimal percentage, string definitionName, int errorThreshhold, int warningThreshhold)
        {
            var fillColor = Green;

            if (percentage < errorThreshhold && percentage >= 0)
                fillColor = Red;
            if (percentage < warningThreshhold && percentage >= errorThreshhold)
                fillColor = Yellow;

            var template = $"""
                <svg width="180.8" height="20.0" xmlns="http://www.w3.org/2000/svg">
                  <linearGradient id="a" x2="0" y2="100%">
                    <stop offset="0.0" stop-opacity="0.0" stop-color="#000" />
                    <stop offset="1.0" stop-opacity="0.2" stop-color="#000" />
                  </linearGradient>
                  <clipPath id="c">
                    <rect width="180.8" height="20.0" rx="3.0" />
                  </clipPath>
                  <g clip-path="url(#c)">
                    <rect width="180.8" height="20.0" fill="#555555" />
                    <rect width="68.9" height="20.0" fill="{fillColor}" x="111.9" />
                    <rect width="180.8" height="20.0" fill="url(#a)" />
                  </g>
                  <svg width="12" height="12" viewBox="0 0 12 12" fill="none" xmlns="http://www.w3.org/2000/svg" x="5" y="4">
                    <g>
                      <path fill-rule="evenodd" clip-rule="evenodd" d="M0 9H1V11H3L3 12H0V9Z" fill="#fff" />
                      <path fill-rule="evenodd" clip-rule="evenodd" d="M0.666656 4H3.7352L6.20309 0.444336C6.38861 0.166992 6.70068 0 7.03479 0H11.5C11.7762 0 12 0.224609 12 0.5V4.96484C12 5.29883 11.8332 5.61133 11.5553 5.79688L8 8.26465V11.333C8 11.7012 7.70154 12 7.33334 12H5L4 11L5.25 9.75L4.25 8.75L2.99997 10L1.99997 9L3.25 7.75L2.25 6.75L1 8L0 7V4.66699C0 4.29883 0.298462 4 0.666656 4ZM10.5 3C10.5 3.82812 9.82843 4.5 9.00003 4.5C8.1716 4.5 7.50003 3.82812 7.50003 3C7.50003 2.17188 8.1716 1.5 9.00003 1.5C9.82843 1.5 10.5 2.17188 10.5 3Z" fill="#fff" />
                    </g>
                  </svg>
                  <g fill="#fff" text-anchor="middle" font-family="DejaVu Sans,Verdana,Geneva,sans-serif" font-size="11">
                    <text x="64.9" y="15.0" fill="#000" fill-opacity="0.3">{definitionName}</text>
                    <text x="64.9" y="14.0" fill="#fff">{definitionName}</text>
                    <text x="145.4" y="15.0" fill="#000" fill-opacity="0.3">{percentage}%</text>
                    <text x="145.4" y="14.0" fill="#fff">{percentage}%</text>
                  </g>
                </svg>
                """;
            return template;
        }
    }
}
