using Nortal.Utilities.Csv;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace Modd
{
    using DecisionDiagram.Metadata;
    using Metadata;
    using DecisionTree = DecisionTree<BaseDtVertexType, DtBranchTest>;

    public class DecisionTreeBuilder
    {
        public MetadataConfiguration Config { get; set; }

        public DecisionTreeBuilder(MetadataConfiguration config)
        {
            Config = config;
            this.SymbolTable = LoadConfig();
            this.DefaultOutcome = config.DefaultOutcome;
        }

        public DecisionTreeBuilder(string metadataFileLocation)
        {
            var doc = XDocument.Load(metadataFileLocation);
            this.SymbolTable = LoadConfig(doc.Document.Root);
            this.DefaultOutcome = doc.Document.Root.Element("DefaultOutcome").Value;
        }

        public SymbolTable SymbolTable { get; private set; }
        public string DefaultOutcome { get; private set; }

        private SymbolTable LoadConfig()
        {
            var mdb = new MetadataBuilder(Config.SampleDataLocation)
                .WithOutcomeColumn(Config.OutcomeColumn);
            var metadata = mdb.Build();
            return BuildSymbolTable(metadata);
        }

        private SymbolTable LoadConfig(XElement root)
        {
            Config = new MetadataConfiguration
            {
                ColumnsToIgnore = root.Element("Ignore").Descendants("Column").Select(x => x.Value),
                DefaultOutcome = root.Element("DefaultOutcome").Value,
                OutcomeColumn = root.Element("OutcomeColumn").Value,
                SampleDataLocation = root.Element("SampleData").Value
            };
            return LoadConfig();
        }

        private SymbolTable BuildSymbolTable(DecisionMetadata d)
        {
            var result = new SymbolTable(d);

            foreach (var attr in d.Attributes)
            {
                if (attr.KindOfData == "Enumerated")
                {
                    var vals = attr.PossibleValues.ToDictionary(c => c.Value);
                    result.DeclareEnumeratedVariable(attr.Name, attr.PossibleValues);
                }
            }
            return result;
        }

        public DecisionTree CreateReducedTree()
        {
            var unoptimisedTree = CreateTree();
            var reducer = new Reducer(null);
            var optimisedTree = reducer.Reduce(SymbolTable, unoptimisedTree.Tree);

            var result = new DecisionTree
            {
                Tree = optimisedTree,
                SymbolTable = this.SymbolTable
            };
            return result;
        }

        public DecisionTree CreateTree()
        {
            var csvString = File.ReadAllText(SymbolTable.DecisionMetadata.SampleDataLocation);
            var data = CsvParser.Parse(csvString, new CsvSettings { FieldDelimiter = ',', RowDelimiter = "\n" });
            var samples = ConvertSampleDataToDataSet(data).Tables["Samples"].AsEnumerable();
            SymbolTable.DecisionMetadata.AllSamples = samples;
            var stb = new SimpleTreeBuilder();
            CreateTree(stb,
                samples,
                SymbolTable.DecisionMetadata.Attributes,
                null);
            return stb.Build();
        }

        public void CreateTree(SimpleTreeBuilder tb,
            IEnumerable<DataRow> samples,
            IEnumerable<Attribute> attributes,
            string label)
        {
            Attribute attr = null;
            bool isCreatingRootVertex = (tb.IsEmpty || string.IsNullOrWhiteSpace(label));

            if (tb.CurrentVertex != null && tb.CurrentVertex is DtOutcome)
            {
                throw new DecisionException("Shouldn't try to recurse further after terminating on a leaf node");
            }

            if (attributes.Count() < 1)
            {
                // if there are no more attributes, exploration further is impossible
                //throw new DecisionException("Malformed decision model. No attributes defined.");
                return;
            }

            attr = GetDominatingAttribute(samples, attributes);
            try
            {
                // if tree is empty?
                if (isCreatingRootVertex)
                {
                    tb.WithRoot(new DtTest(attr));
                    // PROBLEM: Only the first child edge of the root is being explored. Why?
                }
                else
                {
                    // check here whether the samples all contain the same outcome. if so create an outcome and return
                    foreach (var outcome in SymbolTable.DecisionMetadata.Outcomes.Values)
                    {
                        if (AllSamplesHaveSameOutcome(samples, outcome))
                        {
                            tb.StartChild(new DtOutcome(outcome.Name), label);
                            return;
                        }
                    }
                    tb.StartChild(new DtTest(attr), label);
                }

                //   for pv in possible values
                foreach (var pv in attr.PossibleValues)
                {
                    if (SamplesContainInstanceWithAttributeInClass(samples, attr, pv))
                    {
                        //     filter examples for matches with pv
                        var filteredSamples = FilterExamplesToSubclassOfAttribute(samples, attr, pv);
                        //     filter dominating attribute out of set of attrs
                        var filteredAttributes = attributes.Except(new[] { attr });
                        //     recurse with label <- pv
                        CreateTree(tb, filteredSamples, filteredAttributes, pv.Value);
                        // PROBLEM: By the time we get here, the currentvertex has changed. Why?
                    }
                    else
                    {
                        tb.StartChild(new DtOutcome(this.DefaultOutcome), pv.Value).EndChild();
                    }
                }
            }
            finally
            {
                tb.EndChild();
            }
        }

        private IEnumerable<DataRow> FilterExamplesToSubclassOfAttribute(IEnumerable<DataRow> examples, Attribute attr, PossibleValue cls)
        {
            var col = attr.Name;
            return examples.Where(e => e.Field<string>(col) == cls.Value);
        }

        private bool SamplesContainInstanceWithAttributeInClass(IEnumerable<DataRow> examples, Attribute attr, PossibleValue cls)
        {
            var col = attr.Name;
            return examples.Any(dr => dr.Field<string>(col) == cls.Value);
        }

        private Attribute GetDominatingAttribute(IEnumerable<DataRow> examples,
            IEnumerable<Attribute> attributes)
        {
            var IGs = new Dictionary<Attribute, double>();
            foreach (var attr in attributes)
            {
                IGs[attr] = AverageEntropy(examples, attr);
            }

            // take the absolute value of the scores, order them ascending and take the first (i.e. lowest (lowest entropy, that is))
            return IGs.Select(kvp => new { kvp.Key, Value = Math.Abs(kvp.Value) })
                .OrderBy(kvp => kvp.Value)
                .FirstOrDefault().Key;
        }

        private double AverageEntropy(IEnumerable<DataRow> samples, Attribute attr)
        {
            var attrColName = attr.Name;

            var parentClassEntropy = ComputeEntropy(samples);

            var informationGain = 0.0;
            foreach (PossibleValue cls in attr.PossibleValues)
            {
                var subSamples = samples.Where(dr => dr.Field<string>(attrColName) == cls.Value);
                var subsetEntropy = ComputeEntropy(subSamples);
                informationGain -= (subSamples.Count() / (double)samples.Count()) * subsetEntropy;
            }
            return informationGain;
        }

        private double ComputeEntropy(IEnumerable<DataRow> samples)
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

        private bool AllSamplesHaveSameOutcome(IEnumerable<DataRow> samples, DecisionOutcome outcome)
        {
            return samples.All(dr => dr.Field<string>("DecisionOutcome") == outcome.Name);
        }

        private DataSet ConvertSampleDataToDataSet(string[][] samples)
        {
            var result = new DataSet();
            var st = result.Tables.Add("Samples");
            // mapping from attributes to ordinal numbers
            var map = new Dictionary<string, int>();
            var namemap = new Dictionary<string, string>();
            // first row must contain the column headers (order is not assumed)
            var columnNamesRow = samples[0].ToList();
            st.Columns.Add("DecisionOutcome", typeof(String));
            var outcomeOrdinal = columnNamesRow.IndexOf(SymbolTable.DecisionMetadata.Outcomes.OutcomeColumnNameInSampleData);

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