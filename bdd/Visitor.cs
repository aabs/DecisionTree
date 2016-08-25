using bdd;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bdd
{
    public interface IVisitor<TNodeType, TTestType>
        where TNodeType : IEquatable<TNodeType>
        where TTestType : IEquatable<TTestType>
    {
        void Visit(Edge<TNodeType, TTestType> n);
        void Visit(Vertex<TNodeType, TTestType> v);
    }

    public abstract class VisitorSupertype : IVisitor<BaseDtVertexType, DtBranchTest>
    {
        private DecisionTree<BaseDtVertexType, DtBranchTest> dt;

        public VisitorSupertype(DecisionTree<BaseDtVertexType, DtBranchTest> dt)
        {
            this.dt = dt;
        }

        public virtual void Visit(Vertex<BaseDtVertexType, DtBranchTest> v)
        {
            foreach (var c in v.Children)
            {
                Visit(c);
            }
        }

        public virtual void Visit(Edge<BaseDtVertexType, DtBranchTest> n)
        {
        }
    }

}