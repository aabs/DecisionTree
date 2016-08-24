using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics.Contracts;
using System.Threading.Tasks;
using System.Text;

namespace bdd
{
    public abstract class BaseDtVertexType : IEquatable<BaseDtVertexType>
    {
        public bool Equals(BaseDtVertexType other) { return false; }
    }

    public class DtTest : BaseDtVertexType, IEquatable<DtTest>
    {
        public DtTest(DecisionSpaceAttribute attribute)
        {
            this.Attribute = attribute;
        }
        public DecisionSpaceAttribute Attribute { get; set; }

        public new bool Equals(BaseDtVertexType other)
        {
            return Equals(other as DtTest);
        }

        public bool Equals(DtTest other)
        {
            return Attribute.Equals(other.Attribute);
        }
    }

    public class DtOutcome : BaseDtVertexType, IEquatable<DtOutcome>
    {
        public DtOutcome(string outcome)
        {
            this.OutcomeValue = outcome;
        }
        public string OutcomeValue { get; set; }
        public new bool Equals(BaseDtVertexType other)
        {
            return Equals(other as DtTest);
        }

        public bool Equals(DtOutcome other)
        {
            return OutcomeValue.Equals(other.OutcomeValue);
        }
    }

    public class DtBranchTest : IEquatable<DtBranchTest>
    {
        public AttributePermissibleValue TestValue { get; set; }
        public DtBranchTest(AttributePermissibleValue testValue)
        {
            this.TestValue = testValue;
        }
        public bool Equals(DtBranchTest other)
        {
            return TestValue.Equals(other.TestValue);
        }
    }

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
        Dictionary<AttributePermissibleValue, Node> branches = new Dictionary<AttributePermissibleValue, Node>();
        public Dictionary<AttributePermissibleValue, Node> Branches
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
}
