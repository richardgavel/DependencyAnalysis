using System.IO;
using System.Linq;
using System.Xml.Linq;
using Analyzer.Model.Nodes;
using Analyzer.Model.Relationships;
using Neo4jClient;

namespace Analyzer.Reports.Visitors
{
    public class ReportVisitor
    {
        private readonly GraphClient _graphClient;

        public ReportVisitor(GraphClient graphClient)
        {
            _graphClient = graphClient;
        }

        public NodeReference Visit(string path)
        {
            var reportNode = _graphClient.Create(new Report
            {
                Id = Path.GetFileNameWithoutExtension(path),
                Name = Path.GetFileNameWithoutExtension(path)
            });

            var root = XDocument.Load(path);

            var storedProcedureNames = root
                                .Descendants("{http://schemas.microsoft.com/sqlserver/reporting/2008/01/reportdefinition}Query")
                                .Where(x => (string)x.Element("{http://schemas.microsoft.com/sqlserver/reporting/2008/01/reportdefinition}CommandType") == "StoredProcedure")
                                .Select(x => (string)x.Element("{http://schemas.microsoft.com/sqlserver/reporting/2008/01/reportdefinition}CommandText"));


            foreach (var storedProcedureName in storedProcedureNames)
            {
                var fullyQualifiedStoredProcedureName = string.Format("[dbo].[{0}]", storedProcedureName);
                var storedProcedureQuery = _graphClient.Cypher
                    .Start(new { root = _graphClient.RootNode })
                    .Match("root-[:ROOT_CONTAINS_DATABASESERVER]->databaseServer-[:DATABASESERVER_CONTAINS_DATABASE]->database-[:DATABASE_CONTAINS_STOREDPROCEDURE]->storedprocedure")
                    .Where("storedprocedure.Id = '" + fullyQualifiedStoredProcedureName + "'")
                    .Return<Node<StoredProcedure>>("storedprocedure");

                var results = storedProcedureQuery.Results.ToList();

                if (results.Count() == 1)
                    _graphClient.CreateRelationship(reportNode, new ReportInvokesStoredProcedure(results.First().Reference));
            }

            return reportNode;
        }
    }
}
