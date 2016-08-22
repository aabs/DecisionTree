using Nortal.Utilities.Csv;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace bdd
{
    public class TreeBuilder
    {

        public TreeBuilder(string metadataFileLocation)
        {
            XDocument doc = XDocument.Load(metadataFileLocation);
            this.Config = LoadConfig(doc.Document.Root);
        }

        #region Configuration Loading
        public Decision Config { get; private set; }

        private Decision LoadConfig(XElement root)
        {
            var result = new Decision
            {
                SampleDataLocation = root.Element("SampleData").Value,
                Outcomes = LoadOutcomes(root),
                Attributes = LoadAttributes(root)

            };
            return result;
        }

        private List<DecisionSpaceAttribute> LoadAttributes(XElement root)
        {
            return (from a in root.Descendants("Attribute")
                    select new DecisionSpaceAttribute
                    {
                        Name = a.Element("Name").Value,
                        DataType = a.Element("Type").Value,
                        Classes = (from c in a.Descendants("Class")
                                   select new DataClass
                                   {
                                       Id = int.Parse(c.Element("Id").Value),
                                       Name = c.Element("Name").Value
                                   }).ToList()
                    }).ToList();
        }

        private Outcomes LoadOutcomes(XElement root)
        {
            var outcomesElement = root.Element("Outcomes");
            var vals = from o in outcomesElement.Descendants("Value")
                       select new Outcome
                       {
                           Id = int.Parse(o.Element("Id").Value),
                           Name = o.Element("Name").Value
                       };
            return new Outcomes
            {
                OutcomeType = outcomesElement.Element("Type").Value,
                OutcomeColumn = outcomesElement.Element("Column").Value,
                Values = vals.ToList()
            };
        }
        #endregion

        public DecisionTree CreateTree()
        {
            String csvString = File.ReadAllText(Config.SampleDataLocation);
            String[][] data = CsvParser.Parse(csvString);
            var samples = ConvertSampleDataToDataSet(data);

            Dictionary<DecisionSpaceAttribute, double> IGs = new Dictionary<DecisionSpaceAttribute, double>();
            var x = samples.Tables["Samples"].AsEnumerable();
            foreach (var attr in Config.Attributes)
            {
                IGs[attr] = AverageEntropy(x, attr);
            }

            throw new NotImplementedException();
        }

        /// <summary>
        /// Generate a Decision Tree based on Ross Quinlan's ID3 Iterative Dichotomiser 3 Algorithm
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// ID3 (Examples, Target_Attribute, Attributes)
        ///    Create a root node for the tree
        ///    If all examples are positive, Return the single-node tree Root, with label = +.
        ///    If all examples are negative, Return the single-node tree Root, with label = -.
        ///    If number of predicting attributes is empty, then Return the single node tree Root,
        ///    with label = most common value of the target attribute in the examples.
        ///    Otherwise Begin
        ///        A ← The Attribute that best classifies examples.
        ///        Decision Tree attribute for Root = A.
        ///        For each possible value, vi, of A,
        ///            Add a new tree branch below Root, corresponding to the test A = vi.
        ///            Let Examples(vi) be the subset of examples that have the value vi for A
        ///            If Examples(vi) is empty
        ///                Then below this new branch add a leaf node with label = most common target value in the examples
        ///            Else below this new branch add the subtree ID3(Examples(vi), Target_Attribute, Attributes – {A})
        ///    End
        ///    Return Root
        /// </remarks>
        Node ID3(IEnumerable<DataRow> examples,
            DecisionSpaceAttribute Target_Attribute,
            IEnumerable<DecisionSpaceAttribute> Attributes)
        {
            throw new NotImplementedException();
        }

        double AverageEntropy(IEnumerable<DataRow> samples, DecisionSpaceAttribute attr)
        {
            string attrColName = ConvertAttributeNameToColumnName(attr.Name);

            var parentClassEntropy = ComputeEntropy(samples);

            double minEntropyFound = Double.MaxValue;
            double informationGain = 0.0;
            foreach (DataClass cls in attr.Classes)
            {
                var subSamples = samples.Where(dr => dr.Field<string>(attrColName) == cls.Name);
                var subsetEntropy = ComputeEntropy(subSamples);
                informationGain -= (subSamples.Count() / (double)samples.Count()) * subsetEntropy;
            }
            return informationGain;
        }

        double ComputeEntropy(IEnumerable<DataRow> samples)
        {
            int T = samples.Count();
            Dictionary<string, int> tally = new Dictionary<string, int>();

            foreach (var r in samples)
            {
                var outcome = r.Field<string>("DecisionOutcome");
                int runningTotalForClass = tally.ContainsKey(outcome) ? tally[outcome] : 0;
                tally[outcome] = ++runningTotalForClass;
            }

            double sum = 0.0;
            foreach (var nvp in tally)
            {
                double p = nvp.Value / (double)T;
                double entropy = p * Math.Log(p, 2);
                sum += entropy;
            }
            return -1 * sum;
        }

        bool AllSamplesHaveSameOutcome(DataSet ds, Outcome outcome)
        {
            return Enumerable.All(ds.Tables["Samples"].AsEnumerable(),
                dr => dr.Field<string>("DecisionOutcome") == outcome.Name);
        }

        DataSet ConvertSampleDataToDataSet(string[][] samples)
        {
            DataSet result = new DataSet();
            DataTable st = result.Tables.Add("Samples");
            // mapping from attributes to ordinal numbers
            Dictionary<string, int> map = new Dictionary<string, int>();
            Dictionary<string, string> namemap = new Dictionary<string, string>();
            // first row must contain the column headers (order is not assumed)
            var columnNamesRow = samples[0].ToList();
            st.Columns.Add("DecisionOutcome", typeof(String));
            int outcomeOrdinal = columnNamesRow.IndexOf(Config.Outcomes.OutcomeColumn);

            foreach (var attr in this.Config.Attributes)
            {
                var colName = ConvertAttributeNameToColumnName(attr.Name);
                namemap[attr.Name] = colName;
                st.Columns.Add(colName, typeof(String));
                int ordinal = columnNamesRow.IndexOf(attr.Name);
                map[attr.Name] = ordinal;
            }

            foreach (string[] row in samples.Skip(1)) // 1st row is headers
            {
                DataRow newRow = st.NewRow();
                foreach (var nvp in namemap)
                {
                    var ord = map[nvp.Key];
                    newRow[nvp.Value] = row[ord];
                }
                newRow["DecisionOutcome"] = row[outcomeOrdinal];
                st.Rows.Add(newRow);
            }

            return result;
        }

        private string ConvertAttributeNameToColumnName(string name)
        {
            return name.Replace(' ', '_')
                .Replace('\'', '_')
                .Replace('\"', '_')
                .Replace('\t', '_');
        }
    }
}
