using Analyzer.Model.Nodes;
using Neo4jClient;

namespace Analyzer.Model.Relationships
{
    public class ReportReferencesTable : AbstractReferencesRelationship, IRelationshipAllowingSourceNode<Report>, IRelationshipAllowingTargetNode<Table>
    {
        public const string TypeKey = "REPORT_REFERENCES_TABLE";

        public ReportReferencesTable(NodeReference property)
            : base(property)
        {
        }

        public override string RelationshipTypeKey
        {
            get { return TypeKey; }
        }
    }
}
