using Neo4jClient;
using System;
using System.Reflection;
using Entities = CodeAnalyzer.Entities;

namespace CodeAnalyzer.Analyzer
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
            return client.Create(new Entities.Method
            {
                Id = method.Name,
                Name = method.Name
            });
        }
    }
}
