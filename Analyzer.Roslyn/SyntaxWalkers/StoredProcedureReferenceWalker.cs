using Analyzer.Model.Nodes;
using Analyzer.Model.Relationships;
using Neo4jClient;
using Roslyn.Compilers.CSharp;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Analyzer.Roslyn.SyntaxWalkers
{
    public class StoredProcedureReferenceWalker : SyntaxWalker
    {
        private GraphClient _graphClient;
        private Dictionary<string, Node<StoredProcedure>> _targets = new Dictionary<string,Node<StoredProcedure>>();

        public StoredProcedureReferenceWalker(GraphClient graphClient, IEnumerable<Node<StoredProcedure>> targets)
        {
            _graphClient = graphClient;
            
            foreach (var node in targets)
                _targets.Add(node.Data.Name, node);
        }

        public override void VisitLiteralExpression(LiteralExpressionSyntax node)
        {
            var criteria = node.Token.ValueText;
            criteria = string.Format("[dbo].[{0}]", criteria);

            if (_targets.ContainsKey(criteria))
            {
                var storedProcedure = _targets[criteria];

                ClassDeclarationSyntax classSyntax = (ClassDeclarationSyntax)node.Ancestors(true).Where(x => x is ClassDeclarationSyntax).FirstOrDefault();
                NamespaceDeclarationSyntax namespaceSyntax = (NamespaceDeclarationSyntax)classSyntax.Ancestors(true).Where(x => x is NamespaceDeclarationSyntax).FirstOrDefault();
                var classQualifiedName = string.Format("{0}.{1}", namespaceSyntax.Name.ToString(), classSyntax.Identifier.ValueText);
                var query = _graphClient.Cypher
                    .Start(new { root = _graphClient.RootNode })
                    .Match("root-[:ROOT_CONTAINS_ASSEMBLY]->assembly-[:ASSEMBLY_CONTAINS_CLASS]->class")
                    .Where((Class @class) => @class.Id == classQualifiedName)
                    .Return<Node<Class>>("class");
                var results = query.Results.ToList();

                if (results.Count() == 1)
                    _graphClient.CreateRelationship(results.First().Reference, new ClassReferencesStoredProcedure(storedProcedure.Reference));
            }

            base.VisitLiteralExpression(node);
        }
    }
}
