using Neo4jClient;
using System;
using System.Reflection;
using Nodes = Analyzer.Model.Nodes;

namespace Analyzer.Reflection.Visitors
{
    public class FieldVisitor
    {
        private GraphClient client;

        public FieldVisitor(GraphClient client)
        {
            this.client = client;
        }

        public NodeReference Visit(FieldInfo field)
        {
            return client.Create(new Nodes.Field
            {
                Id = field.Name,
                Name = field.Name
            });
        }
    }
}
