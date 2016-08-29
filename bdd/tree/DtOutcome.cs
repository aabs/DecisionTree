using System;
using System.Diagnostics;

namespace DecisionDiagrams
{
    [DebuggerDisplay("{ToDebuggerString()}")]
    public class DtOutcome : BaseDtVertexType
    {
        public DtOutcome(string outcome)
        {
            if (outcome == null)
            {
                throw new ArgumentNullException(nameof(outcome));
            }
            this.OutcomeValue = outcome;
        }
        public string OutcomeValue { get; set; }

        public string ToDebuggerString()
        {
            return $"O:{OutcomeValue}";
        }
        
    }
}