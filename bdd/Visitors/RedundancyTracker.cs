using QuickGraph;
using QuickGraph.Algorithms.Search;
using System.Collections.Generic;
using System.Linq;

namespace Modd
{
    using GraphType = QuickGraph.AdjacencyGraph<BaseDtVertexType, QuickGraph.TaggedEdge<BaseDtVertexType, DtBranchTest>>;

    public class RedundancyTracker
    {
        private Dictionary<string, string> mapKeyToLabel = new Dictionary<string, string>();
        private Dictionary<string, List<BaseDtVertexType>> mapLabelToVertices = new Dictionary<string, List<BaseDtVertexType>>();

        public void ResetTracking(GraphType g)
        {
            // if there are no key --> label mapping then the annotations on the graph are worthless and should be purged,
            // as should all the vertices being tracked against the reduction labels
            PurgeReductionAnnotationsFromVertices(g);
            mapKeyToLabel.Clear();
            mapLabelToVertices.Clear();
        }

        void PurgeReductionAnnotationsFromVertices(GraphType g)
        {
            var dfs = new DepthFirstSearchAlgorithm<BaseDtVertexType, TaggedEdge<BaseDtVertexType, DtBranchTest>>(g);
            dfs.FinishVertex += (v) =>
            {
                v.DeleteAnnotation(Constants.ReductionKey);
                v.DeleteAnnotation(Constants.ReductionLabel);
            };
            dfs.Compute();
        }

        public bool LabelsRequireRecalculation { get; set; }

        public IEnumerable<string> Labels
            => mapKeyToLabel.Values.OrderBy(s => s);

        public bool TryGetLabel(string key, ref string label)
        {
            if (mapKeyToLabel.ContainsKey(key))
            {
                label = mapKeyToLabel[key];
                return true;
            }
            return false;
        }

        public string GetLabel(string key)
            => mapKeyToLabel[key];

        public void SetLabel(string key, string value)
            => mapKeyToLabel[key] = value;

        public List<BaseDtVertexType> this[string reductionLabel]
            => mapLabelToVertices[reductionLabel];

        public void TrackVertex(BaseDtVertexType v)
        {
            var key = v.ReductionKey();
            var label = v.ReductionLabel();

            if (!mapKeyToLabel.ContainsKey(key))
            {
                mapKeyToLabel[key] = label;
            }

            if (!mapLabelToVertices.ContainsKey(label))
            {
                mapLabelToVertices[label] = new List<BaseDtVertexType>();
            }

            mapLabelToVertices[label].Add(v);
        }

        public void StopTracking(BaseDtVertexType v)
        {
            var rl = v.ReductionLabel();
            if (mapLabelToVertices.ContainsKey(rl))
            {
                var l = mapLabelToVertices[rl];
                l.Remove(v);
                // if this was the last of its type, then clean up tracking maps
                if (l.Count == 0)
                {
                    mapLabelToVertices.Remove(rl);
                    // since the label is no more, so should the tracking on the key be
                    mapKeyToLabel.Remove(v.ReductionKey());
                }
            }
        }
    }
}