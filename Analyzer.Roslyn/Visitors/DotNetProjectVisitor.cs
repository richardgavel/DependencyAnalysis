using Analyzer.Roslyn.SyntaxWalkers;
using Neo4jClient;
using Roslyn.Compilers.CSharp;
using Roslyn.Services;

namespace Analyzer.Roslyn.Visitors
{
    public class DotNetProjectVisitor
    {
        private readonly GraphClient _graphClient;

        public DotNetProjectVisitor(GraphClient graphClient)
        {
            _graphClient = graphClient;
        }

        public void Visit(string path)
        {
            var project = Solution.LoadStandAloneProject(path);
            var compilation = project.GetCompilation();

            foreach (var tree in compilation.SyntaxTrees)
            {
                var methodInvocationWalker = new MethodInvocationWalker(_graphClient, compilation.GetSemanticModel(tree));
                methodInvocationWalker.Visit(tree.GetRoot() as SyntaxNode);

                var methodInvokedStoredProcedureWalker = new MethodInvokesStoredProcedureWalker(_graphClient, compilation.GetSemanticModel(tree));
                methodInvokedStoredProcedureWalker.Visit(tree.GetRoot() as SyntaxNode);
            }
        }
    }
}
