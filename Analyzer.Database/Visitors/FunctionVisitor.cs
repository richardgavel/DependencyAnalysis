using Microsoft.SqlServer.Dac.Model;
using Neo4jClient;
using System;
using Nodes = Analyzer.Model.Nodes;

namespace Analyzer.Database.Visitors
{
    public class UserDefinedFunctionVisitor
    {
        private readonly GraphClient _graphClient;

        public UserDefinedFunctionVisitor(GraphClient graphClient)
        {
            _graphClient = graphClient;
        }

        public NodeReference Visit(TSqlObject userDefinedFunction)
        {
            Console.WriteLine("Discovered user defined function {0}", userDefinedFunction.Name);

            var userDefinedFunctionNode = _graphClient.Create(new Nodes.UserDefinedFunction
            {
                Id = userDefinedFunction.Name.ToString(),
                Name = userDefinedFunction.Name.ToString()
            });

            return userDefinedFunctionNode;
        }
    }
}
