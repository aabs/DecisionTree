using Modd;
using QuickGraph;

namespace bdd_tests
{
    using FluentAssertions;
    using QuickGraph.Algorithms.Search;
    using System;
    using System.Linq;
    using EdgeType = TaggedEdge<V, string>;
    using GraphType = AdjacencyGraph<V, TaggedEdge<V, string>>;

    [NUnit.Framework.TestFixture]
    public class AnnotationTest
    {
        [NUnit.Framework.Test]
        public void TestCanPurgeAnnotationsFromATree()
        {
            var sut = CreateLabelledTree();
            // first check that the annotations are there
            sut.Item1.Vertices.All(v=>v.GetAnnotation("Hashcode") != null).Should().BeTrue();
            // then check that after purging, they are gone.

            // (same code from Reducer) ((yes I know))
            var dfs = new DepthFirstSearchAlgorithm<V, TaggedEdge<V, string>>(sut.Item1);
            dfs.FinishVertex += (v) => {
                v.DeleteAnnotation("Hashcode");
            };
            dfs.Compute();
            sut.Item1.Vertices.Any(v=>v.GetAnnotation("Hashcode") != null).Should().BeFalse();

        }

        Tuple<GraphType, V> CreateLabelledTree()
        {
            GraphType g = new GraphType();
            var root = new V();
            var v1 = new V();
            var v2 = new V();
            var v11 = new V();
            var v12 = new V();
            var v21 = new V();
            var v22 = new V();
            var v221 = new V();
            var v222 = new V();
            g.AddVertexRange(new[] { root, v1, v2, v11, v12, v21, v22, v221, v222 });
            g.AddEdgeRange(new[] {
                new EdgeType(root, v1, "rv1"),
                new EdgeType(root, v2, "rv2"),
                new EdgeType(v1,v11, "v1v11"),
                new EdgeType(v1, v12, "v1v12"),
                new EdgeType(v2,v21, "v2v21"),
                new EdgeType(v2, v22, "v2v22"),
                new EdgeType(v22, v221, "v22v221"),
                new EdgeType(v22, v222, "v22v222") }
                );
            LabelSomeThings(root, v1, v11, v12, v2, v21, v22, v221, v222);
            return Tuple.Create(g, root);
        }

        public void LabelSomeThings(params Annotatable[] args)
        {
            args.Foreach(a => a.AddAnnotation("Hashcode", a.GetHashCode()));
        }
    }
    public class V : Annotatable
    {
    }
    public class E : Annotatable
    {
    }
}