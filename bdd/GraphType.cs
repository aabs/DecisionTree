using System.Linq;
namespace Modd
{
    using QuickGraph;


    public static class GraphExtensions
    {
        public static BaseDtVertexType Root(this AdjacencyGraph<BaseDtVertexType, TaggedEdge<BaseDtVertexType, DtBranchTest>> self)
        {
            return self.Vertices.First(); 
        }

    }

}