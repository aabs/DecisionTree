using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bdd
{
    public class DecisionTree
    {
        static int nextNodeId = 2;
        int rootIndex;

        public int RootIndex
        {
            get
            {
                return rootIndex;
            }

            set
            {
                if (IsTerminal(value))
                {
                    throw new ArgumentOutOfRangeException("cannot set root node ID to be a result type");
                }
                if (!nodes.ContainsKey(value))
                {
                    throw new ArgumentOutOfRangeException("missing node");
                }
                rootIndex = value;
            }
        }

        private static void Assert(bool exp, string failureMessage = "assertion failed")
        {
            if (!exp)
            {
                throw new Exception(failureMessage);
            }
        }

        public DecisionTree(Environment env)
        {
            if (env == null)
            {
                throw new ArgumentNullException(nameof(env));
            }
            this.Environment = env;
        }
        Dictionary<int, Node> nodes = new Dictionary<int, Node>();

        public Environment Environment { get; private set; }

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

        public Node CreateNode(string v)
        {
            TestVariableNameValidity(v);
            var symbolId = Environment.SymbolTable.GetSymbolId(v).Value;
            if (nodes.ContainsKey(symbolId))
            {
                throw new DecisionException("node already exists");
            }
            var id = GetNextNodeId();
            nodes[id] = new Node(id, symbolId, 0, 1);
            return nodes[id];
        }

        public Node CreateNode(string v, Node fail, Node pass)
        {
            TestVariableNameValidity(v);
            var symbolId = Environment.SymbolTable.GetSymbolId(v).Value;
            var id = nextNodeId++;
            nodes[id] = new Node(id, symbolId, fail.Id, pass.Id);
            return nodes[id];
        }

        public Node CreateNode(string v, int fail, Node pass)
        {
            TestVariableNameValidity(v);
            var symbolId = Environment.SymbolTable.GetSymbolId(v).Value;
            var id = GetNextNodeId();
            nodes[id] = new Node(id, symbolId, fail, pass.Id);
            return nodes[id];
        }

        public Node CreateNode(string v, Node fail, int pass)
        {
            TestVariableNameValidity(v);
            var symbolId = Environment.SymbolTable.GetSymbolId(v).Value;
            var id = GetNextNodeId();
            nodes[id] = new Node(id, symbolId, fail.Id, pass);
            return nodes[id];
        }

        public Node CreateNode(string v, int fail, int pass)
        {
            TestVariableNameValidity(v);
            var symbolId = Environment.SymbolTable.GetSymbolId(v).Value;
            var id = GetNextNodeId();
            nodes[id] = new Node(id, symbolId, fail, pass);
            return nodes[id];
        }

        public int GetNextNodeId()
        {
            return nextNodeId++;
        }

        public Node? GetNode(int v)
        {
            if (nodes.ContainsKey(v))
                return nodes[v];
            return null;
        }

        public int Evaluate(int nodeIndex)
        {
            if (nodes.Count == 0)
            {
                throw new ApplicationException("cannot create an evaluator for an empty decision tree");
            }
            var n = GetNode(nodeIndex);
            if (!n.HasValue)
            {
                throw new ApplicationException("cannot create an evaluator for an empty decision tree");
            }
            var node = n.Value;
            var variableValue = Environment.Resolve(node.SymbolId).Value;
            var path = variableValue == 0 ? node.Fail : node.Pass;

            if (IsTerminal(path)) return path;
            return Evaluate(path);
        }

        public int Evaluate()
        {
            return Evaluate(RootIndex);
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
