using System.Linq;
namespace Modd
{
    using QuickGraph;
    using System.Collections.Generic;
    using GraphType = QuickGraph.AdjacencyGraph<BaseDtVertexType, QuickGraph.TaggedEdge<BaseDtVertexType, DtBranchTest>>;
    using EdgeType = QuickGraph.TaggedEdge<BaseDtVertexType, DtBranchTest>;
    public static class GraphExtensions
    {
        public static BaseDtVertexType Root(this GraphType self)
        {
            return self.Vertices.First();
        }

        public static IEnumerable<BaseDtVertexType> Parents(this GraphType self, BaseDtVertexType v)
        {
            return self.InEdges(v).Select(e => e.Source);
        }

        public static IEnumerable<TaggedEdge<BaseDtVertexType, DtBranchTest>> InEdges(this GraphType self, BaseDtVertexType v)
        {
            return self.Edges.Where(e => e.Target.Equals(v));
        }

    }

}