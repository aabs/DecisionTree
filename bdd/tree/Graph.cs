using System;
using System.Collections.Generic;
using System.Linq;

namespace DecisionDiagrams
{
    public class Graph<TVertexType, TEdgeLabelType>
        where TVertexType : IEquatable<TVertexType>
        where TEdgeLabelType : IEquatable<TEdgeLabelType>
    {
        /// <summary>
        /// A link to the root node of the object graph that is the graph.
        /// </summary>
        /// <remarks>
        /// The <see cref="Graph{TVertexType, TEdgeLabelType}"/> type is really just a holder 
        /// for the actual graph which is the set of <see cref="Vertex{TVertexType, TEdgeLabelType}"/> 
        /// and <see cref="Edge{TVertexType, TEdgeLabelType}"/>
        /// </remarks>
        public Edge<TVertexType, TEdgeLabelType> Root { get; set; }
        public IEnumerable<Vertex<TVertexType, TEdgeLabelType>> Vertexs
        {
            get
            {
                return GetAllVertexs(Root.TargetVertex);
            }
        }

        private IEnumerable<Vertex<TVertexType, TEdgeLabelType>> GetAllVertexs(Vertex<TVertexType, TEdgeLabelType> n)
        {
            yield return n;
            var childVertexs = n.Children.SelectMany(c => GetAllVertexs(c.TargetVertex));
            foreach (var cn in childVertexs)
            {
                yield return cn;
            }
        }
    }
}
