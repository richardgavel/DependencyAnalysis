using Analyzer.Model.Relationships;
using Microsoft.SqlServer.Dac.Model;
using Neo4jClient;
using Nodes = Analyzer.Model.Nodes;

namespace Analyzer.Database.Visitors
{
    public class TableVisitor
    {
        private GraphClient client;

        public TableVisitor(GraphClient client)
        {
            this.client = client;
        }

        public NodeReference Visit(TSqlObject table)
        {
            var tableNode = client.Create(new Nodes.Table
            {
                Id = table.Name.ToString(),
                Name = table.Name.ToString()
            });

            var columnVisitor = new ColumnVisitor(client);
            foreach (var column in table.GetChildren())
            {
                var columnNode = columnVisitor.Visit(column);
                client.CreateRelationship(tableNode, new TableContainsColumn(columnNode));
            }

            return tableNode;
        }
    }
}
