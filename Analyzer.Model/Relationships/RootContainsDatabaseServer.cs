using Analyzer.Model.Nodes;
using Neo4jClient;

namespace Analyzer.Model.Relationships
{
    public class RootContainsDatabaseServer : Relationship, IRelationshipAllowingSourceNode<RootNode>, IRelationshipAllowingTargetNode<DatabaseServer>
    {
        public const string TypeKey = "ROOT_CONTAINS_DATABASESERVER";

        public RootContainsDatabaseServer(NodeReference property)
            : base(property)
        {
        }

        public override string RelationshipTypeKey
        {
            get { return TypeKey; }
        }
    }
}
