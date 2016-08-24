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
}
