using Analyzer.Model.Relationships;
using Neo4jClient;
using System;
using System.Reflection;
using Nodes = Analyzer.Model.Nodes;

namespace Analyzer.Reflection.Visitors
{
    public class InterfaceVisitor
    {
        private GraphClient client;

        public InterfaceVisitor(GraphClient client)
        {
            this.client = client;
        }

        public NodeReference Visit(Type type)
        {
            const BindingFlags BindingFlags = BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static;

            var classNode = client.Create(new Nodes.Interface
            {
                Id = type.Name,
                Name = type.Name
            });

            var propertyVisitor = new PropertyVisitor(client);
            foreach (var property in type.GetProperties(BindingFlags))
            {
                var propertyNode = propertyVisitor.Visit(property);
                client.CreateRelationship(classNode, new InterfaceContainsProperty(propertyNode));
            }

            var methodVisitor = new MethodVisitor(client);
            foreach (var method in type.GetMethods(BindingFlags))
            {
                var methodNode = methodVisitor.Visit(method);
                client.CreateRelationship(classNode, new InterfaceContainsMethod(methodNode));
            }

            return classNode;
        }
    }
}
