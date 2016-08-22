using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bdd
{
    public static class DecisionTreePruner
    {
        public static DecisionTree PruneTree(DecisionTree dtIn)
        {
            //return new DecisionTree(Prune(dtIn.TreeRoot));
            throw new NotImplementedException();
        }

        private static Node Prune(Node node)
        {
            //if (node is BranchNode && node.IsRedundant())
            //{
            //    var bn = node as BranchNode;
            //    var parent = bn.Parent as BranchNode;
            //    foreach (var item in parent.)
            //    {

            //    }
            //}
            throw new NotImplementedException();
        }
    }

    //public static class DecisionTreeOptimiser_OLD
    //{
    //    /// <summary>
    //    /// Remove redundant branch nodes from the decision tree supplied.
    //    /// </summary>
    //    /// <param name="dtIn"></param>
    //    /// <returns>optimised version of the tree passed in</returns>
    //    public static DecisionTree Reduce(DecisionTree dtIn)
    //    {
    //        var result = new DecisionTree(dtIn);
    //        var nodes = result.Nodes.Values;
    //        var changesMade = true;
    //        while (changesMade)
    //        {
    //            changesMade = false;
    //            var redundantNodes = nodes.Where(n => n.Lo == n.Hi).ToList();
    //            foreach (var rbn in redundantNodes)
    //            {
    //                RemoveNode(result, rbn);
    //                changesMade = true;
    //            }
    //        }
    //        return result;
    //    }

    //    private static void RemoveNode(DecisionTree dtIn, BranchNode_OLD rbn)
    //    {
    //        foreach (var k in dtIn.Nodes.Keys)
    //        {
    //            var n = dtIn.Nodes[k];
    //            if (n.Lo == rbn.Id)
    //            {
    //                n.Lo = rbn.Lo;
    //            }
    //            if (n.Hi == rbn.Id)
    //            {
    //                n.Hi = rbn.Hi;
    //            }
    //        }
    //        dtIn.Nodes.Remove(rbn.Id);
    //    }
    //}
}
