using System;
using System.Collections.Generic;
using System.Linq;

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

    public class Vertex<TVertexType, TEdgeLabelType>
        where TVertexType : IEquatable<TVertexType>
        where TEdgeLabelType : IEquatable<TEdgeLabelType>
    {
        public IEnumerable<Edge<TVertexType, TEdgeLabelType>> Parents { get; }
        public TVertexType Content { get; set; }
        List<Edge<TVertexType, TEdgeLabelType>> children = new List<Edge<TVertexType, TEdgeLabelType>>();
        private string v;

        public Vertex(TVertexType v)
        {
            this.Content = v;
        }

        public IEnumerable<Edge<TVertexType, TEdgeLabelType>> Children
        {
            get
            {
                return children;
            }
        }

        public Vertex<TVertexType, TEdgeLabelType> AddChild(Edge<TVertexType, TEdgeLabelType> e)
        {
            children.Add(e);
            return this;
        }

        public void AddChild(TVertexType n, TEdgeLabelType e)
        {
            if (children.Any(c => c.Label.Equals(e)))
            {
                throw new ApplicationException("Child link already exists.  Consider using Replace operation instead.");
            }

            var graphVertex = new Vertex<TVertexType, TEdgeLabelType>(n);
            children.Add(new Edge<TVertexType, TEdgeLabelType>(e, graphVertex));
        }

        public Vertex<TVertexType, TEdgeLabelType> Child(TEdgeLabelType e)
        {
            return (from c in Children
                    where c.Label.Equals(e)
                    select c.TargetVertex).First();
        }

        public bool EquivalentTo(Vertex<TVertexType, TEdgeLabelType> sut2)
        {
            try
            {
                bool result = this.Content.Equals(sut2.Content);
                result &= (Children.Count() == sut2.Children.Count());

                foreach (var c in children)
                {
                    var v1 = Child(c.Label);
                    var v2 = sut2.Child(c.Label);
                    result &= v1.EquivalentTo(v2);
                }

                return result;
            }
            catch
            {
                return false;
            }
        }

        public TVertexType this[TEdgeLabelType index]
        {
            get
            {
                try
                {
                    return Child(index).Content;
                }
                catch (Exception e)
                {
                    throw new DecisionException("unable to find child", e);
                }
            }
            set { AddChild(value, index); }
        }
    }

    public class Edge<TVertexType, TEdgeLabelType>
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
    }
}
