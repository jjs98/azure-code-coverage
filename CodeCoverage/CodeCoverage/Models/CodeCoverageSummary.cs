using System.Collections.Generic;

namespace CodeCoverage.Models
{
    public class CodeCoverageSummary
    {
        public IList<CodeCoverageData> CoverageData { get; set; }
        public Microsoft.TeamFoundation.TestManagement.WebApi.ShallowReference Build { get; set; }
        public Microsoft.TeamFoundation.TestManagement.WebApi.ShallowReference DeltaBuild { get; set; }
        public Microsoft.TeamFoundation.TestManagement.WebApi.CoverageSummaryStatus Status { get; set; }
    }

    public class CodeCoverageData
    {
        public IList<CodeCoverageStatistics> CoverageStats { get; set; }
        public string BuildPlatform { get; set; }
        public string BuildFlavor { get; set; }
    }

    public class CodeCoverageStatistics
    {
        public string Label { get; set; }
        public int Position { get; set; }
        public int Total { get; set; }
        public int Covered { get; set; }
        public bool IsDeltaAvailable { get; set; }
        public double Delta { get; set; }
    }
}
