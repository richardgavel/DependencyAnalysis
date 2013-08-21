using Analyzer.Model.Nodes;
using Neo4jClient;

namespace Analyzer.Model.Relationships
{
    public class DatabaseServerContainsJob : Relationship, IRelationshipAllowingSourceNode<DatabaseServer>, IRelationshipAllowingTargetNode<Job>
    {
        public const string TypeKey = "DATABASESERVER_CONTAINS_JOB";

        public DatabaseServerContainsJob(NodeReference property)
            : base(property)
        {
        }

        public override string RelationshipTypeKey
        {
            get { return TypeKey; }
        }
    }
}
