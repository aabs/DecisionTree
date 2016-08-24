using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bdd
{
    public class Decision
    {
        public string SampleDataLocation { get; set; }
        public Outcomes Outcomes { get; set; }
        public List<DecisionSpaceAttribute> Attributes { get; set; }
        public IEnumerable<DataRow> AllSamples { get; set; }
    }

    public class DecisionSpaceAttribute
    {
        public string Name { get; set; }
        public string DataType { get; set; }
        public List<DataClass> Classes { get; set; }
    }

    public class DataClass
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class Outcomes
    {
        public string OutcomeType { get; set; }
        public string OutcomeColumn { get; set; }
        public List<Outcome> Values { get; set; }
    }

    public class Outcome
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
