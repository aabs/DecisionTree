using Microsoft.Pex.Framework.Generated;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using bdd;
// <copyright file="BranchNodeTest.ConstructorTest01.g.cs">Copyright ©  2016</copyright>
// <auto-generated>
// This file contains automatically generated tests.
// Do not modify this file manually.
// 
// If the contents of this file becomes outdated, you can delete it.
// For example, if it no longer compiles.
// </auto-generated>
using System;

namespace bdd.Tests
{
    public partial class BranchNodeTest
    {

[TestMethod]
[PexGeneratedBy(typeof(BranchNodeTest))]
public void ConstructorTest01338()
{
    BranchNode branchNode;
    branchNode = this.ConstructorTest01((Node)null);
    Assert.IsNotNull((object)branchNode);
    Assert.IsNull(branchNode.Symbol);
    Assert.IsNotNull(branchNode.Branches);
    Assert.IsNotNull(branchNode.Branches.Comparer);
    Assert.AreEqual<int>(0, branchNode.Branches.Count);
    Assert.IsNull(((Node)branchNode).Parent);
}
    }
}
