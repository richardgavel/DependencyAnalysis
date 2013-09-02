using Microsoft.SqlServer.Dac.Model;
using Neo4jClient;
using System;
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
            Console.WriteLine("Discovered view {0}", view.Name);

            var viewNode = _graphClient.Create(new Nodes.View
            {
                Id = view.Name.ToString(),
                Name = view.Name.ToString()
            });

            return viewNode;
        }
    }
}
