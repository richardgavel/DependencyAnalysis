using Microsoft.SqlServer.Dac.Model;
using Neo4jClient;
using Nodes = Analyzer.Model.Nodes;

namespace Analyzer.Database.Visitors
{
    public class ColumnVisitor
    {
        private readonly GraphClient _graphClient;

        public ColumnVisitor(GraphClient graphClient)
        {
            _graphClient = graphClient;
        }
    
        public NodeReference Visit(TSqlObject column)
        {
            var columnNode = _graphClient.Create(new Nodes.Column
            {
                Id = column.Name.ToString(),
                Name = column.Name.ToString()
            });

            return columnNode;
        }
    }
}
