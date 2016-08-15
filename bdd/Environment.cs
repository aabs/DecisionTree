using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bdd
{
    public class Environment
    {
        public Environment ParentEnvironment { get; set; }
        Dictionary<int, int> boundVariables = new Dictionary<int, int>();
        public int? Resolve(int symbolId)
        {
            if (boundVariables.ContainsKey(symbolId))
            {
                return boundVariables[symbolId];
            }
            else if (ParentEnvironment != null)
            {
                return ParentEnvironment.Resolve(symbolId);
            }
            else
            {
                return null;
            }
        }
    }
}
