using CodeAnalyzer.Entities;
using Neo4jClient;

namespace CodeAnalyzer.Relationships
{
    public class DatabaseContainsStoredProcedure : Relationship, IRelationshipAllowingSourceNode<Database>, IRelationshipAllowingTargetNode<StoredProcedure>
    {
        public const string TypeKey = "DATABASE_CONTAINS_STOREDPROCEDURE";

        public DatabaseContainsStoredProcedure(NodeReference property)
            : base(property)
        {
        }

        public override string RelationshipTypeKey
        {
            get { return TypeKey; }
        }
    }
}
