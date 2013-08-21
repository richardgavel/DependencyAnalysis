using Microsoft.SqlServer.Dac.Model;
using Neo4jClient;
using Nodes = Analyzer.Model.Nodes;

namespace Analyzer.Database.Visitors
{
    public class ViewVisitor
    {
        private GraphClient client;

        public ViewVisitor(GraphClient client)
        {
            this.client = client;
        }

        public NodeReference Visit(TSqlObject view)
        {
            var viewNode = client.Create(new Nodes.View
            {
                Id = view.Name.ToString(),
                Name = view.Name.ToString()
            });

            return viewNode;
        }
    }
}
