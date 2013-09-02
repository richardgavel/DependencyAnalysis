using Analyzer.Model.Nodes;
using Neo4jClient;

namespace Analyzer.Model.Relationships
{
    public class DatabaseContainsTable : AbstractContainsRelationship, IRelationshipAllowingSourceNode<Database>, IRelationshipAllowingTargetNode<Table>
    {
        public const string TypeKey = "DATABASE_CONTAINS_TABLE";

        public DatabaseContainsTable(NodeReference property)
            : base(property)
        {
        }

        public override string RelationshipTypeKey
        {
            get { return TypeKey; }
        }
    }
}
