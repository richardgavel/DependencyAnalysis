using Analyzer.Database.Visitors;
using Analyzer.Model.Nodes;
using Analyzer.Model.Relationships;
using Analyzer.Reflection.Visitors;
using Analyzer.Reports.Visitors;
using Microsoft.SqlServer.Dac.Model;
using Neo4jClient;
using NUnit.Framework;
using Roslyn.Compilers;
using Roslyn.Compilers.CSharp;
using Roslyn.Services;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CodeAnalyzer.Harness
{
    [TestFixture]
    public class Tests
    {
        private GraphClient client;

        [SetUp]
        public void SetUp()
        {
            client = new GraphClient(new Uri("http://localhost:7474/db/data"));
            client.Connect();
        }

        [Test]
        public void VisitDatabaseServers()
        {
            var databaseVisitor = new DatabaseVisitor(client);

            var db01 = new DatabaseServer
            {
                Id = "DB01VPRD",
                Name = "DB01VPRD"
            };

            var db02 = new DatabaseServer
            {
                Id = "DB02VPRD",
                Name = "DB02VPRD"
            };

            var db01Node = client.Create(db01);
            client.CreateRelationship(client.RootNode, new RootContainsDatabaseServer(db01Node));
            client.CreateRelationship(db01Node, new DatabaseServerContainsDatabase(databaseVisitor.Visit(@"C:\Echo Development\Analyzer Artifacts\DACPAC\API_DB.PROD.dacpac")));
            client.CreateRelationship(db01Node, new DatabaseServerContainsDatabase(databaseVisitor.Visit(@"C:\Echo Development\Analyzer Artifacts\DACPAC\CarrierCapacity.PROD.dacpac")));
            client.CreateRelationship(db01Node, new DatabaseServerContainsDatabase(databaseVisitor.Visit(@"C:\Echo Development\Analyzer Artifacts\DACPAC\EchoLogin2.PROD.dacpac")));
            client.CreateRelationship(db01Node, new DatabaseServerContainsDatabase(databaseVisitor.Visit(@"C:\Echo Development\Analyzer Artifacts\DACPAC\EchoOptimizer.PROD.dacpac")));
            client.CreateRelationship(db01Node, new DatabaseServerContainsDatabase(databaseVisitor.Visit(@"C:\Echo Development\Analyzer Artifacts\DACPAC\EchoOptimizerImages.PROD.dacpac")));
            client.CreateRelationship(db01Node, new DatabaseServerContainsDatabase(databaseVisitor.Visit(@"C:\Echo Development\Analyzer Artifacts\DACPAC\EchoQuote.PROD.dacpac")));
            client.CreateRelationship(db01Node, new DatabaseServerContainsDatabase(databaseVisitor.Visit(@"C:\Echo Development\Analyzer Artifacts\DACPAC\EDIStaging.PROD.dacpac")));
            client.CreateRelationship(db01Node, new DatabaseServerContainsDatabase(databaseVisitor.Visit(@"C:\Echo Development\Analyzer Artifacts\DACPAC\RateIQ.PROD.dacpac")));
            client.CreateRelationship(db01Node, new DatabaseServerContainsDatabase(databaseVisitor.Visit(@"C:\Echo Development\Analyzer Artifacts\DACPAC\RateIQ2.PROD.dacpac")));
            client.CreateRelationship(db01Node, new DatabaseServerContainsDatabase(databaseVisitor.Visit(@"C:\Echo Development\Analyzer Artifacts\DACPAC\SearchRepository.PROD.dacpac")));
            
            var db02Node = client.Create(db02);
            client.CreateRelationship(client.RootNode, new RootContainsDatabaseServer(db02Node));
            client.CreateRelationship(db02Node, new DatabaseServerContainsDatabase(databaseVisitor.Visit(@"C:\Echo Development\Analyzer Artifacts\DACPAC\EchoOptimizer.DB02.PROD.dacpac")));
        }

        [Test]
        public void VisitDatabaseServersForDependencies()
        {
            var databaseVisitor = new StoredProcedureDependencyVisitor(client);

            databaseVisitor.Visit(@"C:\Echo Development\Analyzer Artifacts\DACPAC\API_DB.PROD.dacpac");
            databaseVisitor.Visit(@"C:\Echo Development\Analyzer Artifacts\DACPAC\CarrierCapacity.PROD.dacpac");
            databaseVisitor.Visit(@"C:\Echo Development\Analyzer Artifacts\DACPAC\EchoLogin2.PROD.dacpac");
            databaseVisitor.Visit(@"C:\Echo Development\Analyzer Artifacts\DACPAC\EchoOptimizer.PROD.dacpac");
            databaseVisitor.Visit(@"C:\Echo Development\Analyzer Artifacts\DACPAC\EchoOptimizerImages.PROD.dacpac");
            databaseVisitor.Visit(@"C:\Echo Development\Analyzer Artifacts\DACPAC\EchoQuote.PROD.dacpac");
            databaseVisitor.Visit(@"C:\Echo Development\Analyzer Artifacts\DACPAC\EDIStaging.PROD.dacpac");
            databaseVisitor.Visit(@"C:\Echo Development\Analyzer Artifacts\DACPAC\RateIQ.PROD.dacpac");
            databaseVisitor.Visit(@"C:\Echo Development\Analyzer Artifacts\DACPAC\RateIQ2.PROD.dacpac");
            databaseVisitor.Visit(@"C:\Echo Development\Analyzer Artifacts\DACPAC\SearchRepository.PROD.dacpac");

            databaseVisitor.Visit(@"C:\Echo Development\Analyzer Artifacts\DACPAC\EchoOptimizer.DB02.PROD.dacpac");
        }

        [Test]
        public void VisitReportServer()
        {
            var reportServer = new ReportServer
            {
                Id = "FRS",
                Name = "FRS"
            };

            var reportServerNode = client.Create(reportServer);
            client.CreateRelationship(client.RootNode, new RootContainsReportServer(reportServerNode));

            var directory = new DirectoryInfo(@"C:\Echo Development\Analyzer Artifacts\RDL");
            var reportVisitor = new ReportVisitor(client);

            foreach (var report in directory.EnumerateFiles("*.rdl"))
            {
                var reportNode = reportVisitor.Visit(report.FullName);
                client.CreateRelationship(reportServerNode, new ReportServerContainsReport(reportNode));
            }
        }

        [Test]
        public void VisitAssembliesInDirectory()
        {
            var directory = new DirectoryInfo(@"C:\Echo Development\Analyzer Artifacts\Assemblies");
            var assemblyVisitor = new AssemblyVisitor(client);

            foreach (var assembly in directory.EnumerateFiles("*.dll"))
            {
                Console.WriteLine(assembly.FullName);
                var assemblyNode = assemblyVisitor.Visit(assembly.FullName);
                client.CreateRelationship(client.RootNode, new RootContainsAssembly(assemblyNode));
            }
        }

        [Test]
        public void VisitAssembliesInDirectoryAssemblyDependency()
        {
            var directory = new DirectoryInfo(@"C:\Echo Development\Analyzer Artifacts\Assemblies");
            var assemblyVisitor = new AssemblyDependencyVisitor(client);

            foreach (var assembly in directory.EnumerateFiles("*.dll"))
            {
                Console.WriteLine(assembly.FullName);
                assemblyVisitor.Visit(assembly.FullName);
            }
        }

        [Test]
        public void RoslynTest()
        {
            var sourceFiles = new DirectoryInfo(@"C:\Echo Development\Echo.Optimizer.BusinessLayer.Trunk.Export\");
            var searchPaths = ReadOnlyArray.OneOrZero(@"C:\Echo Development\BusinessLayer References");

            var syntaxTrees = sourceFiles.EnumerateFiles("*.cs", SearchOption.AllDirectories).Select(x => SyntaxTree.ParseFile(x.FullName));
            var references = new List<MetadataReference>();
            references.Add(MetadataReference.CreateAssemblyReference(typeof(object).Assembly.FullName));

            var fileResolver = new FileResolver(searchPaths, searchPaths, Environment.CurrentDirectory, arch => true, CultureInfo.CurrentCulture);

            var compilation = Compilation.Create("OptimizerBusinessLayer", null, syntaxTrees, references, fileResolver, null);

            //var root = tree.GetRoot();
            //var saveWarehouses = root.DescendantNodes().OfType<MethodDeclarationSyntax>().Where(x => x.Identifier.ValueText == "SaveWareHouse").ToList();
        }

        [Test]
        public void RoslynTest2()
        {
            var project = Solution.LoadStandAloneProject(@"C:\Echo Development\Echo.Optimizer.BusinessLayer.Trunk.Export\OptimizerBusinessLayer.csproj");
            var searchPaths = ReadOnlyArray.OneOrZero(@"C:\Echo Development\BusinessLayer References");
            var fileResolver = new FileResolver(searchPaths, searchPaths, Environment.CurrentDirectory, arch => true, CultureInfo.CurrentCulture);
            project = project.UpdateFileResolver(fileResolver);
            var compilation = project.GetCompilation();
        }
    }
}
