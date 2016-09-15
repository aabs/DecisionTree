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

        public static bool Same<T>(this IEnumerable<T> seq)
            where T : IEquatable<T>
        {
            if (seq.Count() < 2)
                return true;
            var first = seq.First();
            return seq.Skip(1).All(x => x.Equals(first));
        }

        public static bool AllIdentical<T>(this IEnumerable<T> seq)
            where T : IEquatable<T> => seq.Same();


        public static void Foreach<X>(this IEnumerable<X> seq, Action<X> task)
        {
            foreach (var item in seq)
            {
                task(item);
            }
        }

        public static string ReductionKey(this BaseDtVertexType self)
            => self.GetAnnotation<string>(Constants.ReductionKey);
        public static string ReductionLabel(this BaseDtVertexType self)
            => self.GetAnnotation<string>(Constants.ReductionLabel);
    }
}