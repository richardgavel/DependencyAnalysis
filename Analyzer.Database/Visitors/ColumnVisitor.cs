using Microsoft.SqlServer.Dac.Model;
using Neo4jClient;
using Nodes = Analyzer.Model.Nodes;

namespace Analyzer.Database.Visitors
{
    public class ColumnVisitor
    {
        private GraphClient client;

        public ColumnVisitor(GraphClient client)
        {
            this.client = client;
        }
    
        public NodeReference Visit(TSqlObject column)
        {
            var columnNode = client.Create(new Nodes.Column
            {
                Id = column.Name.ToString(),
                Name = column.Name.ToString()
            });

            return columnNode;
        }
    }
}
