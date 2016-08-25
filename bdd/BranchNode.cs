using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics.Contracts;
using System.Threading.Tasks;
using System.Text;
using System.Diagnostics;

namespace bdd
{
    public abstract class BaseDtVertexType : IEquatable<BaseDtVertexType>
    {
        public bool Equals(BaseDtVertexType other) { return false; }
    }

    [DebuggerDisplay("{ToString()}")]
    public class DtTest : BaseDtVertexType, IEquatable<DtTest>
    {
        public DtTest(DecisionSpaceAttribute attribute)
        {
            this.Attribute = attribute;
        }
        public DecisionSpaceAttribute Attribute { get; set; }

        public new bool Equals(BaseDtVertexType other)
        {
            return Equals(other as DtTest);
        }

        public bool Equals(DtTest other)
        {
            return Attribute.Equals(other.Attribute);
        }

        public override string ToString()
        {
            return $"Test:{Attribute.Name}";
        }
    }

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
        public new bool Equals(BaseDtVertexType other)
        {
            if (other == null)
            {
                throw new ArgumentNullException(nameof(other));
            }
            return Equals(other as DtOutcome);
        }

        public bool Equals(DtOutcome other)
        {
            if (other == null)
            {
                throw new ArgumentNullException(nameof(other));
            }
            return OutcomeValue.Equals(other.OutcomeValue);
        }
        public override string ToString()
        {
            return $"Outcome:{OutcomeValue}";
        }
    }

    [DebuggerDisplay("{ToString()}")]
    public class DtBranchTest : IEquatable<DtBranchTest>
    {
        public AttributePermissibleValue TestValue { get; set; }
        public DtBranchTest(AttributePermissibleValue testValue)
        {
            this.TestValue = testValue;
        }
        public bool Equals(DtBranchTest other)
        {
            return TestValue.Equals(other.TestValue);
        }
        public override string ToString()
        {
            return $"BranchTest:({TestValue.ClassName}: {TestValue.Value})";
        }
    }
}
