using CodeAnalyzer.Entities;
using Neo4jClient;

namespace CodeAnalyzer.Relationships
{
    public class AssemblyContainsInterface : Relationship, IRelationshipAllowingSourceNode<Assembly>, IRelationshipAllowingTargetNode<Interface>
    {
        public const string TypeKey = "ASSEMBLY_CONTAINS_INTERFACE";

        public AssemblyContainsInterface(NodeReference property)
            : base(property)
        {
        }

        public override string RelationshipTypeKey
        {
            get { return TypeKey; }
        }
    }
}
