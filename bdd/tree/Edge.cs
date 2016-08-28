using System;
using System.Diagnostics;

namespace DecisionDiagrams
{

    [DebuggerDisplay("E:{Label.ToString()} {TargetVertex.ToString()}")]
    public class Edge<TVertexType, TEdgeLabelType> : GraphElementSupertype, IEquatable<Edge<TVertexType, TEdgeLabelType>>
        where TVertexType : IEquatable<TVertexType>
        where TEdgeLabelType : IEquatable<TEdgeLabelType>
    {
        public Edge(TEdgeLabelType e, Vertex<TVertexType, TEdgeLabelType> target)
        {
            Label = e;
            TargetVertex = target;
        }

        public Edge(TVertexType v, TEdgeLabelType e)
        {
            Label = e;
            TargetVertex = new Vertex<TVertexType, TEdgeLabelType>(v);
        }
        public TEdgeLabelType Label { get; set; }
        public Vertex<TVertexType, TEdgeLabelType> OriginVertex { get; internal set; }
        public Vertex<TVertexType, TEdgeLabelType> TargetVertex { get; set; }

        public bool Equals(Edge<TVertexType, TEdgeLabelType> other)
        {
            return Label.Equals(other.Label) &&
                TargetVertex.Equals(other.TargetVertex);
        }
    }
}