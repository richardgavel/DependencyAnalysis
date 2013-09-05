using System.IO;
using Analyzer.Model.Relationships;
using Microsoft.SqlServer.Dac.Model;
using Neo4jClient;
using System;
using System.Linq;
using Nodes = Analyzer.Model.Nodes;

namespace Analyzer.Database.Visitors
{
    public class ViewDependencyVisitor
    {
        private readonly GraphClient _graphClient;

        public ViewDependencyVisitor(GraphClient graphClient)
        {
            _graphClient = graphClient;
        }

        public void Visit(string databasePackagePath)
        {
            var model = new TSqlModel(databasePackagePath);
            var databaseNode = GetDatabaseNode(databasePackagePath);

            foreach (var view in model.GetObjects(DacQueryScopes.All, ModelSchema.View))
                Visit(databaseNode, view);
        }

        public void Visit(Node<Nodes.Database> databaseNode, TSqlObject view)
        {
            var viewNode = GetViewNode(databaseNode, view);

            foreach (var table in view.GetReferenced().Where(x => x.ObjectType == ModelSchema.Table))
            {
                try
                {
                    var tableNode = GetTableNode(databaseNode, table);

                    if ((viewNode == null) || (tableNode == null)) continue;

                    if (RelationshipExists(viewNode.Reference, tableNode.Reference)) continue;

                    Console.WriteLine("Creating dependency between view {0} and table {1}", viewNode.Data.Id, tableNode.Data.Id);
                    _graphClient.CreateRelationship(viewNode.Reference, new ViewReferencesTable(tableNode.Reference));
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error analyzing {0}: {1}", view.Name.ToString(), ex);
                }
            }

            foreach (var column in view.GetReferenced().Where(x => x.ObjectType == ModelSchema.Column))
            {
                try
                {
                    var columnNode = GetColumnNode(databaseNode, column);

                    if ((viewNode == null) || (columnNode == null)) continue;

                    if (RelationshipExists(viewNode.Reference, columnNode.Reference)) continue;

                    Console.WriteLine("Creating dependency between stored procedure {0} and column {1}", viewNode.Data.Id, columnNode.Data.Id);
                    _graphClient.CreateRelationship(viewNode.Reference, new ViewReferencesColumn(columnNode.Reference));
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error analyzing {0}: {1}", view.Name.ToString(), ex);
                }
            }
        }

        private bool RelationshipExists(IAttachedReference sourceNodeReference, IAttachedReference targetNodeReference)
        {
            var query = _graphClient.Cypher
                .Start(new {source = sourceNodeReference, target = targetNodeReference})
                .Where("source-[*]->target")
                .Return<object>("target");

            return query.Results.Any();
        }

        private Node<Nodes.Database> GetDatabaseNode(string databasePackagePath)
        {
            var databasePackageName = Path.GetFileNameWithoutExtension(databasePackagePath);

            var query = _graphClient.Cypher
                .Start(new { root = _graphClient.RootNode })
                .Match("root-[:ROOT_CONTAINS_DATABASESERVER]->databaseserver-[:DATABASESERVER_CONTAINS_DATABASE]->database")
                .Where((Nodes.Database database) => database.Id == databasePackageName)
                .Return<Node<Nodes.Database>>("database");

            var results = query.Results.ToList();

            return results.Count == 1 ? results.First() : null;
        }

        private Node<Nodes.Table> GetTableNode(Node<Nodes.Database> databaseNode, TSqlObject sqlTable)
        {
            var tableName = sqlTable.Name.ToString();

            var query = _graphClient.Cypher
                .Start(new { database = databaseNode.Reference })
                .Match("database-[:DATABASE_CONTAINS_TABLE]->table")
                .Where((Nodes.Table table) => table.Id == tableName)
                .Return<Node<Nodes.Table>>("table");

            var results = query.Results.ToList();

            return results.Count == 1 ? results.First() : null;
        }

        private Node<Nodes.Column> GetColumnNode(Node<Nodes.Database> databaseNode, TSqlObject sqlColumn)
        {
            var sqlTable = sqlColumn.GetParent();

            if (sqlTable == null) return null;

            var tableName = sqlTable.Name.ToString();
            var columnName = sqlColumn.Name.ToString();

            var query = _graphClient.Cypher
                .Start(new { database = databaseNode.Reference })
                .Match("database-[:DATABASE_CONTAINS_TABLE]->table-[:TABLE_CONTAINS_COLUMN]->column")
                .Where((Nodes.Table table, Nodes.Column column) => table.Id == tableName && column.Id == columnName)
                .Return<Node<Nodes.Column>>("column");

            var results = query.Results.ToList();

            return results.Count == 1 ? results.First() : null;
        }

        private Node<Nodes.View> GetViewNode(Node<Nodes.Database> databaseNode, TSqlObject sqlview)
        {
            var viewName = sqlview.Name.ToString();

            var query = _graphClient.Cypher
                .Start(new { database = databaseNode.Reference })
                .Match("database-[:DATABASE_CONTAINS_VIEW]->view")
                .Where((Nodes.View view) => view.Id == viewName)
                .Return<Node<Nodes.View>>("view");

            var results = query.Results.ToList();

            return results.Count == 1 ? results.First() : null;
        }
    }
}
