using bdd;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace bdd_tests
{
    [TestClass]
    public class TreeTests
    {
        [TestMethod]
        public void CanCreateGraphVertex()
        {
            var sut = new Vertex<string, string>("hello world");
            sut.Should().NotBeNull();
        }

        [TestMethod]
        public void CanLinkVertices()
        {
            var sut = new Vertex<string, string>("hello world");
            sut.Children.Count().Should().Be(0);
            sut.AddChild("node", "link");
            sut.Children.Count().Should().Be(1);
            sut.Children.First().LinkLabel.Should().Be("link");
            sut.Children.First().TargetVertex.Value.Should().Be("node");
        }

        [TestMethod]
        public void CanLinkVerticesUsingIndexer()
        {
            var sut = new Vertex<string, string>("hello world");
            sut.Children.Count().Should().Be(0);
            sut["edge"] = "vertex";
            sut.Children.Count().Should().Be(1);
            sut.Children.First().LinkLabel.Should().Be("edge");
            sut.Children.First().TargetVertex.Value.Should().Be("vertex");
        }

        [TestMethod]
        public void CanGetNodeValueViaIndexer()
        {
            var sut = new Vertex<string, string>("hello world");
            sut["edge"] = "vertex";
            sut["edge"].Should().Be("vertex");
        }

        [TestMethod]
        [ExpectedException(typeof(DecisionException))]
        public void HandlesInvalidAccessorProperly()
        {
            var sut = new Vertex<string, string>("hello world");
            var dummy = sut["missing"];
        }

        [TestMethod]
        public void CanCreateChain()
        {
            var sut = new Vertex<string, string>("vertex1");
            sut["link1"] = "vertex2";
            sut.Child("link1")["link2"] = "vertex3";
            var v2 = sut.Child("link1");
            v2.Should().NotBeNull();
            var v3 = v2.Child("link2");
            v3.Should().NotBeNull();
        }

        [TestMethod]
        public void CanHaveMultipleChildren()
        {
            var sut = new Vertex<string, string>("vertex1");
            sut["e1"] = "v2";
            sut["e2"] = "v3";
            sut["e1"].Should().Be("v2");
            sut["e2"].Should().Be("v3");
        }

        [TestMethod]
        public void CanTestEquivalence()
        {
            var sut = new Vertex<string, string>("v1");
            sut["e1"] = "v2";
            sut["e2"] = "v3";

            var sut2 = new Vertex<string, string>("v1");
            sut2["e1"] = "v2";
            sut2["e2"] = "v3";

            sut.EquivalentTo(sut2).Should().Be(true);
        }

        [TestMethod]
        public void CanTestEquivalenceRecursively()
        {
            var sut = new Vertex<string, string>("v1");
            sut["e1"] = "v2";
            sut.Child("e1")["e2"] = "v3";

            var sut2 = new Vertex<string, string>("v1");
            sut2["e1"] = "v2";
            sut2.Child("e1")["e2"] = "v3";

            sut.EquivalentTo(sut2).Should().Be(true);
        }

        [TestMethod]
        public void CanTestNonequivalence()
        {
            var sut = new Vertex<string, string>("v1");
            sut["e1"] = "v2";
            sut["other"] = "values";

            var sut2 = new Vertex<string, string>("v1");
            sut2["e1"] = "v2";
            sut2["e2"] = "v3";

            sut.EquivalentTo(sut2).Should().Be(false);
        }
        [TestMethod]
        public void CanTestNonequivalence2()
        {
            var sut = new Vertex<string, string>("v1");
            sut["e1"] = "v2";
            sut["other"] = "values";

            var sut2 = new Vertex<string, string>("v1");

            sut.EquivalentTo(sut2).Should().Be(false);
        }
        [TestMethod]
        public void CanTestNonequivalence3()
        {
            var sut = new Vertex<string, string>("v1");

            var sut2 = new Vertex<string, string>("v1");
            sut2["e1"] = "v2";
            sut2["e2"] = "v3";

            sut.EquivalentTo(sut2).Should().Be(false);
        }
        [TestMethod]
        public void CanTestNonequivalence4()
        {
            var sut = new Vertex<string, string>("v1");
            var sut2 = new Vertex<string, string>("other");
            sut.EquivalentTo(sut2).Should().Be(false);
        }

        [TestMethod]
        public void CanTestNonequivalence5()
        {
            var sut = new Vertex<string, string>("v1");
            sut["e1"] = "v2";
            sut["e2"] = "different";

            var sut2 = new Vertex<string, string>("v1");
            sut2["e1"] = "v2";
            sut2["e2"] = "v3";

            sut.EquivalentTo(sut2).Should().Be(false);
        }
    }
}
