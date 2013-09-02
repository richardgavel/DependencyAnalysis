using Analyzer.Model.Nodes;
using Neo4jClient;

namespace Analyzer.Model.Relationships
{
    public class ClassImplementsInterface : AbstractReferencesRelationship, IRelationshipAllowingSourceNode<Class>, IRelationshipAllowingTargetNode<Interface>
    {
        public const string TypeKey = "CLASS_IMPLEMENTS_INTERFACE";

        public ClassImplementsInterface(NodeReference property)
            : base(property)
        {
        }

        public override string RelationshipTypeKey
        {
            get { return TypeKey; }
        }
    }
}
