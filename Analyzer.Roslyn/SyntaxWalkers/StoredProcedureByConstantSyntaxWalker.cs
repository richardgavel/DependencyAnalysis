using Roslyn.Compilers.CSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Analyzer.Roslyn.SyntaxWalkers
{
    public class StoredProcedureByConstantSyntaxWalker : SyntaxWalker
    {
        public override void VisitFieldDeclaration(FieldDeclarationSyntax node)
        {
            base.VisitFieldDeclaration(node);
        }

        public override void VisitLiteralExpression(LiteralExpressionSyntax node)
        {
            if (node.Token.ValueText == "AT_UpdateAutoTenderStatus")
            {
            }

            base.VisitLiteralExpression(node);
        }

        public override void VisitInvocationExpression(InvocationExpressionSyntax node)
        {
            if (node.Expression is IdentifierNameSyntax)
            {
                if ((node.Expression as IdentifierNameSyntax).Identifier.ValueText == "ExecuteScalarCmd")
                {
                }
            }

            base.VisitInvocationExpression(node);
        }
    }
}
