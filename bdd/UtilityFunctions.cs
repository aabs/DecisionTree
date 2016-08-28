using System;
using System.Collections.Generic;
using System.Linq;

namespace DecisionDiagrams
{
    using System.Threading;
    using DT = DecisionTree<BaseDtVertexType, DtBranchTest>;
    using TE = Edge<BaseDtVertexType, DtBranchTest>;
    using TV = Vertex<BaseDtVertexType, DtBranchTest>;

    public static class UtilityFunctions
    {
        public static string ConvertAttributeNameToColumnName_Ignore(string name)
        {
            return name.Replace(' ', '_')
                .Replace('\'', '_')
                .Replace('\"', '_')
                .Replace('\t', '_');
        }

        public static bool AllIdentical<T>(this IEnumerable<T> seq)
            where T : IEquatable<T>
        {
            // degenerate case
            if (seq.Count() < 2)
                return true;
            var first = seq.First();
            return seq.Skip(1).All(x => x.Equals(first));
        }

        /// <summary>
        /// Walk the graph visiting each vertex only once
        /// </summary>
        /// <typeparam name="TVertexType">Vertex types</typeparam>
        /// <typeparam name="TEdgeLabelType">Edge types</typeparam>
        /// <param name="v">the vertex of the graph to process</param>
        /// <remarks>
        /// procedure Traverse (v:vertex); 
        /// begin 
        ///     v.mark := not v.mark; 
        ///     ... do something to v ... 
        ///     if v.index ≤ n
        ///     then begin {v nonterminal} 
        ///         if v.mark ≠ v.low.mark then Traverse (v.low); 
        ///         if v.mark ≠ v.high.mark then Traverse (v.high); 
        ///     end; 
        /// end;
        /// 
        /// This was documented in: 
        /// Bryant, R. E. (1986). Graph-Based Algorithms for Boolean Function Manipulation. 
        /// IEEE Transactions on Computers. http://doi.org/10.1109/TC.1986.1676819
        /// 
        /// </remarks>
        public static void Traverse<TVertexType, TEdgeLabelType>(this Vertex<TVertexType, TEdgeLabelType> v, 
            Action<Vertex<TVertexType, TEdgeLabelType>> proc
        )
           where TVertexType : IEquatable<TVertexType>
        where TEdgeLabelType : IEquatable<TEdgeLabelType>
        {
            // multiple traversals can take place, but they MUST be on different threads...
            var mark_label = Thread.CurrentThread.GetHashCode().ToString();
            bool mark = (bool)(v.GetAnnotation(mark_label) ?? false);
            v.AddAnnotate(mark_label, !mark);
            proc(v);
            foreach (var c in v.Children)
            {
                if (mark != c.TargetVertex.GetAnnotation<bool>(mark_label))
                {
                    Traverse(c.TargetVertex, proc);
                }
            }
        }
    }
}