using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bdd
{
    public class DecisionTree
    {
        public Node TreeRoot { get; set; }

        public Environment Environment { get; private set; }

        public DecisionTree(DecisionTree dt)
        {
            // copy across references to the environment, but duplicate everything else
            this.Environment = dt.Environment;
            TreeRoot = dt.TreeRoot;
        }

        public DecisionTree(Environment env)
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
            var symbolId = Environment.SymbolTable.GetSymbolId(v).Value;
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
            var symbolId = Environment.SymbolTable.GetSymbolId(v).Value;
            var id = nextNodeId++;
            Nodes[id] = new BranchNode_OLD(id, symbolId, fail.Id, pass.Id);
            return Nodes[id];
        }

        public BranchNode_OLD CreateNode(string v, int fail, BranchNode_OLD pass)
        {
            TestVariableNameValidity(v);
            var symbolId = Environment.SymbolTable.GetSymbolId(v).Value;
            var id = GetNextNodeId();
            Nodes[id] = new BranchNode_OLD(id, symbolId, fail, pass.Id);
            return Nodes[id];
        }

        public BranchNode_OLD CreateNode(string v, BranchNode_OLD fail, int pass)
        {
            TestVariableNameValidity(v);
            var symbolId = Environment.SymbolTable.GetSymbolId(v).Value;
            var id = GetNextNodeId();
            Nodes[id] = new BranchNode_OLD(id, symbolId, fail.Id, pass);
            return Nodes[id];
        }

        public BranchNode_OLD CreateNode(string v, int fail, int pass)
        {
            TestVariableNameValidity(v);
            var symbolId = Environment.SymbolTable.GetSymbolId(v).Value;
            var id = GetNextNodeId();
            Nodes[id] = new BranchNode_OLD(id, symbolId, fail, pass);
            return Nodes[id];
        }
        */

        public int Evaluate(Node node)
        {
            throw new NotImplementedException();
            //var variableValue = Environment.Resolve(node.SymbolId).Value;
            //var path = variableValue == 0 ? node.Lo : node.Hi;

            //if (IsTerminal(path)) return path;
            //return Evaluate(path);
        }

        public int Evaluate()
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
