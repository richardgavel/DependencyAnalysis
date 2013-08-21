using Analyzer.Model.Relationships;
using Microsoft.SqlServer.Dac.Model;
using Neo4jClient;
using Neo4jClient.Cypher;
using System;
using System.Linq;
using Nodes = Analyzer.Model.Nodes;

namespace Analyzer.Database.Visitors
{
    public class StoredProcedureDependencyVisitor
    {
        private GraphClient _graphClient;

        public StoredProcedureDependencyVisitor(GraphClient graphClient)
        {
            _graphClient = graphClient;
        }

        public void Visit(TSqlObject storedProcedure)
        {
            var storedProcedureNode = GetStoredProcedureNode(storedProcedure);

            foreach (var column in storedProcedure.GetReferenced().Where(x => x.ObjectType == ModelSchema.Column))
            {
                try
                {
                    var columnNode = GetColumnNode(column);

                    if ((storedProcedureNode != null) && (columnNode != null))
                        _graphClient.CreateRelationship(storedProcedureNode.Reference, new StoredProcedureReferencesColumn(columnNode.Reference));
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }
        }

        private Node<Nodes.Column> GetColumnNode(TSqlObject column)
        {
            var query = _graphClient.Cypher
                .Start(new { root = _graphClient.RootNode })
                .Match("root-[:ROOT_CONTAINS_DATABASESERVER]->databaseserver-[:DATABASESERVER_CONTAINS_DATABASE]->database-[:DATABASE_CONTAINS_TABLE]->table-[:TABLE_CONTAINS_COLUMN]->column")
                .Where(string.Format("table.Id='{0}' AND column.Id='{1}'", column.GetParent().Name, column.Name))
                .Return<Node<Nodes.Column>>("column");

            var results = query.Results.ToList();

            if (results.Count() == 1)
                return results.First();
            else
                return null;
        }

        private Node<Nodes.StoredProcedure> GetStoredProcedureNode(TSqlObject storedProcedure)
        {
            var query = _graphClient.Cypher
                .Start(new { root = _graphClient.RootNode })
                .Match("root-[:ROOT_CONTAINS_DATABASESERVER]->databaseserver-[:DATABASESERVER_CONTAINS_DATABASE]->database-[:DATABASE_CONTAINS_STOREDPROCEDURE]->storedprocedure")
                .Where(string.Format("storedprocedure.Id='{0}'", storedProcedure.Name))
                .Return<Node<Nodes.StoredProcedure>>("storedprocedure");

            var results = query.Results.ToList();

            if (results.Count() == 1)
                return results.First();
            else
                return null;
        }
}
}
