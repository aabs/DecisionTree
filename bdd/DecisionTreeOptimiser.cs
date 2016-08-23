using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bdd
{
    interface IVisitor
    {
        void Visit(TerminalNode n);
        bool StartVisit(BranchNode n);
        void EndVisit(BranchNode n);
    }
    class Dispatcher
    {
        public static void Accept<TV>(TerminalNode n, TV visitor)
          where TV : IVisitor
        {
            visitor.Visit(n);
        }
        public static void Accept<TV>(BranchNode n, TV visitor)
          where TV : IVisitor
        {
            if (visitor.StartVisit(n))
            {
                foreach (var subpart in n.Branches.Values)
                {
                    if (subpart is TerminalNode)
                    {
                        Accept(subpart as TerminalNode, visitor);
                    }
                    else if (subpart is BranchNode)
                    {
                        Accept(subpart as BranchNode, visitor);
                    }
                }
                visitor.EndVisit(n);
            }
        }
    }

    class BaseVisitor : IVisitor
    {
        public DecisionTree DT { get; set; }
        public BaseVisitor(DecisionTree dt)
        {
            this.DT = dt;
        }
        public virtual void Visit(TerminalNode n) { }
        public virtual bool StartVisit(BranchNode n)
        {
            return true;
        }
        public virtual void EndVisit(BranchNode n) { }
    }

    class TreePrunerVisitor : BaseVisitor
    {
        public TreePrunerVisitor(DecisionTree dt) : base(dt)
        {
        }
        public override void Visit(TerminalNode n)
        {
            Debug.WriteLine("p2");
        }
        public override bool StartVisit(BranchNode n)
        {
            //  first try to remove this BN if it is redundant
            var b = n.Branches.Select(x => x.Value);
            if (b.Count() == 1 && b.First() is TerminalNode)
            {
                // there is only one outcome and it is terminal, so replace this node with the terminal node.
                if (n.Parent == null)
                {
                    // this must be the root node of the DT, so replace it
                    DT.TreeRoot = b.First();
                    n.Branches = null;
                    n.Symbol = null;
                    // no need to continue with the visit any more, since the subtree has been eradicated.
                    return false;
                }
                else
                {
                    n.Parent = b.First();
                    n.Branches = null;
                    n.Symbol = null;
                    // no need to continue with the visit any more, since the subtree has been eradicated.
                    return false;
                }
            }
            if (Enumerable.All(b.Skip(1), x => x.Equals(b.First())))
            {
                // TODO: remove the node
                // do something cool here...
            }
            return true;
        }

        /// <summary>
        /// Remove a node from the tree by linking it's parent(s) to its children
        /// </summary>
        private void CollapseNode(Node n)
        {

        }
        public override void EndVisit(BranchNode n)
        {
            Debug.WriteLine("<-c");
        }
    }



    public static class DecisionTreePruner
    {
        public static DecisionTree PruneTree(DecisionTree dtIn)
        {
            //return new DecisionTree(Prune(dtIn.TreeRoot));
            throw new NotImplementedException();
        }

        private static Node Prune(Node node)
        {
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
