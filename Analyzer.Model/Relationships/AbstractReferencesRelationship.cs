using Neo4jClient;

namespace Analyzer.Model.Relationships
{
    public abstract class AbstractReferencesRelationship : Relationship<RelationshipPayload>
    {
        public AbstractReferencesRelationship(NodeReference property)
            : base(property, new RelationshipPayload { RelationshipType = "REFERENCES" })
        { }
    }
}
