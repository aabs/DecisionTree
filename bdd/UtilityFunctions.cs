using System;
using System.Collections.Generic;
using System.Linq;

namespace Modd
{
    using System.Threading;
    using DT = DecisionTree<BaseDtVertexType, DtBranchTest>;

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
        {
            // degenerate case
            if (seq.Count() < 2)
                return true;
            var first = seq.First();
            return seq.Skip(1).All(x => x.Equals(first));
        }


        public static void Foreach<X>(this IEnumerable<X> seq, Action<X> task)
        {
            foreach (var item in seq)
            {
                task(item);
            }
        }
    }
}