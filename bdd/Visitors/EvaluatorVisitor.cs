namespace bdd
{
    using DT = DecisionTree<BaseDtVertexType, DtBranchTest>;
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
}