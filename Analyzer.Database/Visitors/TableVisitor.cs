using Analyzer.Model.Relationships;
using Microsoft.SqlServer.Dac.Model;
using Neo4jClient;
using Nodes = Analyzer.Model.Nodes;

namespace Analyzer.Database.Visitors
{
    public class TableVisitor
    {
        private readonly GraphClient _graphClient;

        public TableVisitor(GraphClient graphClient)
        {
            _graphClient = graphClient;
        }

        public NodeReference Visit(TSqlObject table)
        {
            var tableNode = _graphClient.Create(new Nodes.Table
            {
                Id = table.Name.ToString(),
                Name = table.Name.ToString()
            });

            var columnVisitor = new ColumnVisitor(_graphClient);
            foreach (var column in table.GetChildren())
            {
                var columnNode = columnVisitor.Visit(column);
                _graphClient.CreateRelationship(tableNode, new TableContainsColumn(columnNode));
            }

            return tableNode;
        }
    }
}
