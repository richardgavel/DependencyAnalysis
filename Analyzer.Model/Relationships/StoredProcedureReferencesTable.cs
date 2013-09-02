using Analyzer.Model.Nodes;
using Neo4jClient;

namespace Analyzer.Model.Relationships
{
    public class StoredProcedureReferencesTable : AbstractReferencesRelationship, IRelationshipAllowingSourceNode<StoredProcedure>, IRelationshipAllowingTargetNode<Table>
    {
        public const string TypeKey = "STOREDPROCEDURE_REFERENCES_TABLE";

        public StoredProcedureReferencesTable(NodeReference property)
            : base(property)
        {
        }

        public override string RelationshipTypeKey
        {
            get { return TypeKey; }
        }
    }
}
