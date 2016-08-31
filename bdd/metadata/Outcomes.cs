using System.Collections.Generic;

namespace Modd.Metadata
{

    public class DecisionOutcomes
    {
        public string OutcomeType { get; set; }
        public string OutcomeColumnNameInSampleData { get; set; }
        public List<DecisionOutcome> Values { get; set; }
    }
}