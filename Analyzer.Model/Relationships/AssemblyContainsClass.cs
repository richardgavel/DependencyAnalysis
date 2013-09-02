using Analyzer.Model.Nodes;
using Neo4jClient;

namespace Analyzer.Model.Relationships
{
    public class AssemblyContainsClass : AbstractContainsRelationship, IRelationshipAllowingSourceNode<Assembly>, IRelationshipAllowingTargetNode<Class>
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
