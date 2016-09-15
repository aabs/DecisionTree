using QuickGraph;
using QuickGraph.Algorithms.Search;
using System;
using System.Linq;

namespace Modd
{
    using EdgeType = QuickGraph.TaggedEdge<BaseDtVertexType, DtBranchTest>;
    using GraphType = QuickGraph.AdjacencyGraph<BaseDtVertexType, QuickGraph.TaggedEdge<BaseDtVertexType, DtBranchTest>>;

    public class Reducer
    {
        public RedundancyTracker _tracker = new RedundancyTracker();
        private GraphType graph;
        private SymbolTable symbolTable;
        private int labelCounter;
        private Action<GraphType> graphValidator;

        public Reducer(Action<GraphType> validator)
        {
            this.graphValidator = validator;
        }

        public GraphType Reduce(SymbolTable st, GraphType g)
        {
            symbolTable = st;
            graph = g;
            _tracker.ResetTracking(graph);
            //do the search
            LabelVertices();
            PruneRedundantVertices();

            if (_tracker.LabelsRequireRecalculation)
            {
                _tracker.ResetTracking(graph);
                LabelVertices();
            }

            var labels = _tracker.Labels.ToList();
            foreach (var label in labels)
            {
                var unwantedVerticesUnderReductionLabel = _tracker[label].Skip(1).ToList();
                if (unwantedVerticesUnderReductionLabel.Count == 0)
                {
                    continue; // no unwanted vertices under this label
                }
                var preferredVertex = GetPreferredVertex(unwantedVerticesUnderReductionLabel.FirstOrDefault());
                foreach (var unwantedVertex in unwantedVerticesUnderReductionLabel)
                {
                    ReplaceRedundantVertexWithCanonicalInstance(graph, unwantedVertex, preferredVertex);
                    _tracker.StopTracking(unwantedVertex);
                    g.RemoveVertex(unwantedVertex);
                }
            }
            return g;
        }

        private BaseDtVertexType GetPreferredVertex(BaseDtVertexType unwantedVertex)
        {
            return _tracker[unwantedVertex.ReductionLabel()].First();
        }

        public void LabelVertices()
        {
            var dfs = new DepthFirstSearchAlgorithm<BaseDtVertexType, TaggedEdge<BaseDtVertexType, DtBranchTest>>(graph);
            dfs.FinishVertex += (v) => { ComputeVertexLabel(v); };
            dfs.Compute();
        }

        public void PruneRedundantVertices()
        {
            var dfs = new DepthFirstSearchAlgorithm<BaseDtVertexType, TaggedEdge<BaseDtVertexType, DtBranchTest>>(graph);
            dfs.FinishVertex += Dfs_PruneRedundantNodes;
            dfs.Compute();
        }

        /// <summary>
        /// Remove the vertex if it is redundant
        /// </summary>
        /// <param name="vertex">The vertex to be tested for redundancy, and pruned if found to be redundant.</param>
        /// <remarks>
        /// Redundancy comes in the following forms:
        /// <list type="bullet">
        /// <item>vertexes with all branches resulting in the same outcome</item>
        /// <item>vertexes that have the same ordered set of outcomes as some previously seen vertex</item>
        /// </list>
        /// </remarks>
        private void Dfs_PruneRedundantNodes(BaseDtVertexType vertex)
        {
            // Assumptions:
            //   - Tree is normalised at the commencement of the process,
            //     implying that all branch test outcomes are covered in
            //     the out edges of a branch test.
            var r_key = vertex.ReductionKey();
            var subkeys = r_key.Split(',');
            if (subkeys.Length > 1)
            {
                if (subkeys.Select(s => s.Trim()).Same())
                {
                    CollapseTautologicalTestBranch(vertex);
                }
            }
        }

        /// <summary>
        /// Remove a vertex from the tree.
        /// </summary>
        /// <param name="vertex"></param>
        /// <example>
        ///    +---+                                   +---+                          +---+
        ///    | ? |                                   | ? |                          | X |
        ///    +-+-+                                   +-+-+                          +---+
        ///      |                                       |
        ///      |   +---+                               |    +---+
        ///      +---+ X |                               +----+ X |
        ///      |   +---+                               |    +---+
        ///      |                                       |
        ///      |   +---+                               |    +---+
        ///      +---+ X |                               +----+ X |
        ///      |   +---+                 +------->     |    +---+      +------>
        ///      |                                       |
        ///      |   +---+                               |    +---+
        ///      +---+ ? |                               +----+ X |
        ///          +-+-+                                    +---+
        ///            |
        ///            |     +---+
        ///            +-----+ X |
        ///            |     +---+
        ///            |
        ///            |     +---+
        ///            +-----+ X |
        ///            |     +---+
        ///            |
        ///            |     +---+
        ///            +-----+ X |
        ///                  +---+
        /// </example>
        private void CollapseTautologicalTestBranch(BaseDtVertexType vertex)
        {
            var children = graph.OutEdges(vertex);

            if (!children.All(e => e.Target is DtOutcome))
            {
                throw new DecisionException("Can only prune subtree with leaf children");
            }

            // we replace with the canonical instance labelled the same as the first child.
            // that way, any other label based pruning will not result in the new vertex being remove accidentally.
            var labelOfPreferredVertex = graph.OutEdge(vertex, 0).Target.ReductionLabel();
            var preferredVertex = _tracker[labelOfPreferredVertex].First();

            // replace self with one of the children
            ReplaceRedundantVertexWithCanonicalInstance(graph, vertex, preferredVertex);

            // now relabel the parent of the vertex.
            graph.Parents(preferredVertex).Foreach(v => ComputeVertexLabel(v));
        }

