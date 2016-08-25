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

    public class NormaliserSimplifier : VisitorSupertype
    {
        public NormaliserSimplifier(DT dt, string defaultOutcome) : base(dt)
        {
            this.DefaultOutcome = defaultOutcome;
        }

        public string DefaultOutcome { get; private set; }

        public override void Visit(TE e)
        {
            Visit(e.TargetVertex);
        }

        public override void Visit(TV v)
        {
            if (v.Content is DtTest)
            {
                var t = v.Content as DtTest;

                // first visit all the children before we go plugging any gaps
                foreach (var child in v.Children)
                {
                    Visit(child);
                }

                // for each of the permissible outcomes of the variable being tested by this vertex,
                // check that the vertex has a child for that outcome.  If it doesn't, then
                // create a default outcome for any missing outcomes.
                foreach (var dc in t.Attribute.Classes)
                {
                    // if none of the children of this vertex has a label corresponding to the
                    // data class dc, then create a default outcome
                    if (!v.Children.Any(te => dc.Name == (string)te.Label.TestValue.Value))
                    {
                        v.AddChild(
                            new DtOutcome(DefaultOutcome),
                            new DtBranchTest(new AttributePermissibleValue { ClassName = dc.Name, Value = dc.Name })
                            );
                    }
                }
            }
        }
    }
}
