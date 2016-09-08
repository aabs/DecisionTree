using System;
using System.Linq;
using System.Collections.Generic;
using Modd;
using FluentAssertions;
using NUnit.Framework;
using System.Data;
using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Modd.Metadata;

namespace bdd_tests
{

    [TestFixture]
    public class SimpletreeBuilderTest
    {
        [Test]
        public void TestCanCreateSut()
        {
            var sut = new SimpleTreeBuilder();
            sut.Should().NotBeNull();
        }

        [Test]
        public void TestCanCreateDefaultStructure()
        {
            var sut = new SimpleTreeBuilder();
            var r = sut.Build();
            r.Should().NotBeNull();
        }

        [Test]
        public void TestCanCreateJustOutcomeVertex()
        {
            var sut = new SimpleTreeBuilder()
                .WithRoot(new DtOutcome("Accepted"));
            var r = sut.Build();
            r.Should().NotBeNull();
            r.Tree.Root().Should().NotBeNull();
            r.Tree.Root().Should().BeOfType(typeof(DtOutcome));
        }
        [Test]
        public void TestCanAddTestAsRoot()
        {
            var sut = new SimpleTreeBuilder()
                .WithRoot(new DtTest(new Modd.Metadata.Attribute("root", "hello", "world")));
            var r = sut.Build();
            r.Should().NotBeNull();
            r.Tree.Root().Should().NotBeNull();
            var test = r.Tree.Root() as DtTest;
            test.Should().NotBeNull();
            test.Attribute.Should().NotBeNull();
            test.Attribute.Name.Should().Be("root");
            test.Attribute.PossibleValues.Should().NotBeNullOrEmpty();
        }

        [Test]
        public void TestCanLinkInAChildVertex()
        {
            var sut = new SimpleTreeBuilder()
                .WithRoot(new DtTest(new Modd.Metadata.Attribute("root", "hello", "world")))
                .StartChild(new DtOutcome("Accepted"), "hello");
            var r = sut.Build();
            r.Should().NotBeNull();
        }

        [Test]
        public void TestCannotLinkInAChildVertexViaAnUnacceptableLabel()
        {
            var sut = new SimpleTreeBuilder()
                .WithRoot(new DtTest(new Modd.Metadata.Attribute("root", "acceptable1", "acceptable2")));
            sut.StartChild(new DtOutcome("Accepted"), "acceptable1")
                .EndChild();
            NUnit.Framework.Assert.Throws<DecisionException>(() =>
            {
                sut.StartChild(new DtOutcome("Accepted"), "XXUNACCEPTABLEXX");
            }, "unknown test result value supplied");
            var r = sut.Build();
            r.Should().NotBeNull();
        }
    }

}