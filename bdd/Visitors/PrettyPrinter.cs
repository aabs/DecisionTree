using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DecisionDiagrams
{
    using DT = GraphType;
    public class PrettyPrinter : VisitorSupertype
    {
        public PrettyPrinter(DT dt) : base(dt)
        {
        }
    }

}
