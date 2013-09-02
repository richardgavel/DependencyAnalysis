using Neo4jClient;
using System;
using System.Reflection;
using Nodes = Analyzer.Model.Nodes;

namespace Analyzer.Reflection.Visitors
{
    public class MethodVisitor
    {
        private readonly GraphClient _graphClient;

        public MethodVisitor(GraphClient client)
        {
            _graphClient = client;
        }

        public NodeReference Visit(MethodInfo method)
        {
            if (method.DeclaringType != null)
                Console.WriteLine("Discovered method {0}.{1}", method.DeclaringType.Name, method.Name);

            return _graphClient.Create(new Nodes.Method
            {
                Id = method.Name,
                Name = method.Name
            });
        }
    }
}
