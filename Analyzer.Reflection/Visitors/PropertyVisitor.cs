using Neo4jClient;
using System;
using System.Reflection;
using Nodes = Analyzer.Model.Nodes;

namespace Analyzer.Reflection.Visitors
{
    public class PropertyVisitor
    {
        private readonly GraphClient _graphClient;

        public PropertyVisitor(GraphClient client)
        {
            _graphClient = client;
        }

        public NodeReference Visit(PropertyInfo property)
        {
            if (property.DeclaringType != null)
                Console.WriteLine("Discovered property {0}.{1}", property.DeclaringType.Name, property.Name);

            return _graphClient.Create(new Nodes.Property
            {
                Id = property.Name,
                Name = property.Name
            });
        }
    }
}
