using System.Collections.Generic;

namespace CodeCoverage.Models
{
    public class ValueList<T> where T : class
    {
        public IEnumerable<T> Value { get; set; }
        public int Count { get; set; }
    }
}
