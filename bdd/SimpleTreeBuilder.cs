using System.Linq;
using System.Collections.Generic;

namespace Modd
{
    using GraphType = QuickGraph.AdjacencyGraph<BaseDtVertexType, QuickGraph.TaggedEdge<BaseDtVertexType, DtBranchTest>>;
    public class SimpleTreeBuilder
    {
        readonly GraphType g;
        Stack<BaseDtVertexType> currentVertexContext = new Stack<BaseDtVertexType>();

        public SimpleTreeBuilder()
        {
            g = new GraphType();
        }

        public SimpleTreeBuilder WithRoot(BaseDtVertexType vertex)
        {
            if (currentVertexContext.Count > 0)
            {
                throw new DecisionException("Root has already been defined");
            }
            g.AddVertex(vertex);
            currentVertexContext.Push(vertex);
            return this;
        }

        public SimpleTreeBuilder AddLeaf(string name)
        {
            currentVertexContext.Push(new DtOutcome(name));
            return this;
        }

        public SimpleTreeBuilder EndChild()
        {
            currentVertexContext.Pop();
            return this;
        }

        public bool IsEmpty
        {
            get
            {
                return g.VertexCount == 0;
            }
        }

        public BaseDtVertexType CurrentVertex
        {
            get
            {
                if (currentVertexContext.Count == 0)
                {
                    return null;
                }
                return currentVertexContext.Peek();
            }
        }

        public SimpleTreeBuilder StartChild(BaseDtVertexType node, string label)
        {
            if (CurrentVertex is DtTest)
            {
                var t = CurrentVertex as DtTest;
                if (!t.Attribute.PossibleValues.Any(pv => pv.Value.Equals(label)))
                {
                    throw new DecisionException("unknown test result value supplied");
                }
            }
            if (currentVertexContext.Count == 0)
            {
                throw new DecisionException("cannot add children to an empty tree");
            }
            var ctx = currentVertexContext.Peek();
            if (currentVertexContext.Peek() is DtOutcome)
            {
                throw new DecisionException("cannot add children to a leaf node");
            }
            var edgeLabel = new AttributePermissibleValue { ClassName = label, Value = label };
            g.AddVertex(node);
            g.AddEdge(
                new QuickGraph.TaggedEdge<BaseDtVertexType, DtBranchTest>(
                    ctx, node, new DtBranchTest(edgeLabel)
                    )
                );
            currentVertexContext.Push(node);
            return this;
        }

        public DecisionTree<BaseDtVertexType, DtBranchTest> Build()
        {
            var dt = new DecisionTree<BaseDtVertexType, DtBranchTest>
            {
                Tree = g
            };
            return dt;
        }
    }

}