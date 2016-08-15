using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bdd
{
    [Serializable]
    public class DecisionException : ApplicationException
    {
        public DecisionException() { }
        public DecisionException(string message) : base(message) { }
        public DecisionException(string message, Exception inner) : base(message, inner) { }
        protected DecisionException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
