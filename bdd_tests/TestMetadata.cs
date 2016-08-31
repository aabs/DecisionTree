using NUnit.Framework;
using FluentAssertions;
using DecisionDiagram.Metadata;

namespace bdd_tests
{
    [TestFixture]
    public class MetadataBuilderTest
    {
        private string GetMetadataPath()
        {
            return System.Environment.GetEnvironmentVariable("BDD_TEST_DATA_DIR") ?? @"C:\dev\binarydecisiontree\bdd_tests\testdata\usm.mbrmatch.trainingdata.csv";
        }

        [Test]
        public void CanConstructBuilder()
        {
            var sut = new MetadataBuilder(GetMetadataPath());
            sut.Should().NotBeNull();
        }

        [Test]
        public void CanBuildMDObject()
        {
            var sut = new MetadataBuilder(GetMetadataPath())
                .WithOutcomeColumn("Status");

            var x = sut.Build();
            x.Should().NotBeNull();
        }
    }
}