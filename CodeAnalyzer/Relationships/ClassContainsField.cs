using CodeAnalyzer.Entities;
using Neo4jClient;

namespace CodeAnalyzer.Relationships
{
    public class ClassContainsField : Relationship, IRelationshipAllowingSourceNode<Class>, IRelationshipAllowingTargetNode<Field>
    {
        public const string TypeKey = "CLASS_CONTAINS_FIELD";

        public ClassContainsField(NodeReference property)
            : base(property)
        {
        }

        public override string RelationshipTypeKey
        {
            get { return TypeKey; }
        }
    }
}
