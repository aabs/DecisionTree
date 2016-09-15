using Modd.Metadata;
using System.Diagnostics;

namespace Modd
{

    [DebuggerDisplay("{ToDebuggerString()}")]
    public class DtTest : BaseDtVertexType
    {
        public DtTest(Attribute attribute) : base()
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