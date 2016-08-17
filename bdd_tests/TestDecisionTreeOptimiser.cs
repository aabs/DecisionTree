using Microsoft.VisualStudio.TestTools.UnitTesting;
using bdd;
using FluentAssertions;
using System.Collections.Generic;

namespace bdd_tests
{
    [TestClass]
    public class TestDecisionTreeOptimiser
    {
        [TestMethod]
        public void CanDetectRedundantBranchNodes()
        {
            var st = new SymbolTable();
            var env = new Environment(st);
            var sut = new DecisionTree(env);
            var redundantBranchNode = sut.CreateNode("b", 0, 0);
            var rootNode = sut.CreateNode("a", redundantBranchNode, 1);
            sut.RootIndex = rootNode.Id;
            sut.Nodes.Count.Should().Be(2);
            var sut2 = DecisionTreeOptimiser.Reduce(sut);
            sut2.Nodes.Count.Should().Be(1);
        }
    }
}
