using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace DecisionDiagrams
{

    [DebuggerDisplay("V:{Content.ToString()}({children.Count})")]
    public class Vertex<TVertexType, TEdgeLabelType> : GraphElementSupertype, IEquatable<Vertex<TVertexType, TEdgeLabelType>>
        where TVertexType : IEquatable<TVertexType>
        where TEdgeLabelType : IEquatable<TEdgeLabelType>
    {
        public IEnumerable<Edge<TVertexType, TEdgeLabelType>> Parents { get; }
        public TVertexType Content { get; set; }
        List<Edge<TVertexType, TEdgeLabelType>> children = new List<Edge<TVertexType, TEdgeLabelType>>();

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
            // consider wiring up parent vertex propr at this point
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

        public bool IsEqual(Vertex<TVertexType, TEdgeLabelType> sut2)
        {
            try
            {
                bool result = this.Content.Equals(sut2.Content);
                result = result && (Children.Count() == sut2.Children.Count());
                if (result)
                {
                    foreach (var c in children)
                    {
                        var v1 = Child(c.Label);
                        var v2 = sut2.Child(c.Label);
                        result = result && v1.IsEqual(v2);
                    }
                }

                return result;
            }
            catch
            {
                return false;
            }
        }

        // override object.GetHashCode
        public override int GetHashCode()
        {
            // TODO: write your implementation of GetHashCode() here
            return base.GetHashCode();
        }
        // override object.Equals
        public override bool Equals(object obj)
        {
            //       
            // See the full list of guidelines at
            //   http://go.microsoft.com/fwlink/?LinkID=85237  
            // and also the guidance for operator== at
            //   http://go.microsoft.com/fwlink/?LinkId=85238
            //

            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            return IsEqual((Vertex<TVertexType, TEdgeLabelType>)obj);
        }

        public bool Equals(Vertex<TVertexType, TEdgeLabelType> other)
        {
            return IsEqual(other);
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
}