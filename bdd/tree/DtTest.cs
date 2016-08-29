using System;
using System.Diagnostics;

namespace DecisionDiagrams
{

    [DebuggerDisplay("{ToDebuggerString()}")]
    public class DtTest : BaseDtVertexType
    {
        public DtTest(Attribute attribute)
        {
            this.Attribute = attribute;
        }
        public Attribute Attribute { get; set; }

        public string ToDebuggerString()
        {
            return $"T:{Attribute.Name}";
        }
    }
}