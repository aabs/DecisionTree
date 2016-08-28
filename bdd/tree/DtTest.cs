using System;
using System.Diagnostics;

namespace DecisionDiagrams
{

    [DebuggerDisplay("{ToString()}")]
    public class DtTest : BaseDtVertexType, IEquatable<DtTest>
    {
        public DtTest(Attribute attribute)
        {
            this.Attribute = attribute;
        }
        public Attribute Attribute { get; set; }

        // override object.Equals
        public override bool Equals(object obj)
        {
            return IsEqual(obj);
        }

        private bool IsEqual(object obj)
        {
            var other = obj as DtTest;
            if (other == null)
            {
                return false;
            }

            return Attribute.Equals(other.Attribute);
        }

        // override object.GetHashCode
        public override int GetHashCode()
        {
            // TODO: write your implementation of GetHashCode() here
            return Attribute.GetHashCode();
        }
        public override bool Equals(BaseDtVertexType other)
        {
            return IsEqual(other);
        }

        public bool Equals(DtTest other)
        {
            return IsEqual(other);
        }

        public override string ToString()
        {
            return $"Test:{Attribute.Name}";
        }
    }
}