using CodeAnalyzer.Entities;
using Neo4jClient;

namespace CodeAnalyzer.Relationships
{
    public class AssemblyContainsClass : Relationship, IRelationshipAllowingSourceNode<Assembly>, IRelationshipAllowingTargetNode<Class>
    {
        public const string TypeKey = "ASSEMBLY_CONTAINS_CLASS";

        public AssemblyContainsClass(NodeReference property)
            : base(property)
        {
        }

        public override string RelationshipTypeKey
        {
            get { return TypeKey; }
        }
    }
}
