﻿using NUnit.Framework;
using DecisionDiagrams;
using FluentAssertions;
using System.Collections.Generic;

namespace bdd_tests
{
    [TestFixture]
    public class TestDecisionTreeOptimiser
    {
        [Test]
        public void CanDetectRedundantBranchNodes()
        {
            //var st = new SymbolTable();
            //var env = new Environment(st);
            //var sut = new DecisionTree__OLD(env);
            //var redundantBranchNode = sut.CreateNode("b", 0, 0);
            //var rootNode = sut.CreateNode("a", redundantBranchNode, 1);
            //sut.RootIndex = rootNode.Id;
            //sut.Nodes.Count.Should().Be(2);
            //var sut2 = DecisionTreeOptimiser.Reduce(sut);
            //sut2.Nodes.Count.Should().Be(1);
            //sut.Nodes.Count.Should().Be(2);
            //sut.Should().NotBe(sut2);
        }
    }
}
