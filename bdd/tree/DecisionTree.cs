using QuickGraph;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modd
{
    using GraphType = QuickGraph.AdjacencyGraph<BaseDtVertexType, QuickGraph.TaggedEdge<BaseDtVertexType, DtBranchTest>>;
    public class DecisionTree<TNodeType, TTestType>
    {
        public SymbolTable SymbolTable { get; internal set; }
        public GraphType Tree { get; set; }
    }
}
