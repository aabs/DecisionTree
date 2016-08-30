using QuickGraph;
using QuickGraph.Algorithms.Search;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DecisionDiagrams
{
    public class Reducer
    {
        Dictionary<string, string> labels = new Dictionary<string, string>();
        Dictionary<string, List<BaseDtVertexType>> vertices = new Dictionary<string, List<BaseDtVertexType>>();
        GraphType graph;
        SymbolTable symbolTable;
        int labelCounter;
        public GraphType Reduce(SymbolTable st, GraphType g)
        {
            symbolTable = st;
            graph = g;
            labels.Clear();
            //do the search
            var dfs = new DepthFirstSearchAlgorithm<BaseDtVertexType, TaggedEdge<BaseDtVertexType, DtBranchTest>>(g);
            dfs.FinishVertex += Dfs_LabelVertex;
            dfs.Compute();
            // at this point, the tree should be properly labelled.
            // now go through the tree substituting the first vertex 
            // registered against each vertex's child's target vertex.
            // all the other vertices should then be able to 
            foreach (var e in g.Edges.ToArray())
            {
                var label = e.Target.GetAnnotation<string>("reduction_label");
                var newVertex = vertices[label].First();
                var newEdge = new TaggedEdge<BaseDtVertexType, DtBranchTest>(e.Source, newVertex, e.Tag);
                g.RemoveEdge(e);
                g.AddEdge(newEdge);
            }
            foreach (var label in labels.Values)
            {
                foreach (var unwantedVertex in vertices[label].Skip(1))
                {
                    g.RemoveVertex(unwantedVertex);
                }
            }
            return g;
        }

        private void Dfs_LabelVertex(BaseDtVertexType vertex)
        {
            ComputeVertexLabel(vertex);
        }

        void ComputeVertexLabel(BaseDtVertexType v)
        {
            if (v is DtOutcome)
            {
                var tmp = v as DtOutcome;
                var k = (from o in symbolTable.DecisionMetadata.Outcomes.Values
                         where o.Name == tmp.OutcomeValue
                         select o.Id.ToString()).First();
                v.AddAnnotation("reduction_key", k);
                LabelVertex(v);
            }

            if (v is DtTest)
            {
                var tmp = v as DtTest;
                // by virtue of the the way the depth first search algorithm works
                // the sub-trees should have already been visited, and should thus be labelled
                var childLabels = (from c in graph.OutEdges(v)
                                   select c.Target.GetAnnotation("reduction_label")).ToArray();
                var key = string.Join(", ", childLabels);
                v.AddAnnotation("reduction_key", key);
                LabelVertex(v);
            }
        }

        void LabelVertex(BaseDtVertexType v)
        {
            var key = v.GetAnnotation<string>("reduction_key");
            if (string.IsNullOrWhiteSpace(key))
            {
                throw new DecisionException("cannot label a node without a key");
            }

            string label = null;
            // either an isomorphic subtree has already been labelled before, or this is a new subtree
            if (labels.ContainsKey(key))
            {
                // we've seen this kind of vertex before
                label = labels[key];
            }
            else
            {
                // never seena vertex with this key before
                label = (labelCounter++).ToString();
                labels[key] = label;
            }

            v.AddAnnotation("reduction_label", label);
            AddLabelledVertex(v, label);
        }

        private void AddLabelledVertex(BaseDtVertexType v, string label)
        {
            if (!vertices.ContainsKey(label))
            {
                vertices[label] = new List<BaseDtVertexType>();
            }

            vertices[label].Add(v);
        }

        private static void Dfs_DiscoverVertex(BaseDtVertexType vertex)
        {
            if (vertex is DtOutcome)
            {
                var x = vertex as DtOutcome;
                Debug.WriteLine($"O: {x.OutcomeValue}");
            }
            else if (vertex is DtTest)
            {
                var x = vertex as DtTest;
                Debug.WriteLine($"T: {x.Attribute.ToDebuggerString()}");
            }
            else
                Debug.WriteLine($"{vertex}");
        }
    }
}
