using System;
using System.Diagnostics;

namespace DecisionDiagrams
{

    [DebuggerDisplay("{ToDebuggerString()}")]
    public class DtBranchTest
    {
        public AttributePermissibleValue TestValue { get; set; }
        public DtBranchTest(AttributePermissibleValue testValue)
        {
            this.TestValue = testValue;
        }

        public string ToDebuggerString()
        {
            return $"L:({TestValue.ClassName}: {TestValue.Value})";
        }
    }
}
