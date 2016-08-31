using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Modd.Metadata
{
    [DebuggerDisplay("{ToDebuggerString()}")]
    public class Attribute
    {
        public string Name { get; set; }
        public string KindOfData { get; set; }
        public List<PossibleValue> PossibleValues { get; set; }

        public string ToDebuggerString()
        {
            var vals = string.Join(", ", PossibleValues.Select(v => v.Value).ToArray());
            return $"Attr: {Name} ({vals})";
        }
    }
}