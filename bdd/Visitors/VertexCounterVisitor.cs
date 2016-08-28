namespace DecisionDiagrams
{
    using DT = DecisionTree<BaseDtVertexType, DtBranchTest>;
    using TV = Vertex<BaseDtVertexType, DtBranchTest>;
    public class VertexCounterVisitor : VisitorSupertype
    {
        private int counter = 0;

        public VertexCounterVisitor(DT dt) : base(dt)
        {
        }

        public void Reset()
        {
            counter = 0;
        }

        public int Counter { get { return counter; } }
        public string DefaultOutcome { get; private set; }

        public override void Visit(TV v)
        {
            base.Visit(v);
            counter++;
        }
    }
}
