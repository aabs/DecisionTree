using System;
using System.Diagnostics;

namespace DecisionDiagrams
{

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
