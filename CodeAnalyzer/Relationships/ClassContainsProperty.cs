using CodeAnalyzer.Entities;
using Neo4jClient;

namespace CodeAnalyzer.Relationships
{
    public class ClassContainsProperty : Relationship, IRelationshipAllowingSourceNode<Class>, IRelationshipAllowingTargetNode<Property>
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
