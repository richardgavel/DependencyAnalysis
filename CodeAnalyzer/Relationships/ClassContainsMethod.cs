using CodeAnalyzer.Entities;
using Neo4jClient;

namespace CodeAnalyzer.Relationships
{
    public class ClassContainsMethod : Relationship, IRelationshipAllowingSourceNode<Class>, IRelationshipAllowingTargetNode<Method>
    {
        public const string TypeKey = "CLASS_CONTAINS_METHOD";

        public ClassContainsMethod(NodeReference property)
            : base(property)
        {
        }

        public override string RelationshipTypeKey
        {
            get { return TypeKey; }
        }
    }
}
