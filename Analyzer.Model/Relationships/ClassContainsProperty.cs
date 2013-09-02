using Analyzer.Model.Nodes;
using Neo4jClient;

namespace Analyzer.Model.Relationships
{
    public class ClassContainsProperty : AbstractContainsRelationship, IRelationshipAllowingSourceNode<Class>, IRelationshipAllowingTargetNode<Property>
    {
        public const string TypeKey = "CLASS_CONTAINS_PROPERTY";

        public ClassContainsProperty(NodeReference property)
            : base(property)
        {
        }

        public override string RelationshipTypeKey
        {
            get { return TypeKey; }
        }
    }
}
