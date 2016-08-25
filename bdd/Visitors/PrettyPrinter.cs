using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bdd
{
    using DT = DecisionTree<BaseDtVertexType, DtBranchTest>;
    using TE = Edge<BaseDtVertexType, DtBranchTest>;
    using TV = Vertex<BaseDtVertexType, DtBranchTest>;
    public class PrettyPrinter : VisitorSupertype
    {
        public PrettyPrinter(DT dt) : base(dt)
        {
        }
    }

}
