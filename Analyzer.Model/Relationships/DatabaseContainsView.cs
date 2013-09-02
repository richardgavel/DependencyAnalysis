using Analyzer.Model.Nodes;
using Neo4jClient;

namespace Analyzer.Model.Relationships
{
    public class DatabaseContainsView : AbstractContainsRelationship, IRelationshipAllowingSourceNode<Database>, IRelationshipAllowingTargetNode<View>
    {
        public const string TypeKey = "DATABASE_CONTAINS_VIEW";

        public DatabaseContainsView(NodeReference property)
            : base(property)
        {
        }

        public override string RelationshipTypeKey
        {
            get { return TypeKey; }
        }
    }
}
