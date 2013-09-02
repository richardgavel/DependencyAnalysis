using Analyzer.Model.Nodes;
using Neo4jClient;

namespace Analyzer.Model.Relationships
{
    public class MethodInvokesMethod : AbstractReferencesRelationship, IRelationshipAllowingSourceNode<Method>, IRelationshipAllowingTargetNode<Method>
    {
        public const string TypeKey = "METHOD_INVOKES_METHOD";

        public MethodInvokesMethod(NodeReference property)
            : base(property)
        {
        }

        public override string RelationshipTypeKey
        {
            get { return TypeKey; }
        }
    }
}
