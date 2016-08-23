using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bdd
{
    public static class UtilityFunctions
    {
        public static string ConvertAttributeNameToColumnName(string name)
        {
            return name.Replace(' ', '_')
                .Replace('\'', '_')
                .Replace('\"', '_')
                .Replace('\t', '_');
        }
    }
}
