using Neo4jClient;
using System;
using System.Reflection;
using Entities = CodeAnalyzer.Entities;

namespace CodeAnalyzer.Analyzer
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
            return client.Create(new Entities.Property
            {
                Id = property.Name,
                Name = property.Name
            });
        }
    }
}
