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
        public TVertexType Value { get; set; }
        List<Edge<TVertexType, TEdgeLabelType>> children = new List<Edge<TVertexType, TEdgeLabelType>>();
        private string v;

        public Vertex(TVertexType v)
        {
            this.Value = v;
        }

        public IEnumerable<Edge<TVertexType, TEdgeLabelType>> Children
        {
            get
            {
                return children;
            }
        }

        public Vertex<TVertexType, TEdgeLabelType> AddChild(TVertexType n, TEdgeLabelType e)
        {
            if (children.Any(c => c.LinkLabel.Equals(e)))
            {
                throw new ApplicationException("Child link already exists.  Consider using Replace operation instead.");
            }

            var graphVertex = new Vertex<TVertexType, TEdgeLabelType>(n);
            children.Add(new Edge<TVertexType, TEdgeLabelType>
            {
                LinkLabel = e,
                TargetVertex = graphVertex
            });
            return graphVertex;
        }

        public Vertex<TVertexType, TEdgeLabelType> Child(TEdgeLabelType e)
        {
            return (from c in Children
                    where c.LinkLabel.Equals(e)
                    select c.TargetVertex).First();
        }

        public bool EquivalentTo(Vertex<TVertexType, TEdgeLabelType> sut2)
        {
            try
            {
                bool result = this.Value.Equals(sut2.Value);
                result &= (Children.Count() == sut2.Children.Count());

                foreach (var c in children)
                {
                    var v1 = Child(c.LinkLabel);
                    var v2 = sut2.Child(c.LinkLabel);
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
                    return Child(index).Value;
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
        public TEdgeLabelType LinkLabel { get; set; }
        public Vertex<TVertexType, TEdgeLabelType> TargetVertex { get; set; }
    }
}
