using Analyzer.Model.Nodes;
using Neo4jClient;

namespace Analyzer.Model.Relationships
{
    public class DatabaseContainsUserDefinedFunction : AbstractContainsRelationship, IRelationshipAllowingSourceNode<Database>, IRelationshipAllowingTargetNode<UserDefinedFunction>
    {
        public const string TypeKey = "DATABASE_CONTAINS_USERDEFINEDFUNCTION";

        public DatabaseContainsUserDefinedFunction(NodeReference property)
            : base(property)
        {
        }

        public override string RelationshipTypeKey
        {
            get { return TypeKey; }
        }
    }
}
