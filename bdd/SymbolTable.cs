using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bdd
{
    public class SymbolTable
    {
        private static int nextId = 0;

        Dictionary<string, int> symbols = new Dictionary<string, int>();
        public int DeclareVariable(string variableName)
        {
            if (symbols.ContainsKey(variableName))
            {
                throw new DecisionException("symbol already in use");
            }
            symbols[variableName] = nextId++;
            return nextId - 1;
        }

        public int? GetSymbolId(string name)
        {
            if (symbols.ContainsKey(name))
            {
                return symbols[name];
            }
            else
            {
                return null;
            }
        }
    }

}
