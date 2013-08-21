using Neo4jClient;
using System;
using System.Reflection;
using Nodes = Analyzer.Model.Nodes;

namespace Analyzer.Reflection.Visitors
{
    public class MethodVisitor
    {
        private GraphClient client;

        public MethodVisitor(GraphClient client)
        {
            this.client = client;
        }

        public NodeReference Visit(MethodInfo method)
        {
            return client.Create(new Nodes.Method
            {
                Id = method.Name,
                Name = method.Name
            });
        }
    }
}
