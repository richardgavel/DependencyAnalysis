using Analyzer.Model.Relationships;
using Neo4jClient;
using System;
using System.Linq;
using System.Reflection;
using Nodes = Analyzer.Model.Nodes;

namespace Analyzer.Reflection.Visitors
{
    public class AssemblyVisitor
    {
        private readonly GraphClient _graphClient;

        public AssemblyVisitor(GraphClient client)
        {
            _graphClient = client;
        }

        public NodeReference Visit(string path)
        {
            return Visit(Assembly.ReflectionOnlyLoadFrom(path));
        }

        public NodeReference Visit(Assembly assembly)
        {
            Console.WriteLine("Discovered assembly {0}", assembly.FullName);

            var assemblyNode = _graphClient.Create(new Nodes.Assembly
            {
                Id = assembly.FullName,
                Name = assembly.GetName().Name
            });


            Type[] types;
            try
            {
                types = assembly.GetTypes();
            }
            catch (ReflectionTypeLoadException ex)
            {
                types = ex.Types;
            }

            var classVisitor = new ClassVisitor(_graphClient);
            foreach (var classNode in types.Where(x => x != null && x.IsClass).Select(classVisitor.Visit))
                _graphClient.CreateRelationship(assemblyNode, new AssemblyContainsClass(classNode));

            var interfaceVisitor = new InterfaceVisitor(_graphClient);
            foreach (var interfaceNode in types.Where(x => x != null && x.IsInterface).Select(interfaceVisitor.Visit))
                _graphClient.CreateRelationship(assemblyNode, new AssemblyContainsInterface(interfaceNode));

            return assemblyNode;
        }
    }
}
