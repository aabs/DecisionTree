using System.Collections.Generic;
using System.Data;

namespace DecisionDiagrams
{
    public class DecisionMetadata
    {
        public string SampleDataLocation { get; set; }
        public DecisionOutcomes Outcomes { get; set; }
        public List<Attribute> Attributes { get; set; }
        public IEnumerable<DataRow> AllSamples { get; set; }
    }
}