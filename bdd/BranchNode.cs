using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics.Contracts;
using System.Threading.Tasks;
using System.Text;

namespace bdd
{

    public abstract class Node
    {
        public Node Parent { get; set; }

        public abstract bool IsRedundant();
        public string PrettyPrint()
        {
            return PrettyPrint(0);
        }
        public abstract string PrettyPrint(int indentLevel);

    }

    public class TerminalNode : Node
    {
        public object Result { get; set; }

        public override bool IsRedundant()
        {
            return false;
        }

        public override string PrettyPrint(int indentLevel)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < indentLevel; i++)
            {
                sb.Append("  ");
            }
            sb.Append($"T: {Result}\n");
            return sb.ToString();
        }
    }

    public class BranchNode : Node
    {
        /// <summary>
        /// construct a branch node with details about what symbol should be checked 
        /// and what paths different classifications of data will result in.
        /// </summary>
        /// <param name="symbol">what symbol the prevailing test data was classified on</param>
        /// <param name="ParentNode">the parent node if any out of which this node represents a branch</param>
        public BranchNode( SymbolTableEntry symbol, 
            Node parentNode)
        {
            this.Parent = parentNode;
            this.Symbol = symbol;
        }

        public int SymbolId { get { return Symbol.Id; } }
        public SymbolTableEntry Symbol { get; set; }
        /// <summary>
        /// collection of outgoing branches, each of which corresponds to some AttributeClassicationInstance
        /// </summary>
        Dictionary<AttributeClassicationInstance, Node> branches = new Dictionary<AttributeClassicationInstance, Node>();
        public Dictionary<AttributeClassicationInstance, Node> Branches
        {
            get { return branches; }
            set { branches = value; }
        }

        private Node node;
        public BranchNode(Node node)
        {
            this.node = node;
        }

        /// <summary>
        /// returns true if all branches point to the same node instance.
        /// </summary>
        /// <returns></returns>
        public override bool IsRedundant()
        {
            var first = branches.Values.First();
            return Enumerable.All(branches.Values.Skip(1), x => x.Equals(first));
        }

        public override string PrettyPrint(int indentLevel)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < indentLevel; i++)
            {
                sb.Append("  ");
            }

            sb.Append($"B: {Symbol.Name}\n");
            foreach (var b in branches)
            {
                var lbl = $"[{b.Key.ClassName}] -> {b.Value.PrettyPrint(++indentLevel)}";
                sb.Append(lbl);
            }
            return sb.ToString();
        }

    }
    public struct BranchNode_OLD
    {
        public BranchNode_OLD(BranchNode_OLD n) : this(n.Id, n.SymbolId, n.Lo, n.Hi)
        {
        }
        public BranchNode_OLD(int id, int symbolId, int fail, int pass)
        {
            values = new int[4] { id, fail, pass, symbolId };
        }
        public BranchNode_OLD(int id, int symbolId, BranchNode_OLD fail, BranchNode_OLD pass)
        {
            values = new int[4] { id, fail.Id, pass.Id, symbolId };
        }
        public BranchNode_OLD(int id, int symbolId, int fail, BranchNode_OLD pass)
        {
            values = new int[4] { id, fail, pass.Id, symbolId };
        }
        public BranchNode_OLD(int id, int symbolId, BranchNode_OLD fail, int pass)
        {
            values = new int[4] { id, fail.Id, pass, symbolId };
        }
        public int[] values { get; set; }
        public int Id
        {
            get
            {
                return values[0];
            }
            set
            {
                values[0] = value;
            }
        }
        public int Lo
        {
            get
            {
                return values[1];
            }
            set
            {
                values[1] = value;
            }
        }
        public int Hi
        {
            get
            {
                return values[2];
            }
            set
            {
                values[2] = value;
            }
        }
        public int SymbolId
        {
            get
            {
                return values[3];
            }
            set
            {
                values[3] = value;
            }
        }
    }
}
