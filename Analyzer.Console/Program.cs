using Analyzer.Database.Visitors;
using Analyzer.Model.Nodes;
using Analyzer.Model.Relationships;
using Analyzer.Reflection.Visitors;
using Analyzer.Reports.Visitors;
using Analyzer.Roslyn.Visitors;
using Neo4jClient;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Analyzer.Console
{
    class Program
    {
        private static readonly GraphClient GraphClient = new GraphClient(new Uri("http://localhost:7474/db/data"));

        static void Main(string[] args)
        {
            var arguments = new Dictionary<string, string>();
            for (var index = 0; index < args.Length; index = index + 2)
                arguments.Add(args[index].Substring(1), args[index + 1]);

            GraphClient.Connect();

            if (arguments.ContainsKey("dacpac"))
            {
                var databaseVisitor = new DatabaseVisitor(GraphClient);
                var databaseServerNode = GetDatabaseServerNode(arguments["server"]);

                GraphClient.CreateRelationship(databaseServerNode, new DatabaseServerContainsDatabase(databaseVisitor.Visit(arguments["dacpac"])));
            }
            else if (arguments.ContainsKey("dacpac-references"))
            {
                var storedProcedureDependencyVisitor = new StoredProcedureDependencyVisitor(GraphClient);
                storedProcedureDependencyVisitor.Visit(arguments["dacpac-references"]);
            }
            else if (arguments.ContainsKey("assembly"))
            {
                var assemblyVisitor = new AssemblyVisitor(GraphClient);
                GraphClient.CreateRelationship(GraphClient.RootNode, new RootContainsAssembly(assemblyVisitor.Visit(arguments["assembly"])));
            }
            else if (arguments.ContainsKey("reports"))
            {
                var reportVisitor = new ReportVisitor(GraphClient);
                var reportServerNode = GetReportServerNode(arguments["server"]);

                var directory = new DirectoryInfo(arguments["reports"]);
                foreach (var report in directory.EnumerateFiles("*.rdl"))
                    GraphClient.CreateRelationship(reportServerNode, new ReportServerContainsReport(reportVisitor.Visit(report.FullName)));
            }
            else if (arguments.ContainsKey("dotnet"))
            {
                var projectVisitor = new DotNetProjectVisitor(GraphClient);
                projectVisitor.Visit(arguments["dotnet"]);
            }
        }

        static NodeReference<DatabaseServer> GetDatabaseServerNode(string serverName)
        {
            var query = GraphClient.Cypher
                .Start(new { root = GraphClient.RootNode })
                .Match("root-[:ROOT_CONTAINS_DATABASESERVER]->databaseserver")
                .Where(string.Format("databaseserver.Id='{0}'", serverName))
                .Return<Node<DatabaseServer>>("databaseserver");

            var results = query.Results.ToList();

            if (results.Count() == 1)
                return results.First().Reference;

            var databaseServerNode = GraphClient.Create(new DatabaseServer
                {
                    Id = serverName,
                    Name = serverName
                });

            GraphClient.CreateRelationship(GraphClient.RootNode, new RootContainsDatabaseServer(databaseServerNode));

            return databaseServerNode;
        }

        static NodeReference<ReportServer> GetReportServerNode(string serverName)
        {
            var query = GraphClient.Cypher
                .Start(new { root = GraphClient.RootNode })
                .Match("root-[:ROOT_CONTAINS_REPORTSERVER]->reportserver")
                .Where(string.Format("reportserver.Id='{0}'", serverName))
                .Return<Node<ReportServer>>("reportserver");

            var results = query.Results.ToList();

            if (results.Count() == 1)
                return results.First().Reference;

            var reportServerNode = GraphClient.Create(new ReportServer
            {
                Id = serverName,
                Name = serverName
            });

            GraphClient.CreateRelationship(GraphClient.RootNode, new RootContainsReportServer(reportServerNode));

            return reportServerNode;
        }
    }
}
