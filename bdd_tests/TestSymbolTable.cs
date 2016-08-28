using System;
using NUnit.Framework;
using DecisionDiagrams;
using FluentAssertions;

namespace bdd_tests
{
    [TestFixture]
    public class TestSymbolTable
    {
        [Test]
        public void TestCanCreateSymbolTable()
        {
            var st = new SymbolTable();
            st.Should().NotBeNull();
        }

        [Test]
        public void TestCanAddSymbolToTable() {
            var stb = new SymbolTableBuilder();
            var st = stb.Build();
            // symbols are declared on first mention...
            st.GetSymbolId("a").Should().NotHaveValue();
            st.DeclareBooleanVariable("a");
            st.GetSymbolId("a").Should().HaveValue();
        }

        [Test]
        public void TesAddingKnownSymbolToTableCausesException()
        {
            var st = new SymbolTable();
            st.DeclareMaybeBooleanVariable("a");
            try
            {
                st.DeclareMaybeBooleanVariable("a");
                Assert.Fail("should have resulted in an exception");
            }
            catch (DecisionException)
            {
                //OK
            }
        }

        [Test]
        public void TestAddingMultipleSymbolsIsOK()
        {
            var st = new SymbolTable();
            var aid = st.DeclareMaybeBooleanVariable("a");
            var bid = st.DeclareMaybeBooleanVariable("b");
            aid.Should().NotBe(bid);
            st.GetSymbolId("a").Should().HaveValue();
            st.GetSymbolId("b").Should().HaveValue();
        }

    }
}
