﻿using CodeAnalyzer.Entities;
using Neo4jClient;

namespace CodeAnalyzer.Relationships
{
    public class DatabaseContainsView : Relationship, IRelationshipAllowingSourceNode<Database>, IRelationshipAllowingTargetNode<View>
    {
        public const string TypeKey = "DATABASE_CONTAINS_VIEW";

        public DatabaseContainsView(NodeReference property)
            : base(property)
        {
        }

        public override string RelationshipTypeKey
        {
            get { return TypeKey; }
        }
    }
}
