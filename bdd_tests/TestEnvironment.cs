using Microsoft.VisualStudio.TestTools.UnitTesting;
using bdd;
using FluentAssertions;

namespace bdd_tests
{
    [TestClass]
    public class TestEnvironment
    {
        [TestMethod]
        public void TestCanCreateEnvironment()
        {
            var st = new SymbolTable();
            var sut = new Environment(st);
            sut.Should().NotBeNull();
        }

        [TestMethod]
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
