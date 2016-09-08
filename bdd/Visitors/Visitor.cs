using Modd;
using QuickGraph;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modd
{
    using GraphType = QuickGraph.AdjacencyGraph<BaseDtVertexType, QuickGraph.TaggedEdge<BaseDtVertexType, DtBranchTest>>;

    public interface IVisitor<TNodeType, TTestType>
    {
        void Visit(TaggedEdge<TNodeType, TTestType> n);
        void Visit(TNodeType v);
    }

    public abstract class VisitorSupertype : IVisitor<BaseDtVertexType, DtBranchTest>
    {
        protected GraphType g;

        protected VisitorSupertype(GraphType g)
        {
            this.g = g;
        }

        public virtual void Visit(BaseDtVertexType v)
        {
            foreach (var c in g.OutEdges(v))
            {
                Visit(c);
            }
        }

        public virtual void Visit(TaggedEdge<BaseDtVertexType, DtBranchTest> e)
        {
            Visit(e.Target);
        }
    }

}