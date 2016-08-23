using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bdd
{
    public class Graph<TVertexType, TEdgeLabelType>
        where TVertexType : IEquatable<TVertexType>
        where TEdgeLabelType : IEquatable<TEdgeLabelType>
    {
        /// <summary>
        /// A link to the root node of the object graph that is the graph.
        /// </summary>
        /// <remarks>
        /// The <see cref="Graph{TEdgeLabelType, TVertexType}"/> type is really just a holder 
        /// for the actual graph which is the set of <see cref="GraphVertex{TVertexType}"/> 
        /// and <see cref="GraphEdge{TEdgeLabelType}"/>
        /// </remarks>
        public GraphEdge<TVertexType, TEdgeLabelType> Root { get; set; }
        public IEnumerable<GraphVertex<TVertexType, TEdgeLabelType>> Vertexs
        {
            get
            {
                return GetAllVertexs(Root.TargetVertex);
            }
        }

        private IEnumerable<GraphVertex<TVertexType, TEdgeLabelType>> GetAllVertexs(GraphVertex<TVertexType, TEdgeLabelType> n)
        {
            yield return n;
            var childVertexs = n.Children.SelectMany(c => GetAllVertexs(c.TargetVertex));
            foreach (var cn in childVertexs)
            {
                yield return cn;
            }
        }
    }

    public class GraphVertex<TVertexType, TEdgeLabelType>
        where TVertexType : IEquatable<TVertexType>
        where TEdgeLabelType : IEquatable<TEdgeLabelType>
    {
        public IEnumerable<GraphEdge<TVertexType, TEdgeLabelType>> Parents { get; }
        public TVertexType Value { get; set; }
        List<GraphEdge<TVertexType, TEdgeLabelType>> children = new List<GraphEdge<TVertexType, TEdgeLabelType>>();

        public IEnumerable<GraphEdge<TVertexType, TEdgeLabelType>> Children
        {
            get
            {
                return children;
            }
        }

        public GraphVertex<TVertexType, TEdgeLabelType> AddChild(TVertexType n, TEdgeLabelType e)
        {
            if (children.Any(c => c.LinkLabel.Equals(e)))
            {
                throw new ApplicationException("Child link already exists.  Consider using Replace operation instead.");
            }

            var graphVertex = new GraphVertex<TVertexType, TEdgeLabelType>
            {
                Value = n
            };
            children.Add(new GraphEdge<TVertexType, TEdgeLabelType>
                {
                LinkLabel = e,
                TargetVertex = graphVertex
                });
            return graphVertex;
        }
    }

    public class GraphEdge<TVertexType, TEdgeLabelType>
        where TVertexType : IEquatable<TVertexType>
        where TEdgeLabelType : IEquatable<TEdgeLabelType>
    {
        public TEdgeLabelType LinkLabel { get; set; }
        public GraphVertex<TVertexType, TEdgeLabelType> TargetVertex { get; set; }
    }
}
