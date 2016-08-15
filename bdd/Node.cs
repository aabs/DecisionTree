using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bdd
{


    public interface INodeTree
    {
        void Init(SymbolTable st);
        bool Evaluate(Environment env);
    }

    public struct Node
    {
        public int Id { get; set; }
        public int Fail { get; set; }
        public int Pass { get; set; }
    }
}
