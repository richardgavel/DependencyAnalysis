using Analyzer.Model.Nodes;
using Neo4jClient;

namespace Analyzer.Model.Relationships
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
