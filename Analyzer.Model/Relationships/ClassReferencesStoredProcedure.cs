using Analyzer.Model.Nodes;
using Neo4jClient;

namespace Analyzer.Model.Relationships
{
    public class ClassReferencesStoredProcedure : Relationship, IRelationshipAllowingSourceNode<Class>, IRelationshipAllowingTargetNode<StoredProcedure>
    {
        public const string TypeKey = "CLASS_REFERENCES_STOREDPROCEDURE";

        public ClassReferencesStoredProcedure(NodeReference property)
            : base(property)
        {
        }

        public override string RelationshipTypeKey
        {
            get { return TypeKey; }
        }
    }
}
