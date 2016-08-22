using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using bdd;
using FluentAssertions;

namespace bdd_tests
{
    [TestClass]
    public class TestSymbolTable
    {
        [TestMethod]
        public void TestCanCreateSymbolTable()
        {
            var st = new SymbolTable();
            st.Should().NotBeNull();
        }

        [TestMethod]
        public void TestCanAddSymbolToTable() {
            var st = new SymbolTable();
            // symbols are declared on first mention...
            st.GetSymbolId("a").Should().HaveValue();
        }

        [TestMethod]
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

        [TestMethod]
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
