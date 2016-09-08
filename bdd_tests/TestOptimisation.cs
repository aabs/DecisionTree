using NUnit.Framework;
using Modd;
using FluentAssertions;
using System.Collections.Generic;
using DecisionDiagram.Metadata;

namespace bdd_tests
{
    [TestFixture]
    public class TestOptimisation
    {
        [Test]
        public void CanConstructDecisionMetadaatUsingWithColumn()
        {
            var sut = new MetadataBuilder(GetMetadataPath())
                .WithColumn(0, "h1", "true", "false", "huh?")
                .WithColumn(1, "h2", "true", "false", "huh?")
                .WithColumn(2, "status", "OK", "No Way, Jose!")
                .WithOutcomeColumn("status")
                .Build();

            sut.Should().NotBeNull();
        }
        private string GetMetadataPath()
        {
            return System.Environment.GetEnvironmentVariable("BDD_TEST_DATA_DIR") ??
                @"C:\dev\binarydecisiontree\bdd_tests\testdata\problem-metadata.xml";
        }
    }
}
