using Analyzer.Model.Nodes;
using Analyzer.Model.Relationships;
using Neo4jClient;
using Roslyn.Compilers.Common;
using Roslyn.Compilers.CSharp;
using System;
using System.Linq;

namespace Analyzer.Roslyn.SyntaxWalkers
{
    public class MethodInvocationWalker : SyntaxWalker
    {
        private GraphClient _graphClient;
        private ISemanticModel _semanticModel;

        public MethodInvocationWalker(GraphClient graphClient, ISemanticModel semanticModel)
        {
            _graphClient = graphClient;
            _semanticModel = semanticModel;
        }

        public override void VisitInvocationExpression(InvocationExpressionSyntax node)
        {
            try
            {
                var target = _semanticModel.GetSymbolInfo(node.Expression);

                var parent = node.Ancestors().First(x => x is ClassDeclarationSyntax || x is MethodDeclarationSyntax);
                if (parent is ClassDeclarationSyntax)
                {
                    var source = _semanticModel.GetDeclaredSymbol(parent);
                }
                else
                    CreateMethodInvokesMethodRelationship(_semanticModel.GetDeclaredSymbol(parent), target.Symbol);

            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex);
            }

            base.VisitInvocationExpression(node);
        }

        private Node<Method> GetMethodNode(ISymbol symbol)
        {
            var query = _graphClient.Cypher
                .Start(new { root = _graphClient.RootNode })
                .Match("root-[:ROOT_CONTAINS_ASSEMBLY]->assembly-[:ASSEMBLY_CONTAINS_CLASS]->class-[:CLASS_CONTAINS_METHOD]->method")
                .Where(string.Format("class.Id='{0}' AND method.Id='{1}'", symbol.ContainingType.ToString(), symbol.Name))
                .Return<Node<Method>>("method");

            var results = query.Results.ToList();

            if (results.Count() == 1)
                return results.First();
            else
                return null;
        }

        private void CreateMethodInvokesMethodRelationship(ISymbol source, ISymbol target)
        {
            var sourceNode = GetMethodNode(source);
            var targetNode = GetMethodNode(target);

            if ((sourceNode != null) && (targetNode != null))
                _graphClient.CreateRelationship(sourceNode.Reference, new MethodInvokesMethod(targetNode.Reference));

        }
    }
}
