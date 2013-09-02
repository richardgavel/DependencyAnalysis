using Analyzer.Model.Nodes;
using Neo4jClient;

namespace Analyzer.Model.Relationships
{
    public class TableReplicatesToTable : AbstractReferencesRelationship, IRelationshipAllowingSourceNode<Table>, IRelationshipAllowingTargetNode<Table>
    {
        public const string TypeKey = "TABLE_REPLICATESTO_TABLE";

        public TableReplicatesToTable(NodeReference property)
            : base(property)
        {
        }

        public override string RelationshipTypeKey
        {
            get { return TypeKey; }
        }
    }
}
