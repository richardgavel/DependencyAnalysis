using Analyzer.Model.Nodes;
using Neo4jClient;

namespace Analyzer.Model.Relationships
{
    public class RootContainsReportServer : Relationship, IRelationshipAllowingSourceNode<RootNode>, IRelationshipAllowingTargetNode<ReportServer>
    {
        public const string TypeKey = "ROOT_CONTAINS_REPORTSERVER";

        public RootContainsReportServer(NodeReference property)
            : base(property)
        {
        }

        public override string RelationshipTypeKey
        {
            get { return TypeKey; }
        }
    }
}
