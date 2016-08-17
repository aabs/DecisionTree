using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bdd
{
    struct BranchNodeAnalysis
    {

    }
    public static class DecisionTreeOptimiser
    {
        /// <summary>
        /// Remove redundant branch nodes from the decision tree supplied.
        /// </summary>
        /// <param name="dtIn"></param>
        /// <returns>optimised version of the tree passed in</returns>
        public static DecisionTree Reduce(DecisionTree dtIn)
        {
            var nodes = dtIn.Nodes.Values;
            var changesMade = true;
            while (changesMade)
            {
                changesMade = false;
                var redundantNodes = nodes.Where(n => n.Lo == n.Hi).ToList();
                foreach (var rbn in redundantNodes)
                {
                    RemoveNode(dtIn, rbn);
                    changesMade = true;
                }
            }
            return dtIn;
        }

        private static void RemoveNode(DecisionTree dtIn, BranchNode rbn)
        {
            foreach (var k in dtIn.Nodes.Keys)
            {
                var n = dtIn.Nodes[k];
                if (n.Lo == rbn.Id)
                {
                    n.Lo = rbn.Lo;
                }
                if (n.Hi == rbn.Id)
                {
                    n.Hi = rbn.Hi;
                }
            }
            dtIn.Nodes.Remove(rbn.Id);
        }
    }
}
