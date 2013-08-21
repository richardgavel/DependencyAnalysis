using Analyzer.Model.Nodes;
using Neo4jClient;

namespace Analyzer.Model.Relationships
{
    public class InterfaceContainsMethod : Relationship, IRelationshipAllowingSourceNode<Interface>, IRelationshipAllowingTargetNode<Method>
    {
        public const string TypeKey = "INTERFACE_CONTAINS_METHOD";

        public InterfaceContainsMethod(NodeReference property)
            : base(property)
        {
        }

        public override string RelationshipTypeKey
        {
            get { return TypeKey; }
        }
    }
}
