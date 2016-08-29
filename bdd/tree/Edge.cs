using System;
using System.Diagnostics;

namespace DecisionDiagrams
{

    [DebuggerDisplay("E:{Label.ToString()} {TargetVertex.ToString()}")]
    public class KNOCKOUT_Edge<TVertexType, TEdgeLabelType> : GraphElementSupertype, IEquatable<KNOCKOUT_Edge<TVertexType, TEdgeLabelType>>
        where TVertexType : IEquatable<TVertexType>
        where TEdgeLabelType : IEquatable<TEdgeLabelType>
    {
        public KNOCKOUT_Edge(TEdgeLabelType e, KNOCKOUT_Vertex<TVertexType, TEdgeLabelType> target)
        {
            Label = e;
            TargetVertex = target;
        }

        public KNOCKOUT_Edge(TVertexType v, TEdgeLabelType e)
        {
            Label = e;
            TargetVertex = new KNOCKOUT_Vertex<TVertexType, TEdgeLabelType>(v);
        }
        public TEdgeLabelType Label { get; set; }
        public KNOCKOUT_Vertex<TVertexType, TEdgeLabelType> OriginVertex { get; internal set; }
        public KNOCKOUT_Vertex<TVertexType, TEdgeLabelType> TargetVertex { get; set; }

        public bool Equals(KNOCKOUT_Edge<TVertexType, TEdgeLabelType> other)
        {
            return Label.Equals(other.Label) &&
                TargetVertex.Equals(other.TargetVertex);
        }
    }
}