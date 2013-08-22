using Analyzer.Model.Nodes;
using Analyzer.Model.Relationships;
using Neo4jClient;
using Roslyn.Compilers.Common;
using Roslyn.Compilers.CSharp;
using System;
using System.Linq;

namespace Analyzer.Roslyn.SyntaxWalkers
{
    public class MethodInvokesStoredProcedureWalker : SyntaxWalker
    {
        private GraphClient _graphClient;
        private ISemanticModel _semanticModel;

        public MethodInvokesStoredProcedureWalker(GraphClient graphClient, ISemanticModel semanticModel)
        {
            _graphClient = graphClient;
            _semanticModel = semanticModel;
        }

        /// <summary>
        /// Look for patterns of obj.CommandText = "[some string]" and attempt to create a relationship between the containing method
        /// procedure in [some string]
        /// </summary>
        public override void VisitBinaryExpression(BinaryExpressionSyntax node)
        {
            if (node.Left is MemberAccessExpressionSyntax)
            {
                var memberAccess = (MemberAccessExpressionSyntax)node.Left;
                if (memberAccess.Name.Identifier.ValueText == "CommandText")
                {
                    /// TODO: Cover where node.Right is a reference to a constant field
                    if (node.Right is LiteralExpressionSyntax)
                    {
                        var literalExpression = (LiteralExpressionSyntax)node.Right;
                        var storedProcedureName = literalExpression.Token.ValueText;
                        var parent = node.Ancestors().First(x => x is MethodDeclarationSyntax);
                        var source = _semanticModel.GetDeclaredSymbol(parent);
                        CreateMethodInvokesStoredProcedureRelationship(source, storedProcedureName);
                    }
                }
            }

            base.VisitBinaryExpression(node);
        }

        public override void VisitInvocationExpression(InvocationExpressionSyntax node)
        {
            try
            {
                var target = _semanticModel.GetSymbolInfo(node.Expression);

                if (target.Symbol.Name == "SetCommandType")
                {
                    var commandTypeParameter = node.ArgumentList.Arguments[1].Expression as MemberAccessExpressionSyntax;
                    if (commandTypeParameter.Name.Identifier.ValueText == "StoredProcedure")
                    {
                        var storedProcedureName = _semanticModel.GetConstantValue(node.ArgumentList.Arguments[2].Expression).Value.ToString();
                        var parent = node.Ancestors().First(x => x is MethodDeclarationSyntax);
                        var source = _semanticModel.GetDeclaredSymbol(parent);
                        CreateMethodInvokesStoredProcedureRelationship(source, storedProcedureName);
                    }
                }
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

        public Node<StoredProcedure> GetStoredProcedureNode(string storedProcedureName)
        {
            var query = _graphClient.Cypher
                .Start(new { root = _graphClient.RootNode })
                .Match("root-[:ROOT_CONTAINS_DATABASESERVER]->databaseserver-[:DATABASESERVER_CONTAINS_DATABASE]->database-[:DATABASE_CONTAINS_STOREDPROCEDURE]->storedprocedure")
                .Where(string.Format("storedprocedure.Id='[dbo].[{0}]'", storedProcedureName))
                .Return<Node<StoredProcedure>>("storedprocedure");

            var results = query.Results.ToList();

            if (results.Count() == 1)
                return results.First();
            else
                return null;
        }

        private void CreateMethodInvokesStoredProcedureRelationship(ISymbol source, string target)
        {
            var sourceNode = GetMethodNode(source);
            var targetNode = GetStoredProcedureNode(target);

            if ((sourceNode != null) && (targetNode != null))
                _graphClient.CreateRelationship(sourceNode.Reference, new MethodInvokesStoredProcedure(targetNode.Reference));

        }
    }
}
