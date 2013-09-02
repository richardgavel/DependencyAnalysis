using Analyzer.Model.Nodes;
using Neo4jClient;

namespace Analyzer.Model.Relationships
{
    public class BizTalkServerContainsBizTalkApplication : AbstractContainsRelationship, IRelationshipAllowingSourceNode<BizTalkServer>, IRelationshipAllowingTargetNode<BizTalkApplication>
    {
        public const string TypeKey = "BIZTALKSERVER_CONTAINS_BIZTALKAPPLICATION";

        public BizTalkServerContainsBizTalkApplication(NodeReference property)
            : base(property)
        {
        }

        public override string RelationshipTypeKey
        {
            get { return TypeKey; }
        }
    }
}
