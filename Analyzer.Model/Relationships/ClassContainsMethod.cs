using Analyzer.Model.Nodes;
using Neo4jClient;

namespace Analyzer.Model.Relationships
{
    public class ClassContainsMethod : AbstractContainsRelationship, IRelationshipAllowingSourceNode<Class>, IRelationshipAllowingTargetNode<Method>
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
