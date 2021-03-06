// <copyright file="DecisionTreeTest.cs">Copyright ©  2016</copyright>
using System;
using Microsoft.Pex.Framework;
using Microsoft.Pex.Framework.Validation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using bdd;

namespace bdd.Tests
{
    /// <summary>This class contains parameterized unit tests for DecisionTree</summary>
    [PexClass(typeof(DecisionTree<BaseDtVertexType, DtBranchTest>))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(InvalidOperationException))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(ArgumentException), AcceptExceptionSubtypes = true)]
    [TestClass]
    public partial class DecisionTreeTest
    {

    }
}
