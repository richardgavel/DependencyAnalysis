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
        private GraphClient client;

        public AssemblyVisitor(GraphClient client)
        {
            this.client = client;
        }

        public NodeReference Visit(string path)
        {
            return Visit(Assembly.LoadFrom(path));
        }

        public NodeReference Visit(Assembly assembly)
        {
            var assemblyNode = client.Create(new Nodes.Assembly
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

            var classVisitor = new ClassVisitor(client);
            foreach (var @class in types.Where(x => x != null && x.IsClass))
            {
                var classNode = classVisitor.Visit(@class);
                client.CreateRelationship(assemblyNode, new AssemblyContainsClass(classNode));
            }

            var interfaceVisitor = new InterfaceVisitor(client);
            foreach (var @interface in types.Where(x => x != null && x.IsInterface))
            {
                var interfaceNode = interfaceVisitor.Visit(@interface);
                client.CreateRelationship(assemblyNode, new AssemblyContainsInterface(interfaceNode));
            }

            return assemblyNode;
        }
    }
}
