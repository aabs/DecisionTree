using NUnit.Framework;
using DecisionDiagrams;
using FluentAssertions;

namespace bdd_tests
{
    [TestFixture]
    public class TestEnvironment
    {
        [Test]
        public void TestCanCreateEnvironment()
        {
            var st = new SymbolTable();
            var sut = new Environment(st);
            sut.Should().NotBeNull();
        }

        [Test]
        public void TestCanBindVariable()
        {
            var stb = new SymbolTableBuilder().WithSymbol("a", "1", "2", "3");
            var st = stb.Build();
            var sut = new Environment(st);
            sut.Bind("a", "2");
            sut.Resolve("a").Should().NotBeNull();
        }

    }
}
