using CodeAnalyzer.Relationships;
using Neo4jClient;
using System;
using System.Linq;
using System.Reflection;
using Reflection = System.Reflection;

namespace CodeAnalyzer.Analyzer
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
            var assembly = Reflection.Assembly.LoadFrom(path);

            var assemblyNode = client.Create(new Entities.Assembly
            {
                Id = assembly.FullName,
                Name = assembly.GetName().Name
            });

            var classVisitor = new ClassVisitor(client);

            Type[] types;
            try
            {
                types = assembly.GetTypes();
            }
            catch (ReflectionTypeLoadException ex)
            {
                types = ex.Types;
            }

            foreach (var @class in types.Where(x => x != null && x.IsClass))
            {
                var classNode = classVisitor.Visit(@class);
                client.CreateRelationship(assemblyNode, new AssemblyContainsClass(classNode));
            }

            return assemblyNode;
        }

        public NodeReference Visit(Reflection.Assembly assembly)
        {
            throw new NotImplementedException();
        }
    }
}
