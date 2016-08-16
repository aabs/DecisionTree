// <copyright file="DecisionTreeTest.cs">Copyright ©  2016</copyright>
using System;
using Microsoft.Pex.Framework;
using Microsoft.Pex.Framework.Validation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using bdd;

namespace bdd.Tests
{
    /// <summary>This class contains parameterized unit tests for DecisionTree</summary>
    [PexClass(typeof(DecisionTree))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(InvalidOperationException))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(ArgumentException), AcceptExceptionSubtypes = true)]
    [TestClass]
    public partial class DecisionTreeTest
    {
        /// <summary>Test stub for CreateNode(String)</summary>
        [PexMethod]
        public Node CreateNodeTest([PexAssumeUnderTest]DecisionTree target, string v)
        {
            Node result = target.CreateNode(v);
            return result;
            // TODO: add assertions to method DecisionTreeTest.CreateNodeTest(DecisionTree, String)
        }

        /// <summary>Test stub for Evaluate(Int32)</summary>
        [PexMethod]
        public int EvaluateTest([PexAssumeUnderTest]DecisionTree target, int nodeIndex)
        {
            int result = target.Evaluate(nodeIndex);
            return result;
            // TODO: add assertions to method DecisionTreeTest.EvaluateTest(DecisionTree, Int32)
        }
    }
}
