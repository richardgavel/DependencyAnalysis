using Analyzer.Model.Relationships;
using Neo4jClient;
using System;
using System.Reflection;
using Nodes = Analyzer.Model.Nodes;

namespace Analyzer.Reflection.Visitors
{
    public class ClassVisitor
    {
        private GraphClient client;

        public ClassVisitor(GraphClient client)
        {
            this.client = client;
        }

        public NodeReference Visit(Type type)
        {
            const BindingFlags BindingFlags = BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static;

            var classNode = client.Create(new Nodes.Class
            {
                Id = type.FullName,
                Name = type.FullName
            });

            var propertyVisitor = new PropertyVisitor(client);
            foreach (var property in type.GetProperties(BindingFlags))
            {
                var propertyNode = propertyVisitor.Visit(property);
                client.CreateRelationship(classNode, new ClassContainsProperty(propertyNode));
            }

            var fieldVisitor = new FieldVisitor(client);
            foreach (var field in type.GetFields(BindingFlags))
            {
                var fieldNode = fieldVisitor.Visit(field);
                client.CreateRelationship(classNode, new ClassContainsField(fieldNode));
            }

            var methodVisitor = new MethodVisitor(client);
            foreach (var method in type.GetMethods(BindingFlags))
            {
                var methodNode = methodVisitor.Visit(method);
                client.CreateRelationship(classNode, new ClassContainsMethod(methodNode));
            }

            return classNode;
        }
    }
}
