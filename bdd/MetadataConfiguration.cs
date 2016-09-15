using System.Collections.Generic;

namespace Modd
{
    public class MetadataConfiguration
    {
        public string SampleDataLocation { get; set; }
        public string OutcomeColumn { get; set; }
        public string DefaultOutcome { get; set; }
        public IEnumerable<string> ColumnsToIgnore { get; set; }
        public string Namespace { get; set; }
        public string ClassName { get; set; }
        public string FunctionName { get; set; }
    }
}