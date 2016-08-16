using Microsoft.VisualStudio.TestTools.UnitTesting;
using bdd;
using FluentAssertions;
using System.Collections.Generic;

namespace bdd_tests
{
    [TestClass]
    public class node_tests
    {
        [TestMethod]
        public void CanCreateNode()
        {
            var sut = new Node(1, 2, 3, 1);
            sut.Should().NotBeNull();
        }
        [TestMethod]
        public void CanCreateDecisionTree()
        {
            var sut = new DecisionTree(null);
            sut.Should().NotBeNull();
        }
        [TestMethod]
        public void CanCreateNodeInDecisionTree()
        {
            var st = new SymbolTable();
            var env = new Environment(st);
            var sut = new DecisionTree(env);
            var n = sut.CreateNode("a");
            sut.GetNode(n.Id).Should().NotBeNull();
        }

        [TestMethod]
        public void CanCreateFullTree()
        {
            var st = new SymbolTable();
            var env = new Environment(st);
            var sut = new DecisionTree(env);
            var rootNode = sut.CreateNode(
                "a",
                sut.CreateNode("b", 0, 1),
                sut.CreateNode("b",
                    sut.CreateNode("c", 0, 1),
                    sut.CreateNode("c", 1, 1)));
            rootNode.Should().NotBeNull();
            CheckNode(sut, rootNode.Id);
        }

        public void CheckNode(DecisionTree dt, int rootId)
        {
            var root = dt.GetNode(rootId);
            root.Should().NotBeNull();
            if (root.Value.Pass > 1)
            {
                CheckNode(dt, root.Value.Pass);
            }
            if (root.Value.Fail > 1)
            {
                CheckNode(dt, root.Value.Fail);
            }
        }

        public void CheckEvaluationCases(DecisionTree dt, Dictionary<string, int> args, int expectation)
        {
        }
    }
}
