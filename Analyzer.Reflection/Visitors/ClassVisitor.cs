using System.Linq;
using Analyzer.Model.Relationships;
using Neo4jClient;
using System;
using System.Reflection;
using Nodes = Analyzer.Model.Nodes;

namespace Analyzer.Reflection.Visitors
{
    public class ClassVisitor
    {
        private readonly GraphClient _graphClient;

        public ClassVisitor(GraphClient client)
        {
            _graphClient = client;
        }

        public NodeReference Visit(Type type)
        {
            const BindingFlags bindingFlags = BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static;

            Console.WriteLine("Discovered class {0}", type.FullName);

            var classNode = _graphClient.Create(new Nodes.Class
            {
                Id = type.FullName,
                Name = type.FullName
            });

            var propertyVisitor = new PropertyVisitor(_graphClient);
            foreach (var propertyNode in type.GetProperties(bindingFlags).Select(propertyVisitor.Visit))
                _graphClient.CreateRelationship(classNode, new ClassContainsProperty(propertyNode));

            var fieldVisitor = new FieldVisitor(_graphClient);
            foreach (var fieldNode in type.GetFields(bindingFlags).Select(fieldVisitor.Visit))
                _graphClient.CreateRelationship(classNode, new ClassContainsField(fieldNode));

            var methodVisitor = new MethodVisitor(_graphClient);
            foreach (var methodNode in type.GetMethods(bindingFlags).Select(methodVisitor.Visit))
                _graphClient.CreateRelationship(classNode, new ClassContainsMethod(methodNode));

            return classNode;
        }
    }
}
