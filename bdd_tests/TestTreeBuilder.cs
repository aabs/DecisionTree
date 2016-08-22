using bdd;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

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
    }
}
