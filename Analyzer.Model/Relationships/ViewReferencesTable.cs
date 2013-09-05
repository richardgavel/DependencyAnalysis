using Analyzer.Model.Nodes;
using Neo4jClient;

namespace Analyzer.Model.Relationships
{
    public class ViewReferencesTable : AbstractReferencesRelationship, IRelationshipAllowingSourceNode<View>, IRelationshipAllowingTargetNode<Table>
    {
        public const string TypeKey = "VIEW_REFERENCES_TABLE";

        public ViewReferencesTable(NodeReference property)
            : base(property)
        {
        }

        public override string RelationshipTypeKey
        {
            get { return TypeKey; }
        }
    }
}
