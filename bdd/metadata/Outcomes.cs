using System.Collections.Generic;

namespace DecisionDiagrams
{

    public class DecisionOutcomes
    {
        public string OutcomeType { get; set; }
        public string OutcomeColumnNameInSampleData { get; set; }
        public List<DecisionOutcome> Values { get; set; }
    }
}