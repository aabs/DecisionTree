using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics.Contracts;
using System.Threading.Tasks;
using System.Text;

namespace bdd
{
    public struct BranchNode
    {
        public BranchNode(int id, int symbolId, int fail, int pass)
        {
            values = new int[4] { id, fail, pass, symbolId };
        }
        public BranchNode(int id, int symbolId, BranchNode fail, BranchNode pass)
        {
            values = new int[4] { id, fail.Id, pass.Id, symbolId };
        }
        public BranchNode(int id, int symbolId, int fail, BranchNode pass)
        {
            values = new int[4] { id, fail, pass.Id, symbolId};
        }
        public BranchNode(int id, int symbolId, BranchNode fail, int pass)
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
