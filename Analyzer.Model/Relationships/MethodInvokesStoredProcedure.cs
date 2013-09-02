using Analyzer.Model.Nodes;
using Neo4jClient;

namespace Analyzer.Model.Relationships
{
    public class MethodInvokesStoredProcedure : AbstractReferencesRelationship, IRelationshipAllowingSourceNode<Method>, IRelationshipAllowingTargetNode<StoredProcedure>
    {
        public const string TypeKey = "METHOD_INVOKES_STOREDPROCEDURE";

        public MethodInvokesStoredProcedure(NodeReference property)
            : base(property)
        {
        }

        public override string RelationshipTypeKey
        {
            get { return TypeKey; }
        }
    }
}
