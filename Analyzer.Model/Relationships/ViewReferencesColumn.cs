using Analyzer.Model.Nodes;
using Neo4jClient;

namespace Analyzer.Model.Relationships
{
    public class ViewReferencesColumn : AbstractReferencesRelationship, IRelationshipAllowingSourceNode<View>, IRelationshipAllowingTargetNode<Column>
    {
        public const string TypeKey = "VIEW_REFERENCES_COLUMN";

        public ViewReferencesColumn(NodeReference property)
            : base(property)
        {
        }

        public override string RelationshipTypeKey
        {
            get { return TypeKey; }
        }
    }
}
