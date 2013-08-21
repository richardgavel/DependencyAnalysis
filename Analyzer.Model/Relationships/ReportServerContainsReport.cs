using Analyzer.Model.Nodes;
using Neo4jClient;

namespace Analyzer.Model.Relationships
{
    public class ReportServerContainsReport : Relationship, IRelationshipAllowingSourceNode<ReportServer>, IRelationshipAllowingTargetNode<Report>
    {
        public const string TypeKey = "REPORTSERVER_CONTAINS_REPORT";

        public ReportServerContainsReport(NodeReference property)
            : base(property)
        {
        }

        public override string RelationshipTypeKey
        {
            get { return TypeKey; }
        }
    }
}
