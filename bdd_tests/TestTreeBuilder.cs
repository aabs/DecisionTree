
using bdd;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data;

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

            foreach (DataRow row in sut.SymbolTable.DecisionMetadata.AllSamples)
            {
                var environment = RowToEnv(sut.SymbolTable, row);
                var evaluator = new EvaluatorVisitor(dt, environment);
                Dispatcher<BaseDtVertexType, DtBranchTest>.AcceptBranch(dt.Tree.Root, evaluator);
                row.Field<string>("DecisionOutcome").Should().Be(evaluator.EvaluatedResult);
            }
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
