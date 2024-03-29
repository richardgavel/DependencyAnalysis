﻿using Analyzer.Model.Nodes;
using Neo4jClient;

namespace Analyzer.Model.Relationships
{
    public class DatabaseContainsStoredProcedure : AbstractContainsRelationship, IRelationshipAllowingSourceNode<Database>, IRelationshipAllowingTargetNode<StoredProcedure>
    {
        public const string TypeKey = "DATABASE_CONTAINS_STOREDPROCEDURE";

        public DatabaseContainsStoredProcedure(NodeReference property)
            : base(property)
        {
        }

        public override string RelationshipTypeKey
        {
            get { return TypeKey; }
        }
    }
}
