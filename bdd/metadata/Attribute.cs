using System.Collections.Generic;

namespace DecisionDiagrams
{
    public class Attribute
    {
        public string Name { get; set; }
        public string KindOfData { get; set; }
        public List<PossibleValue> PossibleValues { get; set; }
    }
}