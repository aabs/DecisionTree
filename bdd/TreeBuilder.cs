using Nortal.Utilities.Csv;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace bdd
{
    using DecisionTree = DecisionTree<BaseDtVertexType, DtBranchTest>;
    public class TreeBuilder
    {

        public TreeBuilder(string metadataFileLocation)
        {
            var doc = XDocument.Load(metadataFileLocation);
            this.SymbolTable = LoadConfig(doc.Document.Root);
        }

        #region Configuration Loading
        public SymbolTable SymbolTable { get; private set; }

        private SymbolTable LoadConfig(XElement root)
        {
            var metadata = new Decision
            {
                SampleDataLocation = root.Element("SampleData").Value,
                Outcomes = LoadOutcomes(root),
                Attributes = LoadAttributes(root)
            };
            return BuildSymbolTable(metadata);
        }

        private SymbolTable BuildSymbolTable(Decision d)
        {
            var result = new SymbolTable(d);

            foreach (var attr in d.Attributes)
            {
                if (attr.DataType == "Enumerated")
                {
                    var vals = attr.Classes.ToDictionary(c => c.Name, c => c.Id);
                    result.DeclareEnumeratedVariable(attr.Name, vals);
                }
            }
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
            var csvString = File.ReadAllText(SymbolTable.DecisionMetadata.SampleDataLocation);
            var data = CsvParser.Parse(csvString, new CsvSettings { FieldDelimiter = ',', RowDelimiter = "\n" });
            var samples = ConvertSampleDataToDataSet(data).Tables["Samples"].AsEnumerable();
            SymbolTable.DecisionMetadata.AllSamples = samples;
            var root = CreateTree(samples,
                SymbolTable.DecisionMetadata.Attributes);
            var dt = new DecisionTree
            {
                Tree = new Graph<BaseDtVertexType, DtBranchTest>
                {
                    Root = new Edge<BaseDtVertexType, DtBranchTest>(new DtBranchTest(null), root)
                }
            };
            return dt;
        }

        public Vertex<BaseDtVertexType, DtBranchTest> CreateTree(IEnumerable<DataRow> examples,
            IEnumerable<DecisionSpaceAttribute> attributes)
        {
            // 0. if all samples have same outcome create terminal node with that outcome
            foreach (var o in SymbolTable.DecisionMetadata.Outcomes.Values)
            {
                if (AllSamplesHaveSameOutcome(examples, o))
                {
                    return new Vertex<BaseDtVertexType, DtBranchTest>(new DtOutcome(o.Name));
                }
            }
            // if we get to here, data must be variegated...

            // 1. find the best attribute to classify sample data
            var attr = GetDominatingAttribute(examples, attributes);
            var result = new Vertex<BaseDtVertexType, DtBranchTest>(new DtTest(attr));
            // 2. for each class under that attribute
            foreach (DataClass cls in attr.Classes)
            {
                // 2.1. if samples contain instances with that value of the attribute,
                if (SamplesContainInstanceWithAttributeInClass(examples, attr, cls))
                {
                    // 2.1.1 then create a node for that outcome
                    //       with the list filtered to that outcome
                    var filteredExamples = FilterExamplesToSubclassOfAttribute(examples, attr, cls);
                    //       with the attribute list filtered to remove the attribute
                    var filteredAttributes = attributes.Except(new[] { attr });
                    var edgeLabel = new AttributePermissibleValue { ClassName = cls.Name, Value = cls.Name };
                    var targetVertex = CreateTree(filteredExamples, filteredAttributes);
                    var branch = new DtBranchTest(edgeLabel);
                    var edge = new Edge<BaseDtVertexType, DtBranchTest>(branch, targetVertex);
                    edge.OriginVertex = result;
                    result.AddChild(edge);
                }
            }
            return result;
        }

        private IEnumerable<DataRow> FilterExamplesToSubclassOfAttribute(IEnumerable<DataRow> examples, DecisionSpaceAttribute attr, DataClass cls)
        {
            var col = attr.Name;
            return examples.Where(e => e.Field<string>(col) == cls.Name);
        }

        private SymbolTableEntry GetSymbolTableEntryFromAttribute(DecisionSpaceAttribute attr)
        {
            return SymbolTable.GetEntry(attr.Name);
        }

        private bool SamplesContainInstanceWithAttributeInClass(IEnumerable<DataRow> examples, DecisionSpaceAttribute attr, DataClass cls)
        {
            var col = attr.Name;
            return examples.Any(dr => dr.Field<string>(col) == cls.Name);
        }

        DecisionSpaceAttribute GetDominatingAttribute(IEnumerable<DataRow> examples,
            IEnumerable<DecisionSpaceAttribute> attributes)
        {
            var IGs = new Dictionary<DecisionSpaceAttribute, double>();
            foreach (var attr in attributes)
            {
                IGs[attr] = AverageEntropy(examples, attr);
            }

            // take the absolute value of the scores, order them ascending and take the first (i.e. lowest (lowest entropy, that is))
            return IGs.Select(kvp => new { kvp.Key, Value = Math.Abs(kvp.Value) })
                .OrderBy(kvp => kvp.Value)
                .FirstOrDefault().Key;
        }


        double AverageEntropy(IEnumerable<DataRow> samples, DecisionSpaceAttribute attr)
        {
            var attrColName = attr.Name;

            var parentClassEntropy = ComputeEntropy(samples);

            var informationGain = 0.0;
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
            var T = samples.Count();
            var tally = new Dictionary<string, int>();

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

        bool AllSamplesHaveSameOutcome(IEnumerable<DataRow> samples, Outcome outcome)
        {
            return samples.All(dr => dr.Field<string>("DecisionOutcome") == outcome.Name);
        }

        DataSet ConvertSampleDataToDataSet(string[][] samples)
        {
            var result = new DataSet();
            var st = result.Tables.Add("Samples");
            // mapping from attributes to ordinal numbers
            var map = new Dictionary<string, int>();
            var namemap = new Dictionary<string, string>();
            // first row must contain the column headers (order is not assumed)
            var columnNamesRow = samples[0].ToList();
            st.Columns.Add("DecisionOutcome", typeof(String));
            var outcomeOrdinal = columnNamesRow.IndexOf(SymbolTable.DecisionMetadata.Outcomes.OutcomeColumn);

            foreach (var attr in SymbolTable.DecisionMetadata.Attributes)
            {
                var colName = attr.Name;
                namemap[attr.Name] = colName;
                st.Columns.Add(colName, typeof(String));
                var ordinal = columnNamesRow.IndexOf(attr.Name);
                map[attr.Name] = ordinal;
            }

            foreach (string[] row in samples.Skip(1)) // 1st row is headers
            {
                var newRow = st.NewRow();
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

    }
}
