using Analyzer.Web.Models;
using Neo4jClient;
using Neo4jClient.Cypher;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace Analyzer.Web.Controllers
{
    public class NodeController : ApiController
    {
        private static readonly Dictionary<long, ResultModel> Cache = new Dictionary<long, ResultModel>();

        private readonly GraphClient _graphClient;

        public NodeController()
        {
            _graphClient = new GraphClient(new Uri("http://localhost:7474/db/data"));
            _graphClient.Connect();
        }

        public ResultModel Get(long id)
        {
            //if (Cache.ContainsKey(id))
            //    return Cache[id];

            var result = GetResultModel(id);
            //Cache.Add(id, result);
            return result;
        }

        [HttpGet]
        public object Search(string term)
        {
            var query = _graphClient.Cypher
                .Start(new { n = All.Nodes })
                .Where(string.Format("n.Name! =~ '.*{0}.*'", term))
                .Return<Node<string>>("n");

            return query.Results
                .Select(x => new {x.Reference.Id, Data = JObject.Parse(x.Data)})
                .Select(x => new NodeModel
                {
                    Id = x.Id,
                    FriendlyId = x.Data["Id"].Value<string>(),
                    Name = x.Data["Name"].Value<string>(),
                    Type = x.Data["Type"].Value<string>(),
                })
                .OrderBy(x => x.Type);
        }

        private ResultModel GetResultModel(long id)
        {
            var target = GetNode(id);

            var model = new ResultModel
            {
                Contains = GetContains(target.Item1.Reference),
                ReferencedBy = GetReferencedBy(target.Item1.Reference),
                References = GetReferences(target.Item1.Reference),
                Target = target.Item2
            };

            var query = _graphClient.Cypher
                .Start(new { root = _graphClient.RootNode, target = target.Item1.Reference })
                .Match("path = shortestPath(root-[*]-target)")
                .Return<string>("path");

            var path = JObject.Parse(query.Results.First());
            var pathNodes = (JArray)path["nodes"];

            model.Ancestry = pathNodes.Select(x => GetNode(long.Parse(x.Value<string>().Split('/').Last())).Item2).ToList();

            return model;
        }

        private List<NodeModel> GetContains(IAttachedReference sourceReference)
        {
            var query = _graphClient.Cypher
                .Start(new { source = sourceReference })
                .Match("source-[relationship]->target")
                .Where("relationship.RelationshipType='CONTAINS'")
                .Return<Node<string>>("target");

            var results = query.Results.Select(x => GetNode(x.Reference.Id).Item2).OrderBy(x => x.Type + ":" + x.Name).ToList();

            return results;
        }

        private List<NodeModel> GetReferences(IAttachedReference sourceReference)
        {
            var query = _graphClient.Cypher
                .Start(new { source = sourceReference })
                .Match("source-[relationship]->target")
                .Where("relationship.RelationshipType='REFERENCES'")
                .Return<Node<string>>("target");

            var results = query.Results.Select(x => GetNode(x.Reference.Id).Item2).OrderBy(x => x.Type + ":" + x.Name).ToList();

            return results;
        }

        private List<NodeModel> GetReferencedBy(IAttachedReference targetReference)
        {
            var query = _graphClient.Cypher
                .Start(new { target = targetReference })
                .Match("source-[relationship]->target")
                .Where("relationship.RelationshipType='REFERENCES'")
                .Return<Node<string>>("source");

            var results = query.Results.Select(x => GetNode(x.Reference.Id).Item2).OrderBy(x => x.Type + ":" + x.Name).ToList();

            return results;
        }

        private Tuple<Node<string>, NodeModel> GetNode(long id)
        {
            var node = _graphClient.Get<string>((NodeReference)id);
            dynamic data = JObject.Parse(node.Data);

            if (id == 0)
                return new Tuple<Node<string>, NodeModel>(node, new NodeModel
                    {
                        Id = 0,
                        FriendlyId = "Root",
                        Name = "Root",
                        Type = "Root"
                    });

            return new Tuple<Node<string>, NodeModel>(node, new NodeModel
            {
                Id = node.Reference.Id,
                FriendlyId = data.Id,
                Name = data.Name,
                Type = data.Type
            });
        }
    }
}
