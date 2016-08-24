using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bdd
{
    using DtNode = Edge<BaseDtVertexType, DtBranchTest>;
    public class EvaluatorVisitor : BaseVisitor<BaseDtVertexType, DtBranchTest>, IVisitor<BaseDtVertexType, DtBranchTest>
    {
        public EvaluatorVisitor(DecisionTree<BaseDtVertexType, DtBranchTest> dt, Environment environment):base(dt)
        {
            EvaluatedResult = null;
            this.Environment = environment;
        }

        public Environment Environment { get; private set; }
        public string EvaluatedResult { get; set; }
        public override bool StartVisit(DtNode n)
        {
            // keep drilling down until we get a result from an outcome
            if (EvaluatedResult == null)
            {
                return true;
            }
            // if we haven't found a match yet, then look at the link to see if the 
            // test value matches the bound variable in the environment.
            // is the link we're on a match with the variable in the environment?  
            // If so, then visit that, but not the others
            if (n.OriginVertex.Content is DtTest)
            {
                var x = n.OriginVertex.Content as DtTest;
                var testValue = Environment.Resolve(x.Attribute);
                return testValue.Equals(n.Label.TestValue.Value);
            }
            // After a result has been found, then stop looking for a result
            return false;
        }

        public override void Visit(DtNode n)
        {
            if (n.TargetVertex.Content is DtOutcome)
            {
                EvaluatedResult = ((DtOutcome)n.TargetVertex.Content).OutcomeValue;
            }
        }
    }

    public class PrettyPrinter : BaseVisitor<BaseDtVertexType, DtBranchTest>, IVisitor<BaseDtVertexType, DtBranchTest>
    {
        public PrettyPrinter(DecisionTree<BaseDtVertexType, DtBranchTest> dt) : base(dt)
        {
        }
    }
}
