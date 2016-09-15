using System;
using System.Linq;
using System.Text;

namespace Modd.visitors
{
    using DTType = DecisionTree<BaseDtVertexType, DtBranchTest>;
    using EdgeType = QuickGraph.TaggedEdge<BaseDtVertexType, DtBranchTest>;
    using GraphType = QuickGraph.AdjacencyGraph<BaseDtVertexType, QuickGraph.TaggedEdge<BaseDtVertexType, DtBranchTest>>;

    public class CSharpCodeGenerator
    {
        public MetadataConfiguration Config { get; private set; }

        public string GenerateSource(MetadataConfiguration config)
        {
            if (config == null)
                throw new ArgumentNullException(nameof(config));
            this.Config = config;
            var builder = new DecisionTreeBuilder(config);
            var dt = builder.CreateReducedTree();
            var sb = new StringBuilder();
            GenerateModuleHeader(sb, dt);
            GenerateFunctionHeader(sb, dt);
            GenerateFunctionExpression(sb, dt);
            GenerateFunctionFooter(sb, dt);
            GenerateModuleFooter(sb, dt);
            return sb.ToString();
        }

        private void GenerateModuleFooter(StringBuilder sb, DecisionTree<BaseDtVertexType, DtBranchTest> dt)
        {
            sb.Append("} // end class\n} // end namespace\n");
        }

        private void GenerateFunctionFooter(StringBuilder sb, DecisionTree<BaseDtVertexType, DtBranchTest> dt)
        {
            sb.Append("} // end function\n");
        }

        private void GenerateFunctionExpression(StringBuilder sb,
            DecisionTree<BaseDtVertexType, DtBranchTest> dt)
        {
            GenerateVertexExpression(sb, dt.Tree.Root(), dt.Tree);
        }

        private void GenerateVertexExpression(StringBuilder sb, BaseDtVertexType v, GraphType g)
        {
            if (v is DtOutcome)
            {
                var o = v as DtOutcome;
                sb.AppendFormat("return \"{0}\";\n", o.OutcomeValue);
            }
            if (v is DtTest)
            {
                foreach (var edge in g.OutEdges(v))
                {
                    GenerateEdgeExpression(sb, edge, g);
                }
            }
        }

        private void GenerateEdgeExpression(StringBuilder sb, EdgeType e, GraphType g)
        {
            var vertexVariableName = GetVariableNameForEdgeSource(e);
            var outcomeComparison = GetOutcomeComparisonForLabel(e);
            sb.Append(
                $"if({vertexVariableName} == \"{outcomeComparison}\"){{\n"
                );
            GenerateVertexExpression(sb, e.Target, g);
            sb.Append("}\n");
        }

        private string GetOutcomeComparisonForLabel(EdgeType e)
        {
            return e.Tag.TestValue.Value.ToString();
        }

        private string GetVariableNameForEdgeSource(EdgeType e)
        {
            var t = e.Source as DtTest;
            if (t != null)
            {
                return CreateVarName(t.Attribute.Name);
            }
            throw new DecisionException("Outcome can never be the parent of any other node.");
        }

        private void GenerateFunctionHeader(StringBuilder sb, DTType dt)
        {
            var attrs = dt.SymbolTable.DecisionMetadata.Attributes.Select(a => "string " + CreateVarName(a.Name));
            var args = string.Join(", ", attrs.ToArray());
            sb.AppendFormat("\nstring {1}({0})\n{{\n", args, Config.FunctionName ?? "ShouldAccept");
        }

        private string CreateVarName(string name)
        {
            var invalidChars = name.Where(c => !Char.IsLetterOrDigit(c)).Distinct();
            var result = name;
            foreach (var c in invalidChars)
            {
                result = result.Replace(c, '_');
            }
            return "_" + result.ToLowerInvariant();
        }

        private void GenerateModuleHeader(StringBuilder sb, DTType dt)
        {
            sb.AppendFormat(@"
using System;
using Modd;

namespace {0} {{
    class {1} {{
", Config.Namespace ?? "Some.Namespace",
Config.ClassName ?? "SomeClass"
);
        }
    }
}