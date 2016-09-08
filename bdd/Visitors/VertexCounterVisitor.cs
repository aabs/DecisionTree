namespace Modd
{
    using QuickGraph;
    using QuickGraph.Algorithms.Search;
    using GraphType = QuickGraph.AdjacencyGraph<BaseDtVertexType, QuickGraph.TaggedEdge<BaseDtVertexType, DtBranchTest>>;
    public class VertexCounterVisitor : VisitorSupertype
    {
        private const string MarkAnnotation = "anything at all";
        private int counter = 0;

        public VertexCounterVisitor(GraphType g) : base(g)
        {
        }

        public void Reset()
        {
            counter = 0;
            var dfs = new DepthFirstSearchAlgorithm<BaseDtVertexType, TaggedEdge<BaseDtVertexType, DtBranchTest>>(g);
            dfs.FinishVertex += (BaseDtVertexType v)=> { v.DeleteAnnotation(MarkAnnotation); };
            dfs.Compute();
        }

        public int Counter { get { return counter; } }
        public string DefaultOutcome { get; private set; }

        public override void Visit(BaseDtVertexType v)
        {
            if (v.HasAnnotation(MarkAnnotation))
            {
                base.Visit(v);
                return;
            }
            v.AddAnnotation(MarkAnnotation, null);
            base.Visit(v);
            counter++;
        }
    }
}
