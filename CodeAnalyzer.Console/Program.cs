using Roslyn.Compilers;
using Roslyn.Compilers.CSharp;
using Roslyn.Services;
using System;
using System.IO;
using System.Globalization;
using System.Linq;
using Roslyn.Compilers.Common;
using Analyzer.Roslyn;
using Neo4jClient;
using Analyzer.Model.Nodes;
using Analyzer.Roslyn.SyntaxWalkers;
using Neo4jClient.Cypher;
using System.Net.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace CodeAnalyzer.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            //var project = Solution.LoadStandAloneProject(@"C:\Echo Development\Echo.Optimizer.BusinessLayer.Trunk.Export\OptimizerBusinessLayer.csproj");
            //var searchPaths = ReadOnlyArray.OneOrZero(@"C:\Echo Development\BusinessLayer References");
            //var fileResolver = new FileResolver(searchPaths, searchPaths, null, arch => true, CultureInfo.CurrentCulture);
            //project = project.UpdateFileResolver(fileResolver);
            //var compilation = project.GetCompilation();


            //var newWalker = new StoredProcedureByConstantSyntaxWalker();
            //foreach (var tree in compilation.SyntaxTrees)
            //{
            //    newWalker.Visit(tree.GetRoot() as SyntaxNode);
            //}
            //System.Console.WriteLine("*** DONE ***");
            //System.Console.ReadLine();
            //return;

            var client = new GraphClient(new Uri("http://localhost:7474/db/data"));
            client.Connect();

            //FindMethodInvocationLinks(client);

            PrintRelationshipTree(new HttpClient(), "http://localhost:7474/db/data/node/0", 0, new List<string>());

            System.Console.WriteLine("*** DONE ***");
            //System.Console.ReadLine();
        }

        static void FindMethodInvocationLinks(GraphClient client)
        {
            var project = Solution.LoadStandAloneProject(@"C:\Echo Development\Analyzer Artifacts\Code\Echo.Optimizer.WebUI.Trunk\Echo.Optimizer.WebUI.csproj");
            var searchPaths = ReadOnlyArray.OneOrZero((string)null);
            var fileResolver = new FileResolver(searchPaths, searchPaths, null, arch => true, CultureInfo.CurrentCulture);
            project = project.UpdateFileResolver(fileResolver);
            var compilation = project.GetCompilation();
            foreach (var tree in compilation.SyntaxTrees)
            {
                var methodInvocationWalker = new MethodInvocationWalker(client, compilation.GetSemanticModel(tree));
                methodInvocationWalker.Visit(tree.GetRoot() as SyntaxNode);

                var methodInvokedStoredProcedureWalker = new MethodInvokesStoredProcedureWalker(client, compilation.GetSemanticModel(tree));
                methodInvokedStoredProcedureWalker.Visit(tree.GetRoot() as SyntaxNode);
            }
        }

        static void PrintRelationshipTree(HttpClient httpClient, string uri, int indentLevel, List<string> urisFollowed)
        {
            httpClient.GetStringAsync(uri)
                .ContinueWith(node =>
                {
                    urisFollowed.Add(uri);
                    dynamic jnode = JObject.Parse(node.Result);
                    System.Console.WriteLine("{0}{1}", new String(' ', indentLevel * 3), jnode.data.Id);

                    httpClient.GetStringAsync((string)jnode["outgoing_relationships"])
                        .ContinueWith(relationship =>
                        {
                            dynamic jrelationships = JArray.Parse(relationship.Result);
                            foreach (dynamic jrelationship in jrelationships)
                            {
                                System.Console.WriteLine("{0}{1}", new String(' ', indentLevel * 3), jrelationship.type);

                                if (urisFollowed.Contains((string)jrelationship.end))
                                    System.Console.WriteLine("{0}RECURSIVE CALL", new String(' ', indentLevel * 3), jrelationship.type);
                                else
                                    PrintRelationshipTree(httpClient, (string)jrelationship.end, indentLevel + 1, new List<string>(urisFollowed));
                            }
                        })
                        .Wait();
                })
                .Wait();
        }

        static void FindClassStoredProcedureLinks(GraphClient client)
        {
            var query = client.Cypher
                .Start(new { root = client.RootNode })
                .Match("root-[:ROOT_CONTAINS_DATABASESERVER]->server-[:DATABASESERVER_CONTAINS_DATABASE]->database-[:DATABASE_CONTAINS_STOREDPROCEDURE]->storedprocedure")
                .Return<Node<StoredProcedure>>("storedprocedure");

            var project = Solution.LoadStandAloneProject(@"C:\Echo Development\Echo.Optimizer.BusinessLayer.Trunk.Export\OptimizerBusinessLayer.csproj");
            var searchPaths = ReadOnlyArray.OneOrZero(@"C:\Echo Development\BusinessLayer References");
            var fileResolver = new FileResolver(searchPaths, searchPaths, null, arch => true, CultureInfo.CurrentCulture);
            project = project.UpdateFileResolver(fileResolver);
            var compilation = project.GetCompilation();

            var newWalker = new StoredProcedureReferenceWalker(client, query.Results.AsEnumerable());
            foreach (var tree in compilation.SyntaxTrees)
            {
                newWalker.Visit(tree.GetRoot() as SyntaxNode);
            }

            //foreach (var name in query.Results.Select(x => x.Data.Name))
            //    System.Console.WriteLine(name);
        }
    }
}
