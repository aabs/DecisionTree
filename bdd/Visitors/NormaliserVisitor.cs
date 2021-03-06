﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modd
{
    using QuickGraph;
    using DT = DecisionTree<BaseDtVertexType, DtBranchTest>;
    using GraphType = QuickGraph.AdjacencyGraph<BaseDtVertexType, QuickGraph.TaggedEdge<BaseDtVertexType, DtBranchTest>>;

    public class NormaliserSimplifier : VisitorSupertype
    {
        public NormaliserSimplifier(GraphType dt, string defaultOutcome) : base(dt)
        {
            this.DefaultOutcome = defaultOutcome;
        }

        public string DefaultOutcome { get; private set; }

        public override void Visit(TaggedEdge<BaseDtVertexType, DtBranchTest> e)
        {
            Visit(e.Target);
        }

        public override void Visit(BaseDtVertexType v)
        {
            if (v is DtTest)
            {
                var t = v as DtTest;

                // first visit all the children before we go plugging any gaps
                foreach (var child in g.OutEdges(v))
                {
                    Visit(child);
                }

                // for each of the permissible outcomes of the variable being tested by this vertex,
                // check that the vertex has a child for that outcome.  If it doesn't, then
                // create a default outcome for any missing outcomes.
                foreach (var dc in t.Attribute.PossibleValues)
                {
                    // if none of the children of this vertex has a label corresponding to the
                    // data class dc, then create a default outcome

                    var children = g.OutEdges(v);
                    if (!children.Any(te => dc.Value == (string)te.Tag.TestValue.Value))
                    {
                        var newTag = new DtBranchTest(new AttributePermissibleValue { ClassName = dc.Value, Value = dc.Value });
                        var newLeaf = new DtOutcome(DefaultOutcome);
                        g.AddVertex(newLeaf);
                        g.AddEdge(new TaggedEdge<BaseDtVertexType, DtBranchTest>(v, newLeaf, newTag));
                    }
                }
            }
        }
    }
}
