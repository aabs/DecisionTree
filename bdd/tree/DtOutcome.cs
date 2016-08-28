using System;
using System.Diagnostics;

namespace DecisionDiagrams
{
    [DebuggerDisplay("{ToString()}")]
    public class DtOutcome : BaseDtVertexType, IEquatable<DtOutcome>
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
        // override object.Equals
        public override bool Equals(object obj)
        {
            return IsEqual(obj);
        }

        // override object.GetHashCode
        public override int GetHashCode()
        {
            // TODO: write your implementation of GetHashCode() here
            return OutcomeValue.GetHashCode();
        }
        public override bool Equals(BaseDtVertexType other)
        {
            return IsEqual(other);
        }

        public bool Equals(DtOutcome other)
        {
            return IsEqual(other);
        }

        private bool IsEqual(object obj)
        {
            // since this compares the hashcodes of the outcome value, 
            // then equality will return true for two DtOutcomes 
            // (so long as they both have the same string value for their outcome values)
            return GetHashCode() == obj.GetHashCode();
            //if (obj == null || GetType() != obj.GetType())
            //{
            //    return false;
            //}

            //var other = obj as DtOutcome;
            //return OutcomeValue.Equals(other.OutcomeValue);
        }
        public override string ToString()
        {
            return $"==:{OutcomeValue}";
        }
        
    }
}