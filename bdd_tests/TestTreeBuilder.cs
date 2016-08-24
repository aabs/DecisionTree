
using bdd;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data;
using System.Diagnostics;

namespace bdd_tests
{
    [TestClass]
    public class TestTreeBuilder
    {
        private const string testDataCsvFile = @"..\..\..\testdata\problem-metadata.xml";

        [TestMethod]
        public void CanCreateTreeBuilder()
        {
            var sut = new TreeBuilder(testDataCsvFile);
            sut.Should().NotBeNull();
        }

        [TestMethod]
        public void CanRunTreeBuilder()
        {
            var sut = new TreeBuilder(testDataCsvFile);
            var dt = sut.CreateTree();
        }

        [TestMethod]
        public void CanEvaluateTree()
        {
            var sut = new TreeBuilder(testDataCsvFile);
            var dt = sut.CreateTree();

            int correct = 0, total = 0;

            foreach (DataRow row in sut.SymbolTable.DecisionMetadata.AllSamples)
            {
                var environment = RowToEnv(sut.SymbolTable, row);
                var evaluator = new EvaluatorVisitor(dt, environment);
                evaluator.Visit(dt.Tree.Root);
                if (evaluator.EvaluatedResult.Equals(row["DecisionOutcome"]))
                {
                    correct++;
                }
                else
                {
                    Debug.Write($"Fail: (Eval: {evaluator.EvaluatedResult})\t");
                    foreach (var c in row.Table.Columns)
                    {
                        var col = c as DataColumn;
                        Debug.Write($"{col.ColumnName}: {row[col.ColumnName]},\t");
                    }
                    Debug.Write("\n");
                }
                total++;
            }
            correct.Should().Be(total);
        }

        private bdd.Environment RowToEnv(SymbolTable st, DataRow row)
        {
            var env = new bdd.Environment(st);
            foreach (var attr in st.DecisionMetadata.Attributes)
            {
                var colName = attr.Name;
                env.Bind(attr.Name, row.Field<string>(colName));
            }
            return env;
        }
    }
}
