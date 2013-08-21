using Analyzer.Model.Relationships;
using Microsoft.SqlServer.Dac.Model;
using Neo4jClient;
using Neo4jClient.Cypher;
using System;
using System.Linq;
using Nodes = Analyzer.Model.Nodes;

namespace Analyzer.Database.Visitors
{
    public class StoredProcedureVisitor
    {
        private GraphClient client;

        public StoredProcedureVisitor(GraphClient client)
        {
            this.client = client;
        }

        public NodeReference Visit(TSqlObject storedProcedure)
        {
            var storedProcedureNode = client.Create(new Nodes.StoredProcedure
            {
                Id = storedProcedure.Name.ToString(),
                Name = storedProcedure.Name.ToString()
            });

            //foreach (var columnDependency in storedProcedure.GetReferenced().Where(x => x.ObjectType == ModelSchema.Column))
            //{
            //    var query = client.Cypher
            //        .Start(new { node = All.Nodes })
            //        .Where(string.Format("HAS(node.Id) AND node.Id = '{0}'", columnDependency.Name.ToString()))
            //        .Return<Node<Entities.Column>>("node");

            //    foreach (var columnNode in query.Results)
            //        client.CreateRelationship(storedProcedureNode, new StoredProcedureReferencesColumn(columnNode.Reference));
            //}

            return storedProcedureNode;
        }
    }
}
