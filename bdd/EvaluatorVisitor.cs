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

    public class RecursiveSimplifier : VisitorSupertype
    {
        public RecursiveSimplifier(DT dt) : base(dt)
        {
        }

        new void Visit(TE n)
        {

        }

        new void Visit(TV v)
        {

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


            var children = from c in v.Children
                           select Simplify(c);

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
            return new TE(
                new DtBranchTest(new AttributePermissibleValue { ClassName = e.Label.TestValue.ClassName, Value = e.Label.TestValue.Value }),
                Simplify(e.TargetVertex)
                );
        }

        TV Duplicate(TV v, IEnumerable<TE> newChildren)
        {
        }

        TV Duplicate(TV v, IEnumerable<TE> newChildren)
        {
            // assumption is that no simplifications were possible for v, so a straight copy is all that is required.
            // all of the children will have been duplicated in attempting to simplify them, so no need to repeat the process.
            return new TV
            throw new NotImplementedException();
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
