using Microsoft.SqlServer.Dac.Model;
using Neo4jClient;
using Nodes = Analyzer.Model.Nodes;

namespace Analyzer.Database.Visitors
{
    public class ViewVisitor
    {
        private readonly GraphClient _graphClient;

        public ViewVisitor(GraphClient graphClient)
        {
            _graphClient = graphClient;
        }

        public NodeReference Visit(TSqlObject view)
        {
            var viewNode = _graphClient.Create(new Nodes.View
            {
                Id = view.Name.ToString(),
                Name = view.Name.ToString()
            });

            return viewNode;
        }
    }
}
