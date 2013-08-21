using Neo4jClient;
using System;
using System.Reflection;
using Nodes = Analyzer.Model.Nodes;

namespace Analyzer.Reflection.Visitors
{
    public class PropertyVisitor
    {
        private GraphClient client;

        public PropertyVisitor(GraphClient client)
        {
            this.client = client;
        }

        public NodeReference Visit(PropertyInfo property)
        {
            return client.Create(new Nodes.Property
            {
                Id = property.Name,
                Name = property.Name
            });
        }
    }
}
