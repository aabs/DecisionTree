using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Modd.Metadata
{
    [DebuggerDisplay("{ToDebuggerString()}")]
    public class Attribute
    {
        public Attribute(string name, params string[] vals )
        {
            if (vals == null || vals.Length < 1)
                throw new System.ArgumentException(nameof(vals));
            if (name == null)
                throw new System.ArgumentNullException(nameof(name));
            Name = name;
            KindOfData = "Enumerated";
            PossibleValues = vals.Select(v => new PossibleValue { Value = v }).ToList();
        }
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