using QuickGraph;

namespace Modd
{
    using DT = GraphType;
    using TE = TaggedEdge<BaseDtVertexType, DtBranchTest>;

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
            Visit(e.Target);
        }

        public new void Visit(BaseDtVertexType v)
        {
            if (EvaluatedResult != null)
            {
                return;
            }
            // if the vertex is an outcome then take it, otherwise navigate allong the matching edge
            if (v is DtOutcome)
            {
                EvaluatedResult = ((DtOutcome)v).OutcomeValue;
                return;
            }
            if (v is DtTest)
            {
                var x = v as DtTest;
                var testValue = Environment.Resolve(x.Attribute);
                foreach (var c in g.OutEdges(v))
                {
                    var lblVal = c.Tag.TestValue.Value;
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
}