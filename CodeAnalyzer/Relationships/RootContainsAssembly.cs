using CodeAnalyzer.Entities;
using Neo4jClient;

namespace CodeAnalyzer.Relationships
{
    public class RootContainsAssembly : Relationship, IRelationshipAllowingSourceNode<RootNode>, IRelationshipAllowingTargetNode<Assembly>
    {
        public const string TypeKey = "ROOT_CONTAINS_ASSEMBLY";

        public RootContainsAssembly(NodeReference property)
            : base(property)
        {
        }

        public override string RelationshipTypeKey
        {
            get { return TypeKey; }
        }
    }
}
