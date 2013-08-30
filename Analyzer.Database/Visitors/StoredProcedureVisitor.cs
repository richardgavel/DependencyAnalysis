using Microsoft.SqlServer.Dac.Model;
using Neo4jClient;
using Nodes = Analyzer.Model.Nodes;

namespace Analyzer.Database.Visitors
{
    public class StoredProcedureVisitor
    {
        private readonly GraphClient _graphClient;

        public StoredProcedureVisitor(GraphClient graphClient)
        {
            _graphClient = graphClient;
        }

        public NodeReference Visit(TSqlObject storedProcedure)
        {
            var storedProcedureNode = _graphClient.Create(new Nodes.StoredProcedure
            {
                Id = storedProcedure.Name.ToString(),
                Name = storedProcedure.Name.ToString()
            });

            return storedProcedureNode;
        }
    }
}
