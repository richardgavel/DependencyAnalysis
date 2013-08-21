using Analyzer.Model.Nodes;
using Neo4jClient;

namespace Analyzer.Model.Relationships
{
    public class TableContainsColumn : Relationship, IRelationshipAllowingSourceNode<Table>, IRelationshipAllowingTargetNode<Column>
    {
        public const string TypeKey = "TABLE_CONTAINS_COLUMN";

        public TableContainsColumn(NodeReference property)
            : base(property)
        {
        }

        public override string RelationshipTypeKey
        {
            get { return TypeKey; }
        }
    }
}
