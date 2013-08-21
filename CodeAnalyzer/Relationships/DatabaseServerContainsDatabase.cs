using CodeAnalyzer.Entities;
using Neo4jClient;

namespace CodeAnalyzer.Relationships
{
    public class DatabaseServerContainsDatabase : Relationship, IRelationshipAllowingSourceNode<DatabaseServer>, IRelationshipAllowingTargetNode<Database>
    {
        public const string TypeKey = "DATABASESERVER_CONTAINS_DATABASE";

        public DatabaseServerContainsDatabase(NodeReference property)
            : base(property)
        {
        }

        public override string RelationshipTypeKey
        {
            get { return TypeKey; }
        }
    }
}
