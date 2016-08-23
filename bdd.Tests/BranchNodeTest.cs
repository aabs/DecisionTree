using System.Collections.Generic;
using System;
using Microsoft.Pex.Framework;
using Microsoft.Pex.Framework.Validation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using bdd;

namespace bdd.Tests
{
    /// <summary>This class contains parameterized unit tests for BranchNode</summary>
    [TestClass]
    [PexClass(typeof(BranchNode))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(ArgumentException), AcceptExceptionSubtypes = true)]
    [PexAllowedExceptionFromTypeUnderTest(typeof(InvalidOperationException))]
    public partial class BranchNodeTest
    {

        /// <summary>Test stub for .ctor(SymbolTableEntry, Node)</summary>
        [PexMethod]
        public BranchNode ConstructorTest(SymbolTableEntry symbol, Node parentNode)
        {
            BranchNode target = new BranchNode(symbol, parentNode);
            return target;
            // TODO: add assertions to method BranchNodeTest.ConstructorTest(SymbolTableEntry, Node)
        }

        /// <summary>Test stub for .ctor(Node)</summary>
        [PexMethod]
        public BranchNode ConstructorTest01(Node node)
        {
            BranchNode target = new BranchNode(node);
            return target;
            // TODO: add assertions to method BranchNodeTest.ConstructorTest01(Node)
        }

        /// <summary>Test stub for IsRedundant()</summary>
        [PexMethod]
        public bool IsRedundantTest([PexAssumeUnderTest]BranchNode target)
        {
            bool result = target.IsRedundant();
            return result;
            // TODO: add assertions to method BranchNodeTest.IsRedundantTest(BranchNode)
        }

        /// <summary>Test stub for PrettyPrint(Int32)</summary>
        [PexMethod]
        public string PrettyPrintTest([PexAssumeUnderTest]BranchNode target, int indentLevel)
        {
            string result = target.PrettyPrint(indentLevel);
            return result;
            // TODO: add assertions to method BranchNodeTest.PrettyPrintTest(BranchNode, Int32)
        }

        /// <summary>Test stub for get_Branches()</summary>
        [PexMethod]
        public Dictionary<AttributeClassicationInstance, Node> BranchesGetTest([PexAssumeUnderTest]BranchNode target)
        {
            Dictionary<AttributeClassicationInstance, Node> result = target.Branches;
            return result;
            // TODO: add assertions to method BranchNodeTest.BranchesGetTest(BranchNode)
        }

        /// <summary>Test stub for get_SymbolId()</summary>
        [PexMethod]
        public int SymbolIdGetTest([PexAssumeUnderTest]BranchNode target)
        {
            int result = target.SymbolId;
            return result;
            // TODO: add assertions to method BranchNodeTest.SymbolIdGetTest(BranchNode)
        }

        /// <summary>Test stub for set_Branches(Dictionary`2&lt;AttributeClassicationInstance,Node&gt;)</summary>
        [PexMethod]
        public void BranchesSetTest(
            [PexAssumeUnderTest]BranchNode target,
            Dictionary<AttributeClassicationInstance, Node> value
        )
        {
            target.Branches = value;
            // TODO: add assertions to method BranchNodeTest.BranchesSetTest(BranchNode, Dictionary`2<AttributeClassicationInstance,Node>)
        }
    }
}
