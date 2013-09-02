using Microsoft.SqlServer.Dac.Model;
using Neo4jClient;
using System;
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
            Console.WriteLine("Discovered stored procedure {0}", storedProcedure.Name);

            var storedProcedureNode = _graphClient.Create(new Nodes.StoredProcedure
            {
                Id = storedProcedure.Name.ToString(),
                Name = storedProcedure.Name.ToString()
            });

            return storedProcedureNode;
        }
    }
}
