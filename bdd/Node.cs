using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bdd
{
    public interface IDecisionTree
    {
        int Evaluate();
    }

    public class DecisionTree : IDecisionTree
    {
        static int nodeIndex = 0;
        public int RootIndex { get; set; }
   
        private static void Assert(bool exp, string failureMessage = "assertion failed")
        {
            if (!exp)
            {
                throw new Exception(failureMessage);
            }
        }

        public DecisionTree(Environment env)
        {
            this.Environment = env;
        }
        Dictionary<int, Node> nodes = new Dictionary<int, Node>();

        public Environment Environment { get; private set; }

        public Node CreateNode(string v)
        {
            var symbolId = Environment.SymbolTable.GetSymbolId(v).Value;
            if (nodes.ContainsKey(symbolId))
            {
                throw new DecisionException("node already exists");
            }
            var id = GetNextNodeId();
            nodes[symbolId] = new Node(id, 0, 1, symbolId);
            return nodes[symbolId];
        }

        public Node CreateNode(string v, Node fail, Node pass)
        {
            var symbolId = Environment.SymbolTable.GetSymbolId(v).Value;
            var id = nodeIndex++;
            nodes[id] = new Node(id, fail.Id, pass.Id, symbolId);
            return nodes[id];
        }

        public Node CreateNode(string v, int fail, Node pass)
        {
            var symbolId = Environment.SymbolTable.GetSymbolId(v).Value;
            var id = GetNextNodeId();
            nodes[id] = new Node(id, fail, pass.Id, symbolId);
            return nodes[id];
        }

        public Node CreateNode(string v, Node fail, int pass)
        {
            var symbolId = Environment.SymbolTable.GetSymbolId(v).Value;
            var id = GetNextNodeId();
            nodes[id] = new Node(id, fail.Id, pass, symbolId);
            return nodes[id];
        }

        public Node CreateNode(string v, int fail, int pass)
        {
            var symbolId = Environment.SymbolTable.GetSymbolId(v).Value;
            var id = GetNextNodeId();
            nodes[id] = new Node(id, fail, pass, symbolId);
            return nodes[id];
        }

        public int GetNextNodeId()
        {
            return nodeIndex++;
        }

        public Node? GetNode(int v)
        {
            if (nodes.ContainsKey(v))
                return nodes[v];
            else
                return null;
        }

        public int Evaluate()
        {
            throw new NotImplementedException();
        }

        public Func<Dictionary<string, int>, int> Evaluator()
        {
            return Evaluator(RootIndex);
        }

        public Func<Dictionary<string, int>, int> Evaluator(int rootIndex)
        {
            var n = GetNode(rootIndex);
            if (!n.HasValue)
            {
                throw new ApplicationException("cannot create an evaluator for an empty decision tree");
            }
            var node = n.Value;
            return (env) => {
                throw new NotImplementedException();
                Environment environment = new Environment(env);
                var failed = Fail(environment.Resolve(node.SymbolId).Value);
                var path = failed ? node.Fail : node.Pass;

                if (IsTerminal(path)) return path;

            };
        }

        private bool Pass(int p)
        {
            return p == 1;
        }

        private bool Fail(int p)
        {
            return p == 0;
        }

        public bool IsTerminal(int nodeId)
        {
            return nodeId < 2;
        }
    }

    public struct Node
    {
        public Node(int id, int symbolId, int fail, int pass)
        {
            values = new int[4] { id, fail, pass, symbolId };
        }
        public Node(int id, int symbolId, Node fail, Node pass)
        {
            values = new int[4] { id, fail.Id, pass.Id, symbolId };
        }
        public Node(int id, int symbolId, int fail, Node pass)
        {
            values = new int[4] { id, fail, pass.Id, symbolId};
        }
        public Node(int id, int symbolId, Node fail, int pass)
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
        public int Fail
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
        public int Pass
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
