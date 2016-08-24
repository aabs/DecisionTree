using bdd;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
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
            sut.Children.First().Label.Should().Be("link");
            sut.Children.First().TargetVertex.Content.Should().Be("node");
        }

        [TestMethod]
        public void CanLinkVerticesUsingIndexer()
        {
            var sut = new Vertex<string, string>("hello world");
            sut.Children.Count().Should().Be(0);
            sut["edge"] = "vertex";
            sut.Children.Count().Should().Be(1);
            sut.Children.First().Label.Should().Be("edge");
            sut.Children.First().TargetVertex.Content.Should().Be("vertex");
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

        public class TestEdgeType : IEquatable<TestEdgeType>
        {
            public TestEdgeType(int label)
            {
                this.LabelType = label;
            }
            public int LabelType { get; set; }

            public bool Equals(TestEdgeType other)
            {
                return LabelType.Equals(other.LabelType);
            }
        }
        class TestVertexType : IEquatable<TestVertexType>
        {
            public TestVertexType(int something)
            {
                Value = Name = something;
            }
            public int Value { get; set; }
            public int Name { get; set; }

            public bool Equals(TestVertexType other)
            {
                return Name.Equals(other.Name) && Value.Equals(other.Value);
            }
        }

        [TestMethod]
        public void CanCreateGraphVertex_Objects()
        {
            var sut = new Vertex<TestVertexType, TestEdgeType>(new TestVertexType(42));
            sut.Should().NotBeNull();
        }

        [TestMethod]
        public void CanLinkVertices_Objects()
        {
            var sut = new Vertex<TestVertexType, TestEdgeType>(new TestVertexType(42));
            sut.Children.Count().Should().Be(0);
            sut.AddChild(new TestVertexType(3), new TestEdgeType(4));
            sut.Children.Count().Should().Be(1);
            sut.Children.First().Label.LabelType.Should().Be(4);
            sut.Children.First().TargetVertex.Content.Name.Should().Be(3);
        }

        [TestMethod]
        public void CanLinkVerticesUsingIndexer_Objects()
        {
            var sut = new Vertex<TestVertexType, TestEdgeType>(new TestVertexType(42));
            sut.Children.Count().Should().Be(0);

            var e1 = new TestEdgeType(5);
            var v1 = new TestVertexType(7);

            sut[e1] = v1;
            sut.Children.Count().Should().Be(1);
            sut.Children.First().Label.LabelType.Should().Be(5);
            sut.Children.First().TargetVertex.Content.Name.Should().Be(7);
        }

        [TestMethod]
        public void CanGetNodeValueViaIndexer_Objects()
        {
            var sut = new Vertex<TestVertexType, TestEdgeType>(new TestVertexType(42));
            var e1 = new TestEdgeType(1);
            var v1 = new TestVertexType(1);
            sut[e1] = v1;
            sut[e1].Should().Be(v1);
        }

        [TestMethod]
        [ExpectedException(typeof(DecisionException))]
        public void HandlesInvalidAccessorProperly_Objects()
        {
            var sut = new Vertex<TestVertexType, TestEdgeType>(new TestVertexType(42));
            var missing = new TestEdgeType(1);
            var dummy = sut[missing];
        }

        [TestMethod]
        public void CanCreateChain_Objects()
        {
            var e1 = new TestEdgeType(1);
            var v1 = new TestVertexType(1);
            var e2 = new TestEdgeType(2);
            var v2 = new TestVertexType(2);
            var e3 = new TestEdgeType(3);
            var v3 = new TestVertexType(3);

            var sut = new Vertex<TestVertexType, TestEdgeType>(v1);
            sut[e1] = v2;
            sut.Child(e1)[e2] = v3;
            var v4 = sut.Child(e1);
            v4.Should().NotBeNull();
            var v5 = v4.Child(e2);
            v5.Should().NotBeNull();
        }

        [TestMethod]
        public void CanHaveMultipleChildren_Objects()
        {
            var e1 = new TestEdgeType(1);
            var v1 = new TestVertexType(1);
            var e2 = new TestEdgeType(2);
            var v2 = new TestVertexType(2);
            var e3 = new TestEdgeType(3);
            var v3 = new TestVertexType(3);

            var sut = new Vertex<TestVertexType, TestEdgeType>(v1);
            sut[e1] = v2;
            sut[e2] = v3;
            sut[e1].Should().Be(v2);
            sut[e2].Should().Be(v3);
        }

        [TestMethod]
        public void CanTestEquivalence_Objects()
        {
            var e1 = new TestEdgeType(1);
            var v1 = new TestVertexType(1);
            var e2 = new TestEdgeType(2);
            var v2 = new TestVertexType(2);
            var e3 = new TestEdgeType(3);
            var v3 = new TestVertexType(3);

            var sut = new Vertex<TestVertexType, TestEdgeType>(v1);
            sut[e1] = v2;
            sut[e2] = v3;

            var sut2 = new Vertex<TestVertexType, TestEdgeType>(v1);
            sut2[e1] = v2;
            sut2[e2] = v3;

            sut.EquivalentTo(sut2).Should().Be(true);
        }

        [TestMethod]
        public void CanTestEquivalenceRecursively_Objects()
        {
            var e1 = new TestEdgeType(1);
            var v1 = new TestVertexType(1);
            var e2 = new TestEdgeType(2);
            var v2 = new TestVertexType(2);
            var e3 = new TestEdgeType(3);
            var v3 = new TestVertexType(3);

            var sut = new Vertex<TestVertexType, TestEdgeType>(v1);
            sut[e1] = v2;
            sut.Child(e1)[e2] = v3;

            var sut2 = new Vertex<TestVertexType, TestEdgeType>(v1);
            sut2[e1] = v2;
            sut2.Child(e1)[e2] = v3;

            sut.EquivalentTo(sut2).Should().Be(true);
        }

        [TestMethod]
        public void CanTestNonequivalence_Objects()
        {
            var e1 = new TestEdgeType(1);
            var v1 = new TestVertexType(1);
            var other = new TestEdgeType(2);
            var values = new TestVertexType(2);
            var e2 = new TestEdgeType(2);
            var v2 = new TestVertexType(2);
            var v3 = new TestVertexType(3);


            var sut = new Vertex<TestVertexType, TestEdgeType>(v1);
            sut[e1] = v2;
            sut[other] = values;

            var sut2 = new Vertex<TestVertexType, TestEdgeType>(v1);
            sut2[e1] = v2;
            sut2[e2] = v3;

            sut.EquivalentTo(sut2).Should().Be(false);
        }
        [TestMethod]
        public void CanTestNonequivalence2_Objects()
        {
            var e1 = new TestEdgeType(1);
            var v1 = new TestVertexType(1);
            var v2 = new TestVertexType(2);
            var other = new TestEdgeType(2);
            var values = new TestVertexType(2);

            var sut = new Vertex<TestVertexType, TestEdgeType>(v1);
            sut[e1] = v2;
            sut[other] = values;

            var sut2 = new Vertex<TestVertexType, TestEdgeType>(v1);

            sut.EquivalentTo(sut2).Should().Be(false);
        }
        [TestMethod]
        public void CanTestNonequivalence3_Objects()
        {
            var e1 = new TestEdgeType(1);
            var e2 = new TestEdgeType(2);
            var v1 = new TestVertexType(1);
            var v2 = new TestVertexType(2);
            var v3 = new TestVertexType(3);

            var sut = new Vertex<TestVertexType, TestEdgeType>(v1);

            var sut2 = new Vertex<TestVertexType, TestEdgeType>(v1);
            sut2[e1] = v2;
            sut2[e2] = v3;

            sut.EquivalentTo(sut2).Should().Be(false);
        }
        [TestMethod]
        public void CanTestNonequivalence4_Objects()
        {
            var v1 = new TestVertexType(1);
            var other = new TestVertexType(2);

            var sut = new Vertex<TestVertexType, TestEdgeType>(v1);
            var sut2 = new Vertex<TestVertexType, TestEdgeType>(other);
            sut.EquivalentTo(sut2).Should().Be(false);
        }

        [TestMethod]
        public void CanTestNonequivalence5_Objects()
        {
            var e1 = new TestEdgeType(1);
            var e2 = new TestEdgeType(2);
            var v1 = new TestVertexType(1);
            var v2 = new TestVertexType(2);
            var v3 = new TestVertexType(3);
            var different = new TestVertexType(7);

            var sut = new Vertex<TestVertexType, TestEdgeType>(v1);
            sut[e1] = v2;
            sut[e2] = different;

            var sut2 = new Vertex<TestVertexType, TestEdgeType>(v1);
            sut2[e1] = v2;
            sut2[e2] = v3;

            sut.EquivalentTo(sut2).Should().Be(false);
        }

    }
}
