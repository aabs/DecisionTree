using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bdd
{
    public class DecisionTree<TNodeType, TTestType>
        where TNodeType : IEquatable<TNodeType>
        where TTestType : IEquatable<TTestType>
    {
        public Graph<TNodeType, TTestType> Tree { get; set; }
    }
    public class DecisionTree__OLD
    {
        public Node TreeRoot { get; set; }

        public Environment Environment { get; set; }

        public DecisionTree__OLD()
        {

        }
        public DecisionTree__OLD(DecisionTree__OLD dt)
        {
            // copy across references to the environment, but duplicate everything else
            this.Environment = dt.Environment;
            TreeRoot = dt.TreeRoot;
        }

        public DecisionTree__OLD(Environment env)
        {
            if (env == null)
            {
                throw new ArgumentNullException(nameof(env));
            }
            this.Environment = env;
        }

        static void TestVariableNameValidity(string v)
        {
            if (string.IsNullOrWhiteSpace(v) || Enumerable.Any(v.ToArray(), c => c == 0))
            {
                throw new ArgumentException("invalid variable name");
            }
            if (!Enumerable.All(v.ToArray(), c => c < 128))
            {
                throw new ArgumentException("Invalid variable name. ASCII chars only.");
            }
        }

/*
 *        public BranchNode_OLD CreateNode(string v)
        {
            TestVariableNameValidity(v);
            var symbolId = Environment.SymbolTable.GetSymbolId(v).Content;
            if (Nodes.ContainsKey(symbolId))
            {
                throw new DecisionException("node already exists");
            }
            var id = GetNextNodeId();
            Nodes[id] = new BranchNode_OLD(id, symbolId, 0, 1);
            return Nodes[id];
        }

        public BranchNode_OLD CreateNode(string v, BranchNode_OLD fail, BranchNode_OLD pass)
        {
            TestVariableNameValidity(v);
            var symbolId = Environment.SymbolTable.GetSymbolId(v).Content;
            var id = nextNodeId++;
            Nodes[id] = new BranchNode_OLD(id, symbolId, fail.Id, pass.Id);
            return Nodes[id];
        }

        public BranchNode_OLD CreateNode(string v, int fail, BranchNode_OLD pass)
        {
            TestVariableNameValidity(v);
            var symbolId = Environment.SymbolTable.GetSymbolId(v).Content;
            var id = GetNextNodeId();
            Nodes[id] = new BranchNode_OLD(id, symbolId, fail, pass.Id);
            return Nodes[id];
        }

        public BranchNode_OLD CreateNode(string v, BranchNode_OLD fail, int pass)
        {
            TestVariableNameValidity(v);
            var symbolId = Environment.SymbolTable.GetSymbolId(v).Content;
            var id = GetNextNodeId();
            Nodes[id] = new BranchNode_OLD(id, symbolId, fail.Id, pass);
            return Nodes[id];
        }

        public BranchNode_OLD CreateNode(string v, int fail, int pass)
        {
            TestVariableNameValidity(v);
            var symbolId = Environment.SymbolTable.GetSymbolId(v).Content;
            var id = GetNextNodeId();
            Nodes[id] = new BranchNode_OLD(id, symbolId, fail, pass);
            return Nodes[id];
        }
        */

        public string Evaluate(Node node)
        {
            if (node is TerminalNode)
            {
                var x = node as TerminalNode;
                return x.Result as string;
            }
            var bn = node as BranchNode;
            //throw new NotImplementedException();
            var variableValue = Environment.Resolve(bn.SymbolId);
            foreach (var b in bn.Branches)
            {
                if ((string)b.Key.Value == variableValue)
                {
                    return Evaluate(b.Value);
                }
            }
            throw new DecisionException("unrecognised path in tree");
        }

        public string Evaluate()
        {
            return Evaluate(TreeRoot);
        }

        bool Pass(int p)
        {
            return p == 1;
        }

        bool Fail(int p)
        {
            return p == 0;
        }

        public bool IsTerminal(int nodeId)
        {
            return nodeId < 2;
        }
    }
}
