using Analyzer.Model.Nodes;
using Neo4jClient;

namespace Analyzer.Model.Relationships
{
    public class StoredProcedureReferencesColumn : AbstractReferencesRelationship, IRelationshipAllowingSourceNode<StoredProcedure>, IRelationshipAllowingTargetNode<Column>
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
