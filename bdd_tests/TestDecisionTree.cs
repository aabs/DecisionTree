﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using bdd;
using FluentAssertions;
using System.Collections.Generic;

namespace bdd_tests
{
    [TestClass]
    public class TestDecisionTree
    {/*
        [TestMethod]
        public void CanCreateNode()
        {
            var sut = new BranchNode_OLD(1, 2, 3, 1);
            sut.Should().NotBeNull();
        }
        [TestMethod]
        [ExpectedException(typeof(System.ArgumentNullException))]
        public void CannotCreateDTWithNullArgs()
        {
            var sut = new DecisionTree__OLD((Environment)null);
        }
        [TestMethod]
        public void CanCreateNodeInDecisionTree()
        {
            var st = new SymbolTable();
            var env = new Environment(st);
            var sut = new DecisionTree__OLD(env);
            var n = sut.CreateNode("a");
            sut.GetNode(n.Id).Should().NotBeNull();
        }

        [TestMethod]
        public void CanCreateFullTree()
        {
            var st = new SymbolTable();
            var env = new Environment(st);
            var sut = new DecisionTree__OLD(env);
            var rootNode = sut.CreateNode(
                "a",
                sut.CreateNode("b", 0, 1),
                sut.CreateNode("b",
                    sut.CreateNode("c", 0, 1),
                    sut.CreateNode("c", 1, 1)));
            rootNode.Should().NotBeNull();
            CheckNode(sut, rootNode.Id);
        }

        public void CheckNode(DecisionTree__OLD dt, int rootId)
        {
            var root = dt.GetNode(rootId);
            root.Should().NotBeNull();
            if (root.Content.Hi > 1)
            {
                CheckNode(dt, root.Content.Hi);
            }
            if (root.Content.Lo > 1)
            {
                CheckNode(dt, root.Content.Lo);
            }
        }

        [TestMethod]
        public void EvaluateBaseCase()
        {
            var env = new Environment(new Dictionary<string, int> { { "a", 0 } });
            var sut = new DecisionTree__OLD(env);
            var rootNode = sut.CreateNode("a", 0, 1);
            sut.Evaluate(rootNode.Id).Should().Be(0);
        }

        [TestMethod]
        public void EvaluateTwoLevelCase()
        {
            var env = new Environment(new Dictionary<string, int> { { "a", 0 }, { "b", 1 } });
            var sut = new DecisionTree__OLD(env);
            var rootNode = sut.CreateNode("a",
                sut.CreateNode("b", 0, 1),
                sut.CreateNode("b", 1, 1));
            sut.Evaluate(rootNode.Id).Should().Be(1);
        }

        [TestMethod]
        public void EvaluateThreeLevelCase()
        {
            var env = new Environment(new Dictionary<string, int> { { "a", 1 }, { "b", 1 }, { "c", 1 } });
            var sut = new DecisionTree__OLD(env);
            var rootNode = sut.CreateNode(
                "a",
                sut.CreateNode("b", 0, 1),
                sut.CreateNode("b",
                    sut.CreateNode("c", 0, 1),
                    sut.CreateNode("c", 1, 0)));
            sut.Evaluate(rootNode.Id).Should().Be(0);
        }
        */
    }
}
