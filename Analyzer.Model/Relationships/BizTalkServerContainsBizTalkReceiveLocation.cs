using Analyzer.Model.Nodes;
using Neo4jClient;

namespace Analyzer.Model.Relationships
{
    public class BizTalkServerContainsBizTalkReceiveLocation : AbstractContainsRelationship, IRelationshipAllowingSourceNode<BizTalkApplication>, IRelationshipAllowingTargetNode<BizTalkReceiveLocation>
    {
        public const string TypeKey = "BIZTALKAPPLICATION_CONTAINS_BIZTALKRECEIVELOCATION";

        public BizTalkServerContainsBizTalkReceiveLocation(NodeReference property)
            : base(property)
        {
        }

        public override string RelationshipTypeKey
        {
            get { return TypeKey; }
        }
    }
}
