using Neo4jClient;
using System.Reflection;
using Nodes = Analyzer.Model.Nodes;

namespace Analyzer.Reflection.Visitors
{
    public class FieldVisitor
    {
        private readonly GraphClient _graphClient;

        public FieldVisitor(GraphClient client)
        {
            _graphClient = client;
        }

        public NodeReference Visit(FieldInfo field)
        {
            return _graphClient.Create(new Nodes.Field
            {
                Id = field.Name,
                Name = field.Name
            });
        }
    }
}
