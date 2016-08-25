using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bdd
{
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
    }
}
