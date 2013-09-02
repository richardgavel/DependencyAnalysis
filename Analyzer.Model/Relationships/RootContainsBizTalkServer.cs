using Analyzer.Model.Nodes;
using Neo4jClient;

namespace Analyzer.Model.Relationships
{
    public class RootContainsBizTalkServer : AbstractContainsRelationship, IRelationshipAllowingSourceNode<RootNode>, IRelationshipAllowingTargetNode<BizTalkServer>
    {
        public const string TypeKey = "ROOT_CONTAINS_BIZTALKSERVER";

        public RootContainsBizTalkServer(NodeReference property)
            : base(property)
        {
        }

        public override string RelationshipTypeKey
        {
            get { return TypeKey; }
        }
    }
}
