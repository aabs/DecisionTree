using System;

namespace DecisionDiagrams
{
    public abstract class BaseDtVertexType : IEquatable<BaseDtVertexType>
    {
        // override object.Equals
        public override bool Equals(object obj)
        {
            //       
            // See the full list of guidelines at
            //   http://go.microsoft.com/fwlink/?LinkID=85237  
            // and also the guidance for operator== at
            //   http://go.microsoft.com/fwlink/?LinkId=85238
            //

            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            return base.Equals(obj);
        }

        // override object.GetHashCode
        public override int GetHashCode()
        {
            // TODO: write your implementation of GetHashCode() here
            return base.GetHashCode();
        }
        public virtual bool Equals(BaseDtVertexType other)
        {
            return base.Equals(other);
        }
    }
}