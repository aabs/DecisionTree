using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bdd
{
    public class Environment
    {
        public Environment(Dictionary<string, string> args)
        {
            if (args == null)
            {
                throw new ArgumentNullException(nameof(args));
            }
            this.SymbolTable = new SymbolTable();
            this.ParentEnvironment = null;
            foreach (var nvp in args)
            {
                Bind(nvp.Key, nvp.Value);
            }
        }
        public Environment(SymbolTable st, Environment parent = null, bool singleAssignment = false)
        {
            if (st == null)
            {
                throw new ArgumentNullException(nameof(st));
            }
            this.SymbolTable = st;
            this.ParentEnvironment = parent;
            this.SingleAssignment = singleAssignment;
        }
        public Environment ParentEnvironment { get; set; }
        public SymbolTable SymbolTable { get; private set; }
        public bool SingleAssignment { get; private set; }

        Dictionary<int, string> boundVariables = new Dictionary<int, string>();
        public string Resolve(int symbolId)
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

        public string Resolve(string variableName)
        {
            var id = SymbolTable.GetSymbolId(variableName);
            if (id.HasValue)
            {
                return Resolve(id.Value);
            }
            throw new DecisionException("Unresolved symbol name");
        }

        public void Bind(string variableName, string value)
        {
            var id = SymbolTable.GetSymbolId(variableName);
            if (!id.HasValue)
            {
                throw new DecisionException("Unresolved symbol name");
            }
            Bind(id.Value, value);
        }

        public void Bind(int variableId, string value)
        {
            if (SingleAssignment)
            {
                if (boundVariables.ContainsKey(variableId))
                {
                    throw new DecisionException("Cannot assign to bound variable");
                }
            }
            /// TODO: Add the symbol to the ysmbol table if not present
            boundVariables[variableId] = value;
        }
    }
}
