using Analyzer.Model.Relationships;
using Microsoft.SqlServer.Dac.Model;
using Neo4jClient;
using System.IO;
using Nodes = Analyzer.Model.Nodes;

namespace Analyzer.Database.Visitors
{
    public class DatabaseVisitor
    {
        private GraphClient _graphClient;

        public DatabaseVisitor(GraphClient graphClient)
        {
            _graphClient = graphClient;
        }

        public NodeReference Visit(string databasePackagePath)
        {
            var databaseNode = _graphClient.Create(new Nodes.Database
            {
                Id = Path.GetFileNameWithoutExtension(databasePackagePath),
                Name = Path.GetFileNameWithoutExtension(databasePackagePath)
            });

            var model = new TSqlModel(databasePackagePath);

            var tableVisitor = new TableVisitor(_graphClient);
            foreach (var table in model.GetObjects(DacQueryScopes.All, ModelSchema.Table))
            {
                var tableNode = tableVisitor.Visit(table);
                _graphClient.CreateRelationship(databaseNode, new DatabaseContainsTable(tableNode));
            }

            var storedProcedureVisitor = new StoredProcedureVisitor(_graphClient);
            foreach (var storedProcedure in model.GetObjects(DacQueryScopes.All, ModelSchema.Procedure))
            {
                var storedProcedureNode = storedProcedureVisitor.Visit(storedProcedure);
                _graphClient.CreateRelationship(databaseNode, new DatabaseContainsStoredProcedure(storedProcedureNode));
            }

            var viewVisitor = new ViewVisitor(_graphClient);
            foreach (var view in model.GetObjects(DacQueryScopes.All, ModelSchema.View))
            {
                var viewNode = viewVisitor.Visit(view);
                _graphClient.CreateRelationship(databaseNode, new DatabaseContainsView(viewNode));
            }

            //var storedProcedureDependencyVisitor = new StoredProcedureDependencyVisitor(_graphClient);
            //foreach (var storedProcedure in model.GetObjects(DacQueryScopes.All, ModelSchema.Procedure))
            //{
            //    storedProcedureDependencyVisitor.Visit(storedProcedure);
            //}

            return databaseNode;
        }
    }
}
