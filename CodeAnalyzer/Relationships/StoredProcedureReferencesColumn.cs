using CodeAnalyzer.Entities;
using Neo4jClient;

namespace CodeAnalyzer.Relationships
{
    public class StoredProcedureReferencesColumn : Relationship, IRelationshipAllowingSourceNode<StoredProcedure>, IRelationshipAllowingTargetNode<Column>
    {
        public const string TypeKey = "STOREDPROCEDURE_REFERENCES_COLUMN";

        public StoredProcedureReferencesColumn(NodeReference property)
            : base(property)
        {
        }

        public override string RelationshipTypeKey
        {
            get { return TypeKey; }
        }
    }
}
