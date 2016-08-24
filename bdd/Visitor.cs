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
        bool StartVisit(Edge<TNodeType, TTestType> n);
        void EndVisit(Edge<TNodeType, TTestType> n);
    }
    public class Dispatcher<TNodeType, TTestType>
        where TNodeType : IEquatable<TNodeType>
        where TTestType : IEquatable<TTestType>
    {
        public static void AcceptLeaf<TV>(Edge<TNodeType, TTestType> n, TV visitor)
          where TV : IVisitor<TNodeType, TTestType>
        {
            visitor.Visit(n);
        }

        public static void AcceptBranch<TV>(Edge<TNodeType, TTestType> n, TV visitor)
          where TV : IVisitor<TNodeType, TTestType>
        {
            if (n.TargetVertex.Content is DtOutcome)
            {
                visitor.Visit(n);
            }
            else if (n.TargetVertex.Content is DtTest)
            {
                if (visitor.StartVisit(n))
                {
                    foreach (var child in n.TargetVertex.Children)
                    {
                        var targetNode = child.TargetVertex.Content;
                        if (targetNode is DtOutcome)
                        {
                            AcceptLeaf(child, visitor);
                        }
                        else if (targetNode is DtTest)
                        {
                            AcceptBranch(child, visitor);
                        }
                    }
                    visitor.EndVisit(n);
                }
            }
            else
            {
                throw new DecisionException("Unknown node type in decision tree. Don't know how to traverse tree.");
            }

        }
    }

    public class BaseVisitor<TNodeType, TTestType> : IVisitor<TNodeType, TTestType>
        where TNodeType : IEquatable<TNodeType>
        where TTestType : IEquatable<TTestType>
    {
        public DecisionTree<TNodeType, TTestType> DT { get; set; }
        public BaseVisitor(DecisionTree<TNodeType, TTestType> dt)
        {
            this.DT = dt;
        }
        public virtual void Visit(Edge<TNodeType, TTestType> n) { }
        public virtual bool StartVisit(Edge<TNodeType, TTestType> n)
        {
            return true;
        }
        public virtual void EndVisit(Edge<TNodeType, TTestType> n) { }
    }
}
