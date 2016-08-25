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

        private TV Simplify(TV v)
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

        private TE Simplify(TE e)
        {
            // edges cannot be simplified, but to support a cleaner form of recursive simplification,
            // let's pretend they can...
            var x = e.Label.TestValue == null ? new DtBranchTest(null) : new DtBranchTest(new AttributePermissibleValue { ClassName = e.Label.TestValue.ClassName, Value = e.Label.TestValue.Value });
            return new TE(
                x,
                Simplify(e.TargetVertex)
                );
        }

        private TV Duplicate(TV v)
        {
            var cs = from c in v.Children
                     select Duplicate(c);
            return Duplicate(v, cs);
        }

        private TV Duplicate(TV v, IEnumerable<TE> newChildren)
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
                    Classes = (from dc in y.Attribute.Classes select new DataClass { Id = dc.Id, Name = dc.Name }).ToList(),
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

        private TE Duplicate(TE e)
        {
            return new TE(
                new DtBranchTest(new AttributePermissibleValue { ClassName = e.Label.TestValue.ClassName, Value = e.Label.TestValue.Value }),
                Duplicate(e.TargetVertex)
                );
        }
    }
}
