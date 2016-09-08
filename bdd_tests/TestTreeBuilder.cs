
using Modd;
using FluentAssertions;
using NUnit.Framework;
using System.Data;
using System.Diagnostics;
using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Modd.Metadata;

namespace bdd_tests
{
    using GraphType = QuickGraph.AdjacencyGraph<BaseDtVertexType, QuickGraph.TaggedEdge<BaseDtVertexType, DtBranchTest>>;
    [TestFixture]
    public class TestTreeBuilder
    {

        [Test]
        public void CanCreateTreeBuilder()
        {
            var sut = new DecisionTreeBuilder(GetMetadataPath(), "ignore");
            sut.Should().NotBeNull();
        }

        private string GetMetadataPath()
        {
            return System.Environment.GetEnvironmentVariable("BDD_TEST_DATA_DIR") ?? @"C:\dev\binarydecisiontree\bdd_tests\testdata\problem-metadata.xml";
        }

        [Test]
        public void CanRunTreeBuilder()
        {
            var sut = new DecisionTreeBuilder(GetMetadataPath(), "ignore");
            var dt = sut.CreateTree();
        }

        [Test]
        public void CanEvaluateTree()
        {
            var sut = new DecisionTreeBuilder(GetMetadataPath(), "ignore");
            var dt = sut.CreateTree();

            EvaluatesAllTestDataCorrectly(sut, (GraphType)dt.Tree);
        }

        private void EvaluatesAllTestDataCorrectly(DecisionTreeBuilder sut, GraphType g)
        {
            int correct = 0, total = 0;

            foreach (DataRow row in sut.SymbolTable.DecisionMetadata.AllSamples)
            {
                var environment = RowToEnv(sut.SymbolTable, row);
                var evaluator = new EvaluatorVisitor(g, environment);
                evaluator.Visit(g.Vertices.First());
                if (evaluator.EvaluatedResult.Equals(row["DecisionOutcome"]))
                {
                    correct++;
                }
                else
                {
                    Debug.Write($"Fail: (Eval: {evaluator.EvaluatedResult})\t");
                    foreach (var c in row.Table.Columns)
                    {
                        var col = c as DataColumn;
                        Debug.Write($"{col.ColumnName}: {row[col.ColumnName]},\t");
                    }
                    Debug.Write("\n");
                }
                total++;
            }
            correct.Should().Be(total);
        }

        private Modd.Environment RowToEnv(SymbolTable st, DataRow row)
        {
            var env = new Modd.Environment(st);
            foreach (var attr in st.DecisionMetadata.Attributes)
            {
                var colName = attr.Name;
                env.Bind(attr.Name, row.Field<string>(colName));
            }
            return env;
        }

        [Test]
        public void CanReduceTreeWithoutChangingFunctionComputed()
        {
            var treeBuilder = new DecisionTreeBuilder(GetMetadataPath(), "Unmatched");
            var decisionTree = treeBuilder.CreateTree();
            //var normaliser = new NormaliserSimplifier(decisionTree.Tree, "Unmatched");
            //normaliser.Visit(decisionTree.Tree.Root());
            var sut = new Reducer();
            EvaluatesAllTestDataCorrectly(treeBuilder, decisionTree.Tree);
            var g = sut.Reduce(treeBuilder.SymbolTable, decisionTree.Tree);
            EvaluatesAllTestDataCorrectly(treeBuilder, decisionTree.Tree);
        }

        [Test]
        public void CanSimplifyTree()
        {
            var sut = new DecisionTreeBuilder(GetMetadataPath(), "Unmatched");
            var dt = sut.CreateTree();
            EvaluatesAllTestDataCorrectly(sut, dt.Tree);

            // first count the number of vertices
            var vertexCounter = new VertexCounterVisitor(dt.Tree);
            vertexCounter.Visit(dt.Tree.Root());
            var initialVertexCount = vertexCounter.Counter;
            Debug.WriteLine($"Vertices: {initialVertexCount}");

            // now start to simplify
            var reducer = new Reducer();
            var reducedTree = reducer.Reduce(sut.SymbolTable,  dt.Tree);

            var dt2 = new DecisionTree<BaseDtVertexType, DtBranchTest>
            {
                Tree = reducedTree
            };

            //// now count vertices in the simplified tree
            vertexCounter.Reset();
            vertexCounter.Visit(dt2.Tree.Root());
            var simplifiedVertexCount = vertexCounter.Counter;
            Debug.WriteLine($"Vertices: {simplifiedVertexCount}");
            simplifiedVertexCount.Should().BeLessThan(initialVertexCount);

            // check that the modified DT still works OK.
            EvaluatesAllTestDataCorrectly(sut, dt2.Tree);
        }

    }

}
