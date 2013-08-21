using Neo4jClient;
using System;
using System.Reflection;
using Entities = CodeAnalyzer.Entities;

namespace CodeAnalyzer.Analyzer
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
            return client.Create(new Entities.Field
            {
                Id = field.Name,
                Name = field.Name
            });
        }
    }
}
