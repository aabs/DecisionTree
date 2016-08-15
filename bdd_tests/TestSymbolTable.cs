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
            st.GetSymbolId("a").Should().NotHaveValue();
            st.DeclareVariable("a");
            st.GetSymbolId("a").Should().HaveValue();
        }

        [TestMethod]
        public void TesAddingKnownSymbolToTableCausesException()
        {
            var st = new SymbolTable();
            st.DeclareVariable("a");
            try
            {
                st.DeclareVariable("a");
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
            var aid = st.DeclareVariable("a");
            var bid = st.DeclareVariable("b");
            aid.Should().NotBe(bid);
            st.GetSymbolId("a").Should().HaveValue();
            st.GetSymbolId("b").Should().HaveValue();
        }

    }
}
