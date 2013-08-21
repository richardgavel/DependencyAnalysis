using Analyzer.Model.Nodes;
using Neo4jClient;

namespace Analyzer.Model.Relationships
{
    public class InterfaceContainsProperty : Relationship, IRelationshipAllowingSourceNode<Interface>, IRelationshipAllowingTargetNode<Property>
    {
        public const string TypeKey = "CLASS_CONTAINS_PROPERTY";

        public InterfaceContainsProperty(NodeReference property)
            : base(property)
        {
        }

        public override string RelationshipTypeKey
        {
            get { return TypeKey; }
        }
    }
}
