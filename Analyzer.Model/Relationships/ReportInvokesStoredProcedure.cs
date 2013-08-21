using Analyzer.Model.Nodes;
using Neo4jClient;

namespace Analyzer.Model.Relationships
{
    public class ReportInvokesStoredProcedure : Relationship, IRelationshipAllowingSourceNode<Report>, IRelationshipAllowingTargetNode<StoredProcedure>
    {
        public const string TypeKey = "REPORT_INVOKES_STOREDPROCEDURE";

        public ReportInvokesStoredProcedure(NodeReference property)
            : base(property)
        {
        }

        public override string RelationshipTypeKey
        {
            get { return TypeKey; }
        }
    }
}
