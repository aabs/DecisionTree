namespace Modd
{
    public class VertexCounterVisitor : VisitorSupertype
    {
        private int counter = 0;

        public VertexCounterVisitor(GraphType g) : base(g)
        {
        }

        public void Reset()
        {
            counter = 0;
        }

        public int Counter { get { return counter; } }
        public string DefaultOutcome { get; private set; }

        public override void Visit(BaseDtVertexType v)
        {
            base.Visit(v);
            counter++;
        }
    }
}
