using Analyzer.Model.Relationships;
using Neo4jClient;
using System;
using System.Linq;
using System.Reflection;
using Nodes = Analyzer.Model.Nodes;

namespace Analyzer.Reflection.Visitors
{
    public class AssemblyDependencyVisitor
    {
        private GraphClient _graphClient;

        public AssemblyDependencyVisitor(GraphClient graphClient)
        {
            _graphClient = graphClient;
        }

        public void Visit(string path)
        {
            Visit(Assembly.LoadFrom(path));
        }

        public void Visit(Assembly assembly)
        {
            foreach (var referenceAssembly in assembly.GetReferencedAssemblies())
                CreateAssemblyReferencesAssemblyRelationship(assembly, referenceAssembly);
        }

        private void CreateAssemblyReferencesAssemblyRelationship(Assembly source, AssemblyName target)
        {
            var sourceNode = GetAssemblyNode(source);
            var targetNode = GetAssemblyNode(target);

            if ((sourceNode != null) && (targetNode != null))
                _graphClient.CreateRelationship(sourceNode.Reference, new AssemblyReferencesAssembly(targetNode.Reference));

        }

        private Node<Nodes.Assembly> GetAssemblyNode(Assembly assembly)
        {
            var query = _graphClient.Cypher
                .Start(new { root = _graphClient.RootNode })
                .Match("root-[:ROOT_CONTAINS_ASSEMBLY]->assembly")
                .Where(string.Format("assembly.Id='{0}'", assembly.FullName))
                .Return<Node<Nodes.Assembly>>("assembly");

            var results = query.Results.ToList();

            if (results.Count() == 1)
                return results.First();
            else
                return null;
        }


        private Node<Nodes.Assembly> GetAssemblyNode(AssemblyName assemblyName)
        {
            var query = _graphClient.Cypher
                .Start(new { root = _graphClient.RootNode })
                .Match("root-[:ROOT_CONTAINS_ASSEMBLY]->assembly")
                .Where(string.Format("assembly.Id='{0}'", assemblyName.FullName))
                .Return<Node<Nodes.Assembly>>("assembly");

            var results = query.Results.ToList();

            if (results.Count() == 1)
                return results.First();
            else
                return null;
        }
    }
}
