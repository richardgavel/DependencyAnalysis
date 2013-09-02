using Neo4jClient;

namespace Analyzer.Model.Relationships
{
    public abstract class AbstractContainsRelationship : Relationship<RelationshipPayload>
    {
        public AbstractContainsRelationship(NodeReference property)
            : base(property, new RelationshipPayload { RelationshipType = "CONTAINS" })
        { }
    }
}
