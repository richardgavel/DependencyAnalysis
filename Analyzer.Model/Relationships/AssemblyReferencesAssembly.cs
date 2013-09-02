using Analyzer.Model.Nodes;
using Neo4jClient;

namespace Analyzer.Model.Relationships
{
    public class AssemblyReferencesAssembly : AbstractReferencesRelationship, IRelationshipAllowingSourceNode<Assembly>, IRelationshipAllowingTargetNode<Assembly>
    {
        public const string TypeKey = "ASSEMBLY_REFERENCES_ASSEMBLY";

        public AssemblyReferencesAssembly(NodeReference property)
            : base(property)
        {
        }

        public override string RelationshipTypeKey
        {
            get { return TypeKey; }
        }
    }
}
