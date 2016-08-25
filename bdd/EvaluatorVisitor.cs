using bdd;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bdd
{
    using DT = DecisionTree<BaseDtVertexType, DtBranchTest>;
    using TV = Vertex<BaseDtVertexType, DtBranchTest>;
    using TE = Edge<BaseDtVertexType, DtBranchTest>;
    public class EvaluatorVisitor : VisitorSupertype
    {
        public EvaluatorVisitor(DT dt,
            Environment environment) : base(dt)
        {
            this.EvaluatedResult = null;
            this.Environment = environment;
        }

        public Environment Environment { get; private set; }
        public string EvaluatedResult { get; set; }

        public new void Visit(TE e)
        {
            if (EvaluatedResult != null)
            {
                return;
            }
            Visit(e.TargetVertex);
        }
        public new void Visit(Vertex<BaseDtVertexType, DtBranchTest> v)
        {
            if (EvaluatedResult != null)
            {
                return;
            }
            // if the vertex is an outcome then take it, otherwise navigate allong the matching edge
            if (v.Content is DtOutcome)
            {
                EvaluatedResult = ((DtOutcome)v.Content).OutcomeValue;
                return;
            }
            if (v.Content is DtTest)
            {
                var x = v.Content as DtTest;
                var testValue = Environment.Resolve(x.Attribute);
                foreach (var c in v.Children)
                {
                    var lblVal = c.Label.TestValue.Value;
                    if (testValue == (string)lblVal)
                    {
                        Visit(c);
                        return;
                    }
                }
                EvaluatedResult = CalculateDefaultResponse();
                return;
            }
        }

        private string CalculateDefaultResponse()
        {
            return "default";
        }
    }

    public class PrettyPrinter : VisitorSupertype
    {
        public PrettyPrinter(DT dt) : base(dt)
        {
        }
    }

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
    public class VertexCounter : VisitorSupertype
    {
        int counter = 0;
        public VertexCounter(DT dt) : base(dt)
        {
        }
        public void Reset() { counter = 0; }
        public int Counter { get { return counter; } }
        public string DefaultOutcome { get; private set; }

        public override void Visit(TV v)
        {
            base.Visit(v);
            counter++;
        }
    }
    public class RecursiveSimplifier : VisitorSupertype
    {
        public TE SimplifiedTree { get; set; }
        public string DefaultOutcome { get; private set; }

        public RecursiveSimplifier(DT dt) : base(dt)
        {
        }

        public override void Visit(TE e)
        {
            SimplifiedTree = Simplify(e);
        }

        public override void Visit(TV v)
        {
            throw new NotImplementedException();
        }

        TV Simplify(TV v)
        {
            // if it's an outcome there is no sub structure or simplifications possible
            // so just duplicate and return.
            if (v.Content is DtOutcome)
            {
                return Duplicate(v);
            }

            // if not, then it must have children.


            var children = (from c in v.Children
                           select Simplify(c)).ToList();

            // if all children are the same (whether leaves or trees) then
            // v can be replaced with any one of the children. All of the rest
            // of the children, plus v, can be thrown away (V is an element from 
            // the original tree being simplified, so it probably won't be disposed right away
            // but at least it won't make it into the new simplified tree.
            if (children.AllIdentical())
            {
                return children.First().TargetVertex;
            }


            //  if we get to this point, then there are no simplifications possible, so just return a duplicate of the tree.
            return Duplicate(v, children);
        }

        TE Simplify(TE e)
        {
            // edges cannot be simplified, but to support a cleaner form of recursive simplification,
            // let's pretend they can...
            var x = e.Label.TestValue == null ? new DtBranchTest(null) : new DtBranchTest(new AttributePermissibleValue { ClassName = e.Label.TestValue.ClassName, Value = e.Label.TestValue.Value });
            return new TE(
                x,
                Simplify(e.TargetVertex)
                );
        }

        TV Duplicate(TV v)
        {
            var cs = from c in v.Children
                     select Duplicate(c);
            return Duplicate(v, cs);
        }

        TV Duplicate(TV v, IEnumerable<TE> newChildren)
        {
            // assumption is that no simplifications were possible for v, so a straight copy is all that is required.
            // all of the children will have been duplicated in attempting to simplify them, so no need to repeat the process.
            BaseDtVertexType contents = null;

            if (v.Content is DtOutcome)
            {
                var x = v.Content as DtOutcome;
                contents = new DtOutcome(x.OutcomeValue);
            }
            if (v.Content is DtTest)
            {
                var y = v.Content as DtTest;
                contents = new DtTest(new DecisionSpaceAttribute
                {
                    Classes = (from dc in y.Attribute.Classes select new DataClass {Id = dc.Id, Name = dc.Name }).ToList(),
                    DataType = y.Attribute.DataType,
                    Name = y.Attribute.Name
                });
            }
            var result = new TV(contents);
            foreach (var c in newChildren)
            {
                result.AddChild(c);
            }

            return result;
        }

        TE Duplicate(TE e)
        {
            return new TE(
                new DtBranchTest(new AttributePermissibleValue { ClassName = e.Label.TestValue.ClassName, Value = e.Label.TestValue.Value }),
                Duplicate(e.TargetVertex)
                );
        }
    }
}
