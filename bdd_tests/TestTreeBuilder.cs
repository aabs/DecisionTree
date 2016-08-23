
using bdd;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data;

namespace bdd_tests
{
    [Microsoft.VisualStudio.TestTools.UnitTesting.TestClass]
    public class TestTreeBuilder
    {
        private const string testDataCsvFile = @"C:\dev\binarydecisiontree\testdata\problem-metadata.xml";

        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethod]
        public void CanCreateTreeBuilder()
        {
            var sut = new TreeBuilder(testDataCsvFile);
            sut.Should().NotBeNull();
        }

        [TestMethod]
        public void CanRunTreeBuilder()
        {
            var sut = new TreeBuilder(testDataCsvFile);
            DecisionTree dt = sut.CreateTree();
        }

        [TestMethod]
        public void CanEvaluateTree()
        {
            var sut = new TreeBuilder(testDataCsvFile);
            DecisionTree dt = sut.CreateTree();

            foreach (DataRow row in sut.Config.AllSamples)
            {
                dt.Environment = RowToEnv(sut.Config, row);
                row.Field<string>("DecisionOutcome").Should().Be(dt.Evaluate());
            }
        }

        private bdd.Environment RowToEnv(Decision config, DataRow row)
        {
            var env = new bdd.Environment(config.SymbolTable);
            foreach (var attr in config.Attributes)
            {
                var colName = UtilityFunctions.ConvertAttributeNameToColumnName(attr.Name);
                env.Bind(attr.Name, row.Field<string>(colName));
            }
            return env;
        }
    }
}