        public void ReplaceRedundantVertexWithCanonicalInstance(GraphType self, BaseDtVertexType v_old, BaseDtVertexType v_new)
        {
            if (v_new == null)
            {
                return;
            }
            // add new vertex, if it is not already in the vertices collection
            if (!self.ContainsVertex(v_new))
            {
                self.AddVertex(v_new);
            }

            // redirect InEdges to the new vertex
            var in_edges = self.InEdges(v_old).ToList();
            self.AddEdgeRange(in_edges.Select(e => new EdgeType(e.Source, v_new, e.Tag)));
            in_edges.Foreach(e => self.RemoveEdge(e));

            // if the vertex is an outcome it won't have any children so there's no need to prune its children
            if (v_old is DtOutcome) return;

            // now dispose of any unwanted children, taking into account that the new
            // vertex may be one of the children, and thus should be kept.
            var unwanted_children = self.OutEdges(v_old)
                .ToList();
            unwanted_children.ForEach(v => self.RemoveEdge(v));
        }

        /// <summary>
        /// Compute the key for a vertex and use that to look up (or compute) the label to
        /// apply to the vertex <paramref name="v"/>. Allows detection of tree isomorphisms
        /// (and therefore potential reductions)
        /// </summary>
        /// <param name="v">The vertex to be inspected and labelled</param>
        /// <remarks>
        /// This function will coordinate the process of computing keys for the </remarks>
        private void ComputeVertexLabel(BaseDtVertexType v)
        {
            // if the node is a simple outcome node, then there will be no children
            // in that case it is safe to assign it a simple key based on the outcome Id
            // since all identical outcomes have the same ID, they will also have the
            // same reduction key, hence the base case is covered (the proper labelling
            // of leaf nodes)
            if (v is DtOutcome)
            {
                var tmp = v as DtOutcome;
                var k = (from o in symbolTable.DecisionMetadata.Outcomes.Values
                         where o.Name == tmp.OutcomeValue
                         select o.Id.ToString()).First();
                v.AddAnnotation(Constants.ReductionKey, k);
                LabelVertex(v);
            }

            //  if the node is not an outcome, it must be a test, and thus by definition must
            // have children (that are either outcomes or more tests).  In that case the reduction
            // key is formed by joining the reduction keys of the sub-vertices.  This allows the
            // formation of unique keys for unique combinations of child nodes.
            if (v is DtTest)
            {
                var tmp = v as DtTest;
                // by virtue of the the way the depth first search algorithm works
                // the sub-trees should have already been visited, and should thus be labelled
                var childLabels = (from c in graph.OutEdges(v)
                                   select c.Target.ReductionLabel()).ToArray();
                var key = string.Join(", ", childLabels);
                v.AddAnnotation(Constants.ReductionKey, $"{tmp.Attribute.InstanceIdentifier}({key})");

                // produce a label for the node.  At this point, the vertex has a label, but it needs to
                // be given the canonical label to allow all identical vertices to be redirected to the
                // first instance of this reduction key.
                LabelVertex(v);
            }
        }

        /// <summary>
        /// Assign the label of the vertex <paramref name="v"/> and ensure that the label is common
        /// to all isomorphic vertices.  In this case, that means all vertices that have the same <c>reduction_key</c>
        /// </summary>
        /// <param name="v">The vertex to be labelled</param>
        private void LabelVertex(BaseDtVertexType v)
        {
            // first get the key for the vertex and ensure it has been computed already.
            var key = v.ReductionKey();
            if (string.IsNullOrWhiteSpace(key))
            {
                throw new DecisionException("cannot label a node without a key");
            }

            // next, either get the label if this reduction_key has been seen before, or create a new one.
            string label = null;
            // either an isomorphic subtree has already been labelled before, or this is a new subtree
            if (_tracker.TryGetLabel(key, ref label))
            {
                // we've seen this kind of vertex before
                label = _tracker.GetLabel(key);
            }
            else
            {
                // never seen a vertex with this key before
                label = (++labelCounter).ToString();
                _tracker.SetLabel(key, label);
            }

            // now annotate the object, and add it to the list of vertices labelled under that reduction_label.
            v.AddAnnotation(Constants.ReductionLabel, label);
            _tracker.TrackVertex(v);
        }
    }
}