using System.Linq;
using Analyzer.Model.Relationships;
using Neo4jClient;
using System;
using System.Reflection;
using Nodes = Analyzer.Model.Nodes;

namespace Analyzer.Reflection.Visitors
{
    public class InterfaceVisitor
    {
        private readonly GraphClient _graphClient;

        public InterfaceVisitor(GraphClient client)
        {
            _graphClient = client;
        }

        public NodeReference Visit(Type type)
        {
            const BindingFlags bindingFlags = BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static;

            var classNode = _graphClient.Create(new Nodes.Interface
            {
                Id = type.Name,
                Name = type.Name
            });

            var propertyVisitor = new PropertyVisitor(_graphClient);
            foreach (var propertyNode in type.GetProperties(bindingFlags).Select(propertyVisitor.Visit))
                _graphClient.CreateRelationship(classNode, new InterfaceContainsProperty(propertyNode));

            var methodVisitor = new MethodVisitor(_graphClient);
            foreach (var methodNode in type.GetMethods(bindingFlags).Select(methodVisitor.Visit))
                _graphClient.CreateRelationship(classNode, new InterfaceContainsMethod(methodNode));

            return classNode;
        }
    }
}
