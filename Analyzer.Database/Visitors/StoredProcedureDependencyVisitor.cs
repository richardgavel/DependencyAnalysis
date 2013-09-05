using System.IO;
using System.Text.RegularExpressions;
using Analyzer.Model.Relationships;
using Microsoft.SqlServer.Dac.Model;
using Neo4jClient;
using System;
using System.Linq;
using Neo4jClient.Cypher;
using Nodes = Analyzer.Model.Nodes;

namespace Analyzer.Database.Visitors
{
    public class StoredProcedureDependencyVisitor
    {
        private static readonly Regex FromRegex = new Regex(@"FROM\s+(?<table>\S+)\s", RegexOptions.IgnoreCase | RegexOptions.Multiline);
        private static readonly Regex JoinRegex = new Regex(@"JOIN\s+(?<table>\S+)\s", RegexOptions.IgnoreCase | RegexOptions.Multiline);

        private readonly GraphClient _graphClient;

        public StoredProcedureDependencyVisitor(GraphClient graphClient)
        {
            _graphClient = graphClient;
        }

        public void Visit(string databasePackagePath)
        {
            var model = new TSqlModel(databasePackagePath);
            var databaseNode = GetDatabaseNode(databasePackagePath);

            foreach (var storedProcedure in model.GetObjects(DacQueryScopes.All, ModelSchema.Procedure))
                Visit(databaseNode, storedProcedure);
        }

        public void Visit(Node<Nodes.Database> databaseNode, TSqlObject storedProcedure)
        {
            var storedProcedureNode = GetStoredProcedureNode(databaseNode, storedProcedure);

            foreach (var table in storedProcedure.GetReferenced().Where(x => x.ObjectType == ModelSchema.Table))
            {
                try
                {
                    var tableNode = GetTableNode(databaseNode, table);

                    if ((storedProcedureNode == null) || (tableNode == null)) continue;

                    if (RelationshipExists(storedProcedureNode.Reference, tableNode.Reference)) continue;

                    Console.WriteLine("Creating dependency between stored procedure {0} and table {1}", storedProcedureNode.Data.Id, tableNode.Data.Id);
                    _graphClient.CreateRelationship(storedProcedureNode.Reference, new StoredProcedureReferencesTable(tableNode.Reference));
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error analyzing {0}: {1}", storedProcedure.Name.ToString(), ex);
                }
            }

            foreach (var column in storedProcedure.GetReferenced().Where(x => x.ObjectType == ModelSchema.Column))
            {
                try
                {
                    var columnNode = GetColumnNode(databaseNode, column);

                    if ((storedProcedureNode == null) || (columnNode == null)) continue;

                    if (RelationshipExists(storedProcedureNode.Reference, columnNode.Reference)) continue;

                    Console.WriteLine("Creating dependency between stored procedure {0} and column {1}", storedProcedureNode.Data.Id, columnNode.Data.Id);
                    _graphClient.CreateRelationship(storedProcedureNode.Reference, new StoredProcedureReferencesColumn(columnNode.Reference));
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error analyzing {0}: {1}", storedProcedure.Name.ToString(), ex);
                }
            }

            // Check for dynamic SQL
            var script = storedProcedure.GetScript();
            if (!script.Contains("EXEC")) return;

            foreach (Match match in FromRegex.Matches(script))
            {
                try
                {
                    var tableNode = GetTableNode(databaseNode, string.Format("[dbo].[{0}]", match.Groups["table"].Value));

                    if ((storedProcedureNode == null) || (tableNode == null)) continue;

                    if (RelationshipExists(storedProcedureNode.Reference, tableNode.Reference)) continue;

                    Console.WriteLine("Creating dependency between stored procedure {0} and table {1} (dynamic SQL)", storedProcedureNode.Data.Id, tableNode.Data.Id);
                    _graphClient.CreateRelationship(storedProcedureNode.Reference, new StoredProcedureReferencesTable(tableNode.Reference));
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error analyzing {0}: {1}", storedProcedure.Name.ToString(), ex);
                }
            }

            foreach (Match match in JoinRegex.Matches(script))
            {
                try
                {
                    var tableNode = GetTableNode(databaseNode, string.Format("[dbo].[{0}]", match.Groups["table"].Value));

                    if ((storedProcedureNode == null) || (tableNode == null)) continue;

                    if (RelationshipExists(storedProcedureNode.Reference, tableNode.Reference)) continue;

                    Console.WriteLine("Creating dependency between stored procedure {0} and table {1} (dynamic SQL)", storedProcedureNode.Data.Id, tableNode.Data.Id);
                    _graphClient.CreateRelationship(storedProcedureNode.Reference, new StoredProcedureReferencesTable(tableNode.Reference));
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error analyzing {0}: {1}", storedProcedure.Name.ToString(), ex);
                }
            }
        }

        private bool RelationshipExists(IAttachedReference sourceNodeReference, IAttachedReference targetNodeReference)
        {
            var query = _graphClient.Cypher
                .Start(new {source = sourceNodeReference, target = targetNodeReference})
                .Where("source-->target")
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

        private Node<Nodes.Table> GetTableNode(Node<Nodes.Database> databaseNode, string tableName)
        {
            var query = _graphClient.Cypher
                .Start(new { database = databaseNode.Reference })
                .Match("database-[:DATABASE_CONTAINS_TABLE]->table")
                .Where(string.Format("table.Id =~ '(?i){0}'", tableName.Replace("[", "\\\\[").Replace("]", "\\\\]").Replace(".", "\\\\.")))
                .Return<Node<Nodes.Table>>("table");

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

        private Node<Nodes.StoredProcedure> GetStoredProcedureNode(Node<Nodes.Database> databaseNode, TSqlObject sqlStoredProcedure)
        {
            var storedProcedureName = sqlStoredProcedure.Name.ToString();

            var query = _graphClient.Cypher
                .Start(new { database = databaseNode.Reference })
                .Match("database-[:DATABASE_CONTAINS_STOREDPROCEDURE]->storedprocedure")
                .Where((Nodes.StoredProcedure storedprocedure) => storedprocedure.Id == storedProcedureName)
                .Return<Node<Nodes.StoredProcedure>>("storedprocedure");

            var results = query.Results.ToList();

            return results.Count == 1 ? results.First() : null;
        }
    }
}
